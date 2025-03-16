using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;

[PrimaryKey("Id")]
[Table("CustomerList")]
public class CustomerListDto : IdentityClass
{
    public override int Id { get; set; }
    [ForeignKey("CustomerId")]
    public List<CustomerDto> Customers { get; set; }
}