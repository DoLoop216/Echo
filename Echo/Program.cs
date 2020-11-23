using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Echo
{
    public static class Program
    {
        public static Random rnd = new Random();
        public static bool SendingNewsletter = false;
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static void AddVisitor(string WebRoot)
        {
            int n = 1;
            if (File.Exists(WebRoot + "/Visits"))
            {
                string _visits = System.IO.File.ReadAllText(WebRoot + "/Visits");


                if (!string.IsNullOrWhiteSpace(_visits))
                    n += Convert.ToInt32(_visits);
            }

            System.IO.File.WriteAllText(WebRoot + "/Visits", n.ToString());

        }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
