using AB.Framework.UnitTests;
using DevPortal.Cryptography.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SecurityViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        SecurityViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();

            factory = new SecurityViewModelFactory(breadCrumbFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
        }

        #endregion

        #region security

        [Test]
        public void CreateCryptoViewModel_NoCondition_ReturnCryptoViewModel()
        {
            // Arrange
            var cryptoViewModel = new CryptoViewModel();

            // Act
            var result = factory.CreateCryptoViewModel();

            // Assert
            result.Should().NotBeNull();
            result.ConvertedText.Should().BeSameAs(cryptoViewModel.ConvertedText);
        }

        [Test]
        public void CreateAesViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange
            var applications = CreateCryptoApplicationList();
            var viewModel = new AesViewModel
            {
                CryptoApplications = applications,
            };

            // Act
            var result = factory.CreateAesViewModel();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        static List<CryptographyClientApplication> CreateCryptoApplicationList()
        {
            return new List<CryptographyClientApplication>
            {
                new CryptographyClientApplication{
                    SiteId = (int)CryptoApplications.MxMembership,
                    Name = CryptoApplications.MxMembership.ToString()
                },
                new CryptographyClientApplication{
                    SiteId =(int)CryptoApplications.MxSocial,
                    Name = CryptoApplications.MxSocial.ToString(),
                }
            };
        }

        [Test]
        public void CreateGeneratePasswordViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange
            var password = "123456";
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new GeneratePasswordViewModel
            {
                Password = password,
                BreadCrumbViewModel = breadcrumbViewModel
            };
            breadCrumbFactory.Setup(x => x.CreatePasswordModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateGeneratePasswordViewModel(password);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateGuidViewModel_NoCondition_ReturnViewModel()
        {
            // Arrange
            string guid = null;
            var breadcrumbViewModel = new BreadCrumbViewModel();
            var viewModel = new GuidViewModel
            {
                Guid = guid,
                BreadCrumbViewModel = breadcrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateGuidModel()).Returns(breadcrumbViewModel);

            // Act
            var result = factory.CreateGuidViewModel();

            // Assert
            result.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void CreateHashViewModel_NoCondition_ReturnCreateHashViewModel()
        {
            // Arrange
            string hashedText = null;
            var breadCrumbViewModel = new BreadCrumbViewModel();
            var hashViewModel = new HashViewModel
            {
                HashedText = hashedText,
                BreadCrumbViewModel = breadCrumbViewModel
            };

            breadCrumbFactory.Setup(x => x.CreateHashModel()).Returns(breadCrumbViewModel);

            // Act
            var result = factory.CreateHashViewModel();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(hashViewModel);
            result.HashedText.Should().BeSameAs(hashViewModel.HashedText);
            result.BreadCrumbViewModel.Should().BeSameAs(hashViewModel.BreadCrumbViewModel);
        }

        #endregion
    }
}