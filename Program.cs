using Microsoft.EntityFrameworkCore;
using PNJGoldValue;
using PNJGoldValue.Services;

public class Program {
    public static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();
        createDatabaseIfNotExist(host);
        host.Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
        {
            IConfiguration configuration = hostContext.Configuration;
            AppSettings.configuration = configuration;
            AppSettings.ConnectionString = configuration.GetConnectionString("SqlServer");
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(AppSettings.ConnectionString);
            services.AddScoped<AppDbContext>(d => new AppDbContext(optionBuilder.Options));
            services.AddHostedService<Worker>();
        });
    public static void createDatabaseIfNotExist(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<AppDbContext>();
            context.Database.EnsureCreated();
        }
    }
}
