using Autofac;
using DevPortal.Identity.Business;
using DevPortal.Identity.Business.Abstract;
using DevPortal.Identity.Data;
using DevPortal.Model;
using Microsoft.AspNetCore.Identity;

namespace DevPortal.Ioc.Binders
{
    public static class IdentityDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserStore>().As<IUserStore<User>>();
            builder.RegisterType<IdentityUserService>().As<IIdentityUserService>();
            builder.RegisterType<IdentityFactory>().As<IIdentityFactory>();
        }
    }
}