using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Client_Task_Executor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var client = new MainContext())
            {
                client.Database.EnsureCreated();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}