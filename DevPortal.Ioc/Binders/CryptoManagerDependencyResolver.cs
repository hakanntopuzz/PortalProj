using Autofac;

namespace DevPortal.Ioc.Binders
{
    public static class CryptoManagerDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            BindWrappers(builder);
            BindServices(builder);
        }

        static void BindWrappers(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Cryptography.Business.Wrappers.AesCryptographyWrapper).Assembly)
                           .Where(t => t.Name.EndsWith(ClassSuffixNames.Wrapper))
                           .AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        static void BindServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Cryptography.Business.Services.CryptographyService).Assembly)
                           .Where(t => t.Name.EndsWith(ClassSuffixNames.Service))
                           .AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}