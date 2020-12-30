using Autofac;
using DevPortal.Ioc.Extensions;
using DevPortal.Validation;
using DevPortal.Validation.Abstract;
using DevPortal.Validation.FluentValidators;
using FluentValidation;

namespace DevPortal.Ioc.Binders
{
    public static class ValidationDependencyResolver
    {
        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(GeneralSettingsValidator).Assembly)
               .As(t => t.GetInterfaces())
               .Where(t => t.Name.EndsWith(ClassSuffixNames.Validator))
               .InstancePerLifetimeScope();

            builder.RegisterType<AutofacValidatorFactory>().As<IValidatorFactory>().SingleInstance();
            builder.RegisterType<RequestValidator>().As<IRequestValidator>().SingleInstance();
        }
    }
}