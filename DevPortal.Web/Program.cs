using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevPortal.Ioc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;

namespace DevPortal.Web
{
    public static class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var logger = NLogBuilder.ConfigureNLog($"Config/nlog.{env}.config").GetCurrentClassLogger();

            try
            {
                var message = $" MethodName: {nameof(Main)}. Uygulama başlıyor. Konfigürasyonlar yapılıyor...";
                logger.Info(message);

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                var message = $" MethodName: {nameof(Main)}. Everything is something happened";
                logger.Error(message, ex);
            }
            finally
            {
                logger.Info(nameof(Main), "Uygulama başladı.");

                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
               .UseServiceProviderFactory(new AutofacServiceProviderFactory())
               .ConfigureContainer<ContainerBuilder>(DevPortalAutofacContainerConfigurator.ConfigureContainer)
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
               })
               .UseNLog()
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.UseStartup<Startup>();
               });
    }
}