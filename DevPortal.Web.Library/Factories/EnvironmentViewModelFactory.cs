using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;
using Environment = DevPortal.Model.Environment;

namespace DevPortal.Web.Library
{
    public class EnvironmentViewModelFactory : IEnvironmentViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        readonly IAuthorizationServiceWrapper authorizationWrapper;

        public EnvironmentViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory,
            IAuthorizationServiceWrapper authorizationWrapper)
        {
            this.breadCrumbFactory = breadCrumbFactory;
            this.authorizationWrapper = authorizationWrapper;
        }

        #endregion

        bool CheckUserHasAdminDeveloperPolicy()
        {
            return authorizationWrapper.CheckUserHasAdminDeveloperPolicy();
        }

        #region Environment

        public EnvironmentViewModel CreateEnvironmentsViewModel(ICollection<Environment> environments)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateEnvironmentsModel();

            return new EnvironmentViewModel
            {
                Environments = environments,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public EnvironmentViewModel CreateEnvironmentDetailViewModel(Environment environment)
        {
            var isAuthorized = CheckUserHasAdminDeveloperPolicy();
            var breadCrumbViewModel = breadCrumbFactory.CreateEnvironmentDetailModel(environment.Id);

            return new EnvironmentViewModel
            {
                Environment = environment,
                IsAuthorized = isAuthorized,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public EnvironmentViewModel CreateEnvironmentEditViewModel(Environment environment)
        {
            var breadCrumbViewModel = breadCrumbFactory.CreateEnvironmentEditModel(environment.Id);

            return new EnvironmentViewModel
            {
                Environment = environment,
                BreadCrumbViewModel = breadCrumbViewModel
            };
        }

        public EnvironmentViewModel CreateEnvironmentAddView()
        {
            return new EnvironmentViewModel
            {
                BreadCrumbViewModel = breadCrumbFactory.CreateEnvironmentAddModel()
            };
        }

        #endregion
    }
}