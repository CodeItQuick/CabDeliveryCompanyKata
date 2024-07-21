using Production.EmmaCabCompany;

namespace Tests.CabDeliveryCompanyKata;

public class FakeFileReadWriter(string customerListFilename, string cabListFilename) : IFileWriter, IFileReader
{
    private string[] customerListFileContents = Array.Empty<string>();
    private string[] cabListFileContents = Array.Empty<string>();

    public void Write(string filename, string[] fileContents)
    {
        if (filename == customerListFilename)
        {
            customerListFileContents = fileContents;
            return;
        }
        if (filename == cabListFilename)
        {
            cabListFileContents = fileContents;
            return;
        }

        throw new IOException($"{filename} does not match {customerListFilename} or {cabListFilename}");
    }

    public int CountLines()
    {
        return customerListFileContents.Length;
    }

    public string ReadLine(int i)
    {
        return customerListFileContents[i];
    }

    public string[] Read(string filename)
    {
        
        if (filename == customerListFilename)
        {
            return customerListFileContents;
        }
        if (filename == cabListFilename)
        {
            return cabListFileContents;
        }

        throw new IOException($"{filename} does not match {customerListFilename} or {cabListFilename}");
        
    }
    
}