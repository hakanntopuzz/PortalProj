using AB.Framework.UnitTests;
using DevPortal.Model;
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
    public class LogViewModelFactoryTests : BaseTestFixture
    {
        #region members & setup

        LogViewModelFactory factory;

        StrictMock<IBreadCrumbFactory> breadCrumbFactory;

        [SetUp]
        public void Initialize()
        {
            breadCrumbFactory = new StrictMock<IBreadCrumbFactory>();

            factory = new LogViewModelFactory(breadCrumbFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            breadCrumbFactory.VerifyAll();
        }

        #endregion

        #region log manager

        [Test]
        public void CreateLogViewModel_NoCondition_ReturnLogViewModel()
        {
            // Arrange
            var logViewModel = new LogViewModel();

            // Act
            var result = factory.CreateLogViewModel();

            // Assert
            result.Should().NotBeNull();
            result.ApplicationGroups.Should().BeSameAs(logViewModel.ApplicationGroups);
        }

        [Test]
        public void CreateLogViewModel_WithApplicationGroups_ReturnLogViewModel()
        {
            // Arrange
            ICollection<ApplicationGroup> applicationGroups = null;
            const string physicalPath = "";
            BreadCrumbViewModel model = null;

            breadCrumbFactory.Setup(x => x.CreateLogListModel()).Returns(model);

            // Act
            var result = factory.CreateLogViewModel(applicationGroups, physicalPath);

            // Assert
            var expectedResult = new LogViewModel
            {
                ApplicationGroups = applicationGroups,
                LogFilePath = physicalPath,
                BreadCrumbViewModel = model
            };
            result.Should().BeEquivalentTo(result);
        }

        [Test]
        public void CreateLogFileViewModel_NoCondition_ReturnLogFileViewModel()
        {
            // Arrange
            var logFile = new LogFileModel { Path = "" };
            BreadCrumbViewModel model = null;

            breadCrumbFactory.Setup(x => x.CreateLogDetailModel(logFile.FilePath)).Returns(model);

            // Act
            var result = factory.CreateLogFileViewModel(logFile);

            // Assert
            var expectedResult = new LogFileViewModel
            {
                LogFile = logFile,
                BreadCrumbViewModel = model
            };
            result.Should().BeEquivalentTo(result);
        }

        #endregion
    }
}