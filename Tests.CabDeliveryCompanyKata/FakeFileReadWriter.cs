using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class FakeFileReadWriter : IFileWriter, IFileReader
{
    private string[] fileContents = Array.Empty<string>();

    public void Write(string[] fileContents)
    {
        this.fileContents = fileContents;
    }

    public int CountLines()
    {
        return fileContents.Length;
    }

    public string ReadLine(int i)
    {
        return fileContents[i];
    }

    public string[] Read()
    {
        return fileContents;
    }
    
}