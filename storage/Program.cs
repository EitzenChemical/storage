using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using storage.Services;
using System;

namespace storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Choose mode: 1 - InMemory, 2 - Persistent");
            while (true) {
                var str = Console.ReadLine();
                if (str == "1")
                {
                    mode = Mode.InMemory;
                    break;
                }
                else if (str == "2")
                {
                    mode = Mode.Persistent;
                    break;
                }
                else Console.WriteLine("Choose mode: 1 - InMemory, 2 - Persistent");
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static Mode mode;
    }
}
