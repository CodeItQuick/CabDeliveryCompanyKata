using System.ComponentModel.DataAnnotations;

namespace Production.WebCabCompany.Models.AccountViewModels;

public class VerifyAuthenticatorCodeViewModel
{
    [Required]
    public string Code { get; set; }

    public string ReturnUrl { get; set; }

    [Display(Name = "Remember this browser?")]
    public bool RememberBrowser { get; set; }

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}