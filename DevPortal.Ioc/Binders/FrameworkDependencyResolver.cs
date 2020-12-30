using Autofac;
using DevPortal.Framework.Abstract;
using DevPortal.Framework.Factories;
using DevPortal.Framework.Services;
using DevPortal.Framework.Wrappers;

namespace DevPortal.Ioc.Binders
{
    public static class FrameworkDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(RouteValueFactory).Assembly)
                         .Where(t => t.Name.EndsWith(ClassSuffixNames.Factory))
                         .AsImplementedInterfaces()
                         .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(MemoryCacheWrapper).Assembly)
                        .Where(t => t.Name.EndsWith(ClassSuffixNames.Wrapper))
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(UserAgentService).Assembly)
                       .Where(t => t.Name.EndsWith(ClassSuffixNames.Service))
                       .AsImplementedInterfaces()
                       .InstancePerLifetimeScope();

            builder.RegisterType<Settings>().As<ISettings>();
            builder.RegisterType<CsvSerializer>().As<ICsvSerializer>();
        }
    }
}