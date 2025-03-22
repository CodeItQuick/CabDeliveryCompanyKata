using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.CustomerList;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Fleet;
using Production.EmmaCabCompany.Adapter.OutAdapter.CabFileAdapter.Menu;
using Production.EmmaCabCompany.Application;
using Production.EmmaCabCompany.Application.CustomerList;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Command;
using Production.EmmaCabCompany.Application.CustomerList.Commands.Handler;
using Production.EmmaCabCompany.Application.Fleet;
using Production.EmmaCabCompany.Application.Menu;
using Production.WebCabCompany.Controllers;
using Production.WebCabCompany.Models;
using Production.WebCabCompany.Services;

namespace Production.WebCabCompany;

public class Startup
{
    public Startup(IWebHostEnvironment env)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.Development.json", optional: true)
            .AddJsonFile($"appsettings.Local.json", optional: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .AddEnvironmentVariables() // used for prod
            .Build();
        
        Configuration = configuration;
    }
    
    public IConfigurationRoot Configuration { get; set; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        
        // Add framework services.
        services.AddDbContext<ApplicationDbContext>(options =>
            options.ConfigureWarnings(b => b.Log(CoreEventId.ManyServiceProvidersCreatedWarning))
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        services.AddDbContext<CabContext>(options =>
        {
            var connectionFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "production_db.db");
            if (!File.Exists(connectionFile))
            {
                File.Create(connectionFile);
            }
            options.UseSqlite($"Data Source={connectionFile}");
        }, ServiceLifetime.Singleton);
        
        // // Does not work on this dotnet?
        // services.AddDatabaseDeveloperPageExceptionFilter();


        services.AddMvc();

        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies(o => { });

        // Add application services.
        // services.AddTransient<IEmailSender, AuthMessageSender>();
        // services.AddTransient<ISmsSender, AuthMessageSender>();
        // services.AddScoped<IFleetRepository, FleetRepository>();
        // services.AddScoped<ICustomerListRepository, CustomerListRepository>();
        // services.AddScoped<IMenuRepository, MenuRepository>();
        services.AddScoped<IApplicationHandler, ApplicationHandler>();
        // services.AddScoped<IMenuRequestedHandler, MenuRequestedHandler>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // Does not work on this dotnet?
            // app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        CreateRoles(serviceProvider).GetAwaiter().GetResult();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });
    }
    private async Task CreateRoles(IServiceProvider serviceProvider)
    {
        //initializing custom roles 
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        string[] roleNames = { "Admin" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            // ensure that the role does not exist
            if (!roleExist)
            {
                //create the roles and seed them to the database: 
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // find the user with the admin email 
        var user = await userManager.FindByEmailAsync("admin@email.com");

        // check if the user exists
        if(user == null)
        {
            //Here you could create the super admin who will maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = "Admin",
                Email = "admin@email.com",
            };
            string adminPassword = "p@$$w0rd";

            var createPowerUser = await userManager.CreateAsync(poweruser, adminPassword);
            if (createPowerUser.Succeeded)
            {
                //here we tie the new user to the role
                await userManager.AddToRoleAsync(poweruser, "Admin");
            }
        }
    }
}