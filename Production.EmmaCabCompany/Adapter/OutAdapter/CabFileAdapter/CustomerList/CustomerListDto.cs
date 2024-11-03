using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;

[PrimaryKey("Id")]
[Table("CustomerList")]
public class CustomerListDto
{
    public int Id { get; set; }
    [ForeignKey("CustomerId")]
    public List<CustomerDto> Customers { get; set; }
}