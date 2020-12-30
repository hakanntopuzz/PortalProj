using Autofac;
using DevPortal.SvnAdmin.Business;
using DevPortal.SvnAdmin.Data;
using System;

namespace DevPortal.Ioc.Binders
{
    public static class SvnAdminDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            // Data
            builder.RegisterAssemblyTypes(typeof(SvnAdminRepository).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //Business
            builder.RegisterAssemblyTypes(typeof(SvnAdminService).Assembly)
                .Where(t => IsAllowedForRegister(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }

        static bool IsAllowedForRegister(Type t)
        {
            //SvnAdminService'in devreye girebilmesi için
            if (t == typeof(SvnAdminService))
            {
                return false;
            }

            return true;
        }
    }
}