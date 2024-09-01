using Microsoft.Build.Framework;

namespace Production.WebCabCompany.Models.AccountViewModels;

public class UseRecoveryCodeViewModel
{
    [Required]
    public string Code { get; set; }

    public string ReturnUrl { get; set; }
}