﻿using System;
using System.IO;
using Jh.Abp.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Jh.Abp.MenuManagement
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                       //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                       .Enrich.FromLogContext()
                       .WriteTo.Async(c =>
                       {
                           c.File(path: $"Logs/logs-.log",
                           outputTemplate: AbpConsts.SerilogOutputTemplate,
                           fileSizeLimitBytes: 1024000,
                           rollOnFileSizeLimit: true,
                           rollingInterval: RollingInterval.Day,
                           retainedFileCountLimit: 31);
                       })
#if DEBUG
                .WriteTo.Async(c => c.Console())
#endif
                .CreateLogger();

            try
            {
                Log.Information("Starting web host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var _path = Directory.GetCurrentDirectory();
                    var config = new ConfigurationBuilder()
                                    .SetBasePath(_path)
                                    .AddJsonFile("hostsettings.json", optional: true)
                                    .AddCommandLine(args)
                                    .Build();
                    webBuilder.UseConfiguration(config);
                    //webBuilder.UseUrls("https://*:6102");
                    webBuilder.UseStartup<Startup>();
                })
                .UseAutofac()
                .UseSerilog();
    }
}
