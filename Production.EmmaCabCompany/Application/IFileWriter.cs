namespace Production.EmmaCabCompany;

public interface IFileWriter
{
    public void WriteCabList(string[] cabList);
    public void WriteCustomerList(string[] exportedCustomers);
}