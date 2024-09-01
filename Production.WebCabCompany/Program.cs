namespace Production.WebCabCompany;

public class Program
{
    public static void Main(string[] args)
    {

        var host = CreateWebHostBuilder(args).Build();
        host.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>();
}