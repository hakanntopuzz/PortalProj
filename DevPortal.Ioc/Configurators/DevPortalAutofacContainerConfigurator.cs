using AB.Data.DapperClient.Ioc.Autofac.Modules;
using AB.Framework.Logger.Nlog.Ioc.Autofac.Modules;
using AB.Framework.Security.Ioc.Autofac;
using AB.Framework.SettingsReader.Ioc.Autofac;
using AB.Framework.TextTemplates.Ioc.Autofac;
using Autofac;
using DevPortal.Ioc.Modules;
using Microsoft.Extensions.Hosting;

namespace DevPortal.Ioc
{
    public static class DevPortalAutofacContainerConfigurator
    {
        public static void ConfigureContainer(HostBuilderContext context, ContainerBuilder builder)
        {
            builder.RegisterModule(new LoggerAutoFacModule());
            builder.RegisterModule(new SecurityAutofacModule());
            builder.RegisterModule(new SettingsReaderAutoFacModule());
            builder.RegisterModule(new TextTemplatesAutofacModule());
            builder.RegisterModule(new WebAutoFacModule());
            builder.RegisterModule(new DapperClientAutofacModule());
        }
    }
}