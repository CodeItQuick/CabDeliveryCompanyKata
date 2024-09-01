using System.ComponentModel.DataAnnotations;

namespace Production.WebCabCompany.Models.ManageViewModels;

public class DisplayRecoveryCodesViewModel
{
    [Required]
    public IEnumerable<string> Codes { get; set; }

}