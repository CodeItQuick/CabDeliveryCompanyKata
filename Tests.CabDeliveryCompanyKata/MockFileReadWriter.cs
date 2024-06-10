using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class MockFileReadWriter : IFileWriter, IFileReader
{
    private string[] fileContents = Array.Empty<string>();

    public void Write(string filename, string[] fileContents)
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

    public string[] Read(string filename)
    {
        return fileContents;
    }
    
}