namespace Production.EmmaCabCompany;

public class FileReader(string customerListFilename, string cabListFilename) : IFileReader
{
    public string[] Read(string filename)
    {
        if (filename == customerListFilename)
        {
            return File.ReadAllLines(filename);
        }
        if (filename == cabListFilename)
        {
            return File.ReadAllLines(filename);
        }

        throw new IOException($"{filename} does not match {customerListFilename} or {cabListFilename}");
    }
}