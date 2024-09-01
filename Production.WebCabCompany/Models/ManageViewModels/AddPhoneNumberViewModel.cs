using System.ComponentModel.DataAnnotations;

namespace Production.WebCabCompany.Models.ManageViewModels;

public class AddPhoneNumberViewModel
{
    [Required]
    [Phone]
    [Display(Name = "Phone number")]
    public string PhoneNumber { get; set; }
}