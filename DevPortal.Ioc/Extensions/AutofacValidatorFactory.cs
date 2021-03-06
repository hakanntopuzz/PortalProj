﻿using Autofac;
using FluentValidation;
using System;

namespace DevPortal.Ioc.Extensions
{
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        #region ctor

        readonly IComponentContext context;

        public AutofacValidatorFactory(IComponentContext context)
        {
            this.context = context;
        }

        #endregion

        public override IValidator CreateInstance(Type validatorType)
        {
            if (context.TryResolve(validatorType, out object instance))
            {
                return instance as IValidator;
            }

            return null;
        }
    }
}