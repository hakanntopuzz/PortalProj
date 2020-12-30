using AB.Framework.UnitTests;
using DevPortal.Model;
using DevPortal.Web.Library;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GeneralSettingsViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        GeneralSettingsViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();

            factory = new GeneralSettingsViewModelFactory(breadCrumbFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
        }

        #endregion

        #region general settings

        [Test]
        public void CreateGeneralSettingsViewModel_NoCondition_ReturnGeneralSettingsViewModel()
        {
            // Arrange
            var generalSettings = new GeneralSettings();
            BreadCrumbViewModel model = null;

            breadCrumbFactory.Setup(x => x.CreateGeneralSettingsModel()).Returns(model);

            // Act
            var result = factory.CreateGeneralSettingsViewModel(generalSettings);

            // Assert
            var expectedResult = new GeneralSettingsViewModel
            {
                GeneralSettings = generalSettings,
                BreadCrumbViewModel = model
            };
            result.Should().BeEquivalentTo(result);
        }

        #endregion
    }
}