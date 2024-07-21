namespace Production.EmmaCabCompany;

public class FileWriter(string customerListFilename, string cabListFilename) : IFileWriter
{
    public void Write(string filename, string[] fileContents)
    {
        if (customerListFilename == filename)
        {
            File.WriteAllLines(filename, fileContents);
            return;
        } 
        if (cabListFilename == filename)
        {
            File.WriteAllLines(filename, fileContents);
            return;
        }
        throw new IOException($"{filename} does not match {customerListFilename} or {cabListFilename}");
    }
}