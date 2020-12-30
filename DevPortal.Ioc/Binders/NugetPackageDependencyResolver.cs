using Autofac;
using DevPortal.NugetManager.Business;
using System;

namespace DevPortal.Ioc.Binders
{
    public static class NugetPackageDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(NugetService).Assembly)
                .Where(t => IsAllowedForRegister(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        static bool IsAllowedForRegister(Type t)
        {
            //CachedNugetService'in devreye girebilmesi için
            if (t == typeof(NugetService))
            {
                return false;
            }

            return true;
        }
    }
}