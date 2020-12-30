using AB.Framework.UnitTests;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationReportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationRepository> applicationRepository;

        ApplicationReportService service;

        [SetUp]
        public void Initialize()
        {
            applicationRepository = new StrictMock<IApplicationRepository>();

            service = new ApplicationReportService(applicationRepository.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationRepository.VerifyAll();
        }

        #endregion

        #region get application stats

        [Test]
        public void GetApplicationStats_NoCondition_ReturnApplication()
        {
            // Arrange
            int applicationCount = 75;
            int applicationGroupCount = 8;
            int jenkinsJobCount = 120;
            int svnRepositoryCount = 86;
            int sonarQubeProjectCount = 86;
            int applicationRedmineProjectCount = 45;
            int databaseRedmineProjectCount = 18;
            int nugetPackageCount = 51;
            var applicationCountsByType = new List<ApplicationCountByTypeModel> {
                new ApplicationCountByTypeModel { ApplicationCount = 12, ApplicationTypeName = "asp-net-mvc" },
                new ApplicationCountByTypeModel { ApplicationCount = 6, ApplicationTypeName = "wcf" }
            };
            var jenkinsJobCountsByType = new List<JenkinsJobCountByTypeModel> {
                new JenkinsJobCountByTypeModel { JobCount = 12, JobTypeName= "Sürekli Entegrasyon" },
                new JenkinsJobCountByTypeModel { JobCount = 6, JobTypeName = "Günlük Analiz" }
            };
            var sonarQubeProjectCountsByType = new List<SonarQubeProjectCountByTypeModel> {
                new SonarQubeProjectCountByTypeModel { ProjectCount = 24, ProjectTypeName = "Haftalık Analiz" },
                new SonarQubeProjectCountByTypeModel { ProjectCount = 16, ProjectTypeName = "Günlük Analiz" }
            };

            var expectedApplicationStats = new ApplicationStats
            {
                ApplicationCount = applicationCount,
                ApplicationGroupCount = applicationGroupCount,
                JenkinsJobCount = jenkinsJobCount,
                SvnRepositoryCount = svnRepositoryCount,
                SonarQubeProjectCount = sonarQubeProjectCount,
                ApplicationRedmineProjectCount = applicationRedmineProjectCount,
                DatabaseRedmineProjectCount = databaseRedmineProjectCount,
                ApplicationCountsByType = applicationCountsByType,
                JenkinsJobCountsByType = jenkinsJobCountsByType,
                SonarQubeProjectCountsByType = sonarQubeProjectCountsByType,
                NugetPackageCount = nugetPackageCount
            };

            // Act
            applicationRepository.Setup(x => x.GetApplicationCount()).Returns(applicationCount);
            applicationRepository.Setup(x => x.GetApplicationGroupCount()).Returns(applicationGroupCount);
            applicationRepository.Setup(x => x.GetJenkinsJobCount()).Returns(jenkinsJobCount);
            applicationRepository.Setup(x => x.GetSvnRepositoryCount()).Returns(svnRepositoryCount);
            applicationRepository.Setup(x => x.GetSonarQubeProjectCount()).Returns(sonarQubeProjectCount);
            applicationRepository.Setup(x => x.GetApplicationRedmineProjectCount()).Returns(applicationRedmineProjectCount);
            applicationRepository.Setup(x => x.GetDatabaseRedmineProjectCount()).Returns(databaseRedmineProjectCount);
            applicationRepository.Setup(x => x.GetNugetPackageCount()).Returns(nugetPackageCount);
            applicationRepository.Setup(x => x.GetApplicationCountByType()).Returns(applicationCountsByType);
            applicationRepository.Setup(x => x.GetJenkinsJobCountByType()).Returns(jenkinsJobCountsByType);
            applicationRepository.Setup(x => x.GetSonarQubeProjectCountByType()).Returns(sonarQubeProjectCountsByType);

            var result = service.GetApplicationStats();

            // Assert
            result.Should().BeEquivalentTo(expectedApplicationStats);
        }

        #endregion
    }
}