using Autofac;
using DevPortal.Data.Abstract;
using DevPortal.Data.Wrappers;

namespace DevPortal.Ioc.Binders
{
    public static class DataDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            BindRepository(builder);
            BindFactory(builder);
            builder.RegisterType<NugetServerApiClientWrapper>().As<INugetServerApiClientWrapper>().SingleInstance();
        }

        static void BindRepository(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Data.Repositories.ApplicationRepository).Assembly)
                           .Where(t => t.Name.EndsWith(ClassSuffixNames.Repository))
                           .AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        static void BindFactory(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Data.Factories.ApplicationDataRequestFactory).Assembly)
                           .Where(t => t.Name.EndsWith(ClassSuffixNames.Factory))
                           .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}