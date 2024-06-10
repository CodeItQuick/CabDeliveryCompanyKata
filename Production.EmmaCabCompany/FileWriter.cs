namespace Production.EmmaCabCompany;

public class FileWriter : IFileWriter
{
    public void Write(string filename, string[] fileContents)
    {
        File.WriteAllLines(filename, fileContents);
    }
}