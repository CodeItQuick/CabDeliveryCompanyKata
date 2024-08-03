namespace Production.EmmaCabCompany.Adapter.@in;

public class FileHandler(string customerListFilename, string cabListFilename) : IFileHandler
{
    public void WriteCabList(string[] cabList)
    {
        Write(cabListFilename, cabList);
    }

    public void WriteCustomerList(string[] exportedCustomers)
    {
        Write(customerListFilename, exportedCustomers);
    }

    public string[] ReadCustomerList()
    {
        return Read(customerListFilename);
    }

    public string[] ReadReadCabList()
    {
        return Read(cabListFilename);
    }

    private void Write(string filename, string[] fileContents)
    {
        if (customerListFilename == filename)
        {
            
            if (File.Exists(filename))
            {
                File.WriteAllLines(filename, fileContents);
                return;
            }
            var fileStream = File.Create(filename);
            fileStream.Close();
            File.WriteAllLines(filename, fileContents);
            return;
        } 
        if (cabListFilename == filename)
        {
            if (File.Exists(filename))
            {
                File.WriteAllLines(filename, fileContents);
                return;
            }
            var fileStream = File.Create(filename);
            fileStream.Close();
            File.WriteAllLines(filename, fileContents);
            return;
        }
        throw new IOException($"{filename} does not match {customerListFilename} or {cabListFilename}");
    }

    private string[] Read(string filename)
    {
        if (filename == customerListFilename || filename == cabListFilename)
        {
            if (File.Exists(filename)) return File.ReadAllLines(filename);
            var fileStream = File.Create(filename);
            fileStream.Close();

            return File.ReadAllLines(filename);
        }

        throw new IOException($"{filename} does not match {customerListFilename} or {cabListFilename}");
    }
}