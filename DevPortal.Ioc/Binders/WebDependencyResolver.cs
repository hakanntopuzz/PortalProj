using Autofac;

namespace DevPortal.Ioc.Binders
{
    public static class WebDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Web.Library.Factories.BreadCrumbFactory).Assembly)
               .Where(t => t.Name.EndsWith(ClassSuffixNames.Factory))
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
        }
    }
}