namespace Production.EmmaCabCompany;

public class FileReader : IFileReader
{
    public string[] Read(string filename)
    {
        return File.ReadAllLines(filename);
    }
}