namespace Production.EmmaCabCompany;

public class FileWriter(string filename) : IFileWriter
{
    public void Write(string[] fileContents)
    {
        File.WriteAllLines(filename, fileContents);
    }
}