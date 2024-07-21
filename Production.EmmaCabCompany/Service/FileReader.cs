namespace Production.EmmaCabCompany;

public class FileReader(string filename) : IFileReader
{
    public string[] Read()
    {
        return File.ReadAllLines(filename);
    }
}