// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Production.WebCabCompany.Models;
using Production.WebCabCompany.Models.ManageViewModels;

namespace Production.WebCabCompany.Controllers;

[Authorize]
public class ManageController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger _logger;

    public ManageController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILoggerFactory loggerFactory)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = loggerFactory.CreateLogger<ManageController>();
    }

    //
    // GET: /Manage/Index
    [HttpGet]
    public async Task<IActionResult> Index(ManageMessageId? message = null)
    {
        ViewData["StatusMessage"] =
            message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
            : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
            : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
            : message == ManageMessageId.Error ? "An error has occurred."
            : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
            : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
            : "";

        var user = await GetCurrentUserAsync();
        var model = new IndexViewModel
        {
            HasPassword = await _userManager.HasPasswordAsync(user),
            PhoneNumber = await _userManager.GetPhoneNumberAsync(user),
            TwoFactor = await _userManager.GetTwoFactorEnabledAsync(user),
            Logins = await _userManager.GetLoginsAsync(user),
            BrowserRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user),
            AuthenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user)
        };
        return View(model);
    }

    //
    // GET: /Manage/ChangePassword
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    //
    // POST: /Manage/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await GetCurrentUserAsync();
        if (user != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(3, "User changed their password successfully.");
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
    }

    //
    // GET: /Manage/SetPassword
    [HttpGet]
    public IActionResult SetPassword()
    {
        return View();
    }

    //
    // POST: /Manage/SetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await GetCurrentUserAsync();
        if (user != null)
        {
            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(Index), new { Message = ManageMessageId.SetPasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }
        return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
    }

    #region Helpers

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    public enum ManageMessageId
    {
        AddPhoneSuccess,
        AddLoginSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        Error
    }

    private Task<ApplicationUser> GetCurrentUserAsync()
    {
        return _userManager.GetUserAsync(HttpContext.User);
    }

    #endregion
}