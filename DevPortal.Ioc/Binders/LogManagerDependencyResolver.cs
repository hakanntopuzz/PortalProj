using Autofac;
using DevPortal.Log.Business;
using DevPortal.Log.Business.Abstract;

namespace DevPortal.Ioc.Binders
{
    public static class LogManagerDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<LogFactory>().As<ILogFactory>();
            builder.RegisterType<LogService>().As<ILogService>();
        }
    }
}