using DevPortal.Cryptography.Model;
using DevPortal.Framework.Extensions;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class SecurityViewModelFactory : ISecurityViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public SecurityViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        #region security

        public CryptoViewModel CreateCryptoViewModel()
        {
            return new CryptoViewModel();
        }

        public GeneratePasswordViewModel CreateGeneratePasswordViewModel(string password = null)
        {
            return new GeneratePasswordViewModel
            {
                Password = password,
                BreadCrumbViewModel = breadCrumbFactory.CreatePasswordModel()
            };
        }

        public AesViewModel CreateAesViewModel(string convertedText = null)
        {
            var applications = CreateCryptoApplicationList();

            return new AesViewModel
            {
                CryptoApplications = applications,
                ConvertedText = convertedText
            };
        }

        static List<CryptographyClientApplication> CreateCryptoApplicationList()
        {
            return new List<CryptographyClientApplication>
            {
                new CryptographyClientApplication{
                    SiteId = CryptoApplications.MxMembership.ToInt32(),
                    Name = CryptoApplications.MxMembership.ToString()
                },
                new CryptographyClientApplication{
                    SiteId = CryptoApplications.MxSocial.ToInt32(),
                    Name = CryptoApplications.MxSocial.ToString(),
                }
            };
        }

        public HashViewModel CreateHashViewModel(string hashedText = null)
        {
            return new HashViewModel
            {
                HashedText = hashedText,
                BreadCrumbViewModel = breadCrumbFactory.CreateHashModel()
            };
        }

        public GuidViewModel CreateGuidViewModel(string guid = null)
        {
            return new GuidViewModel
            {
                Guid = guid,
                BreadCrumbViewModel = breadCrumbFactory.CreateGuidModel()
            };
        }

        #endregion
    }
}