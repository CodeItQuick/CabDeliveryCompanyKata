using System.ComponentModel.DataAnnotations;

namespace Production.WebCabCompany.Models.AccountViewModels;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}