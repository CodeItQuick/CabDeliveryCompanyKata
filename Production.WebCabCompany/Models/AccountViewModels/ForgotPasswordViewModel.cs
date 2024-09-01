using System.ComponentModel.DataAnnotations;

namespace Production.WebCabCompany.Models.AccountViewModels;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}