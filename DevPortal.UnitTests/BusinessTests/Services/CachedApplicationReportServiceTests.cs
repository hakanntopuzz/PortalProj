using AB.Framework.UnitTests;
using DevPortal.Business.Services;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CachedApplicationReportServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationRepository> applicationRepository;

        StrictMock<ICacheWrapper> cache;

        StrictMock<ISettings> settings;

        ApplicationReportService service;

        [SetUp]
        public void Initialize()
        {
            applicationRepository = new StrictMock<IApplicationRepository>();
            cache = new StrictMock<ICacheWrapper>();
            settings = new StrictMock<ISettings>();

            service = new CachedApplicationReportService(
                applicationRepository.Object,
                cache.Object,
                settings.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationRepository.VerifyAll();
            cache.VerifyAll();
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
            var key = CacheKeyNames.JenkinsJobCountCacheKey;
            var cacheTime = 2;

            var expectedApplicationStats = new ApplicationStats
            {
                ApplicationCount = applicationCount,
                ApplicationGroupCount = applicationGroupCount,
                JenkinsJobCount = jenkinsJobCount,
                SvnRepositoryCount = svnRepositoryCount,
                SonarQubeProjectCount = sonarQubeProjectCount,
                ApplicationRedmineProjectCount = applicationRedmineProjectCount,
                DatabaseRedmineProjectCount = databaseRedmineProjectCount,
                NugetPackageCount = nugetPackageCount,
                ApplicationCountsByType = applicationCountsByType,
                JenkinsJobCountsByType = jenkinsJobCountsByType,
                SonarQubeProjectCountsByType = sonarQubeProjectCountsByType
            };

            // Act
            applicationRepository.Setup(x => x.GetApplicationCount()).Returns(applicationCount);
            applicationRepository.Setup(x => x.GetApplicationGroupCount()).Returns(applicationGroupCount);
            settings.Setup(x => x.JenkinsJobCountCacheTimeInMinutes).Returns(cacheTime);
            cache.Setup(x => x.GetOrCreateWithSlidingExpiration(
                key,
                SetupAny<Func<int>>(),
                cacheTime)).Returns(jenkinsJobCount);

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