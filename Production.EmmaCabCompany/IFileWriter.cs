namespace Production.EmmaCabCompany;

public interface IFileWriter
{
    public void Write(string filename, string[] fileContents);
}