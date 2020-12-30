using Autofac;
using DevPortal.Ioc.Binders;

namespace DevPortal.Ioc.Modules
{
    public class WebAutoFacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            WebDependencyResolver.RegisterServices(builder);
            DataDependencyResolver.RegisterServices(builder);
            FrameworkDependencyResolver.RegisterServices(builder);
            BusinessDependencyResolver.RegisterServices(builder);
            CryptoManagerDependencyResolver.RegisterServices(builder);
            NugetPackageDependencyResolver.RegisterServices(builder);
            JenkinsManagerDependencyResolver.RegisterServices(builder);
            LogManagerDependencyResolver.RegisterServices(builder);
            SvnAdminDependencyResolver.RegisterServices(builder);
            ValidationDependencyResolver.RegisterServices(builder);
            IdentityDependencyResolver.RegisterServices(builder);
            DataDependencyResolver.RegisterServices(builder);
        }
    }
}