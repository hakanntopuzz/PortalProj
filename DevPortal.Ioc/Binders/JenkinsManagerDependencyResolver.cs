using Autofac;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Business.Factories;
using DevPortal.JenkinsManager.Business.Services;
using System;

namespace DevPortal.Ioc.Binders
{
    public static class JenkinsManagerDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(JenkinsManager.Business.Services.JenkinsService).Assembly)
                .Where(t => IsAllowedForRegister(t))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(JenkinsManager.Business.Factories.JenkinsJobFactory).Assembly)
                .Where(t => t.Name.EndsWith(ClassSuffixNames.Factory))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(JenkinsManager.Business.Services.ApplicationBuildScriptServiceProvider).Assembly)
                .InNamespaceOf<ApplicationBuildScriptServiceProvider>()
                .Where(t => t.Name.EndsWith(ClassSuffixNames.ServiceProvider))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<BuildScriptServiceFactory>().As<IBuildScriptServiceFactory>()
                .PropertiesAutowired()
                .InstancePerLifetimeScope();
        }

        static bool IsAllowedForRegister(Type t)
        {
            //CachedJenkinsService'in devreye girebilmesi için
            if (t == typeof(JenkinsService))
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