using Autofac;
using DevPortal.Business.Services;
using System;

namespace DevPortal.Ioc.Binders
{
    public static class BusinessDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Business.Services.ApplicationWriterService).Assembly)
                           .Where(t => IsAllowedForRegister(t))
                           .AsImplementedInterfaces()
                           .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Business.Factories.FileSystemFactory).Assembly)
                           .Where(t => t.Name.EndsWith(ClassSuffixNames.Factory))
                           .AsImplementedInterfaces()
                           .InstancePerLifetimeScope();
        }

        static bool IsAllowedForRegister(Type t)
        {
            //CachedApplicationReportService'in devreye girebilmesi için
            if (t == typeof(ApplicationReportService))
            {
                return false;
            }

            if (t.Name.EndsWith(ClassSuffixNames.Service))
            {
                return true;
            }

            return false;
        }
    }
}