using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Data;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Framework;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Business.Abstract;
using DevPortal.JenkinsManager.Business.Services;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CachedJenkinsServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IJenkinsRepository> jenkinsRepository;

        StrictMock<IGeneralSettingsService> generalSettingsService;

        StrictMock<ICacheWrapper> cache;

        StrictMock<ISettings> settings;

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IJenkinsJobFactory> jenkinsJobFactory;

        CachedJenkinsService service;

        [SetUp]
        public void Initialize()
        {
            jenkinsRepository = new StrictMock<IJenkinsRepository>();
            generalSettingsService = new StrictMock<IGeneralSettingsService>();
            cache = new StrictMock<ICacheWrapper>();
            settings = new StrictMock<ISettings>();
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            jenkinsJobFactory = new StrictMock<IJenkinsJobFactory>();

            service = new CachedJenkinsService(
                generalSettingsService.Object,
                jenkinsRepository.Object,
                cache.Object,
                settings.Object,
                applicationReaderService.Object,
                jenkinsJobFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            jenkinsRepository.VerifyAll();
            generalSettingsService.VerifyAll();
            cache.VerifyAll();
            settings.VerifyAll();
            applicationReaderService.VerifyAll();
            jenkinsJobFactory.VerifyAll();
        }

        #endregion

        [Test]
        public void GetJobsAsync_NoCondition_ReturnJobs()
        {
            // Arrange
            var jenkinsJobsUrl = new Uri($"http://www.example.com/jenkins/api/json");
            var application = new Application();
            var jobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Yellow };
            var successJobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Blue };
            var disabledJobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Disabled };
            var jobs = new List<JenkinsJobItem> { jobItem, successJobItem, disabledJobItem };
            var createdJobItem = new JenkinsJobItem();
            var createdSuccessJobItem = new JenkinsJobItem();
            var createdDisabledJobItem = new JenkinsJobItem();
            var jobList = new List<JenkinsJobItem> { createdJobItem, createdSuccessJobItem, createdDisabledJobItem };

            generalSettingsService.Setup(x => x.GetJenkinsJobsUrl()).Returns(jenkinsJobsUrl);
            jenkinsRepository.Setup(x => x.GetJobs(jenkinsJobsUrl)).ReturnsAsync(jobs);
            applicationReaderService.Setup(x => x.GetApplicationByJenkinsJobName(jobs[0].Name)).Returns(application);
            jenkinsJobFactory.Setup(x => x.CreateJenkinsJobItem(jobs[0], application)).Returns(createdJobItem);
            jenkinsJobFactory.Setup(x => x.CreateJenkinsJobItem(jobs[1], application)).Returns(createdJobItem);
            jenkinsJobFactory.Setup(x => x.CreateJenkinsJobItem(jobs[2], application)).Returns(createdJobItem);

            // Act
            var result = service.GetJobsAsync();

            // Assert
            result.Result.Should().BeEquivalentTo(jobList);
        }

        [Test]
        public void GetFailedJobs_NoCondition_ReturnJobs()
        {
            // Arrange
            var key = CacheKeyNames.FailedJobsCacheKey;
            var cacheTime = 5;
            var failedJob = new JenkinsJobItem { Name = "", Url = new Uri("http://url.com"), Color = JenkinsStatusColorNames.Red };
            var jobs = new List<JenkinsJobItem> { failedJob };
            var failedJobCount = 5;

            settings.Setup(x => x.JenkinsFailedJobCount).Returns(failedJobCount);
            settings.Setup(x => x.FailedJobsCacheTimeInMinutes).Returns(cacheTime);

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration(
                 key, SetupAny<Func<IEnumerable<JenkinsJobItem>>>(), cacheTime)).Returns(jobs);

            // Act
            var result = service.GetFailedJobs();

            // Assert
            result.Should().BeEquivalentTo(jobs);
        }

        [Test]
        public void GetAllFailedJobs_NoCondition_ReturnJobs()
        {
            // Arrange
            var key = CacheKeyNames.FailedJobsCacheKey;
            var cacheTime = 5;
            var failedJob = new JenkinsJobItem { Name = "", Url = new Uri("http://url.com"), Color = JenkinsStatusColorNames.Red };
            var jobs = new List<JenkinsJobItem> { failedJob };

            settings.Setup(x => x.FailedJobsCacheTimeInMinutes).Returns(cacheTime);

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration(
                key, SetupAny<Func<IEnumerable<JenkinsJobItem>>>(), cacheTime)).Returns(jobs);

            // Act
            var result = service.GetAllFailedJobs();

            // Assert
            result.Should().BeEquivalentTo(jobs);
        }

        [Test]
        public void GetJobAsync_NoCondition_ReturnJobDetail()
        {
            // Arrange
            var name = "job-name";
            var url = new Uri($"http://www.example.com/jenkins/job/{name}/api/json");
            var application = new Application
            {
                Id = 1,
                Name = "Application Name"
            };
            var jobDetail = new JenkinsJobDetail
            {
                ApplicationId = application.Id,
                ApplicationName = application.Name
            };

            generalSettingsService.Setup(x => x.GetJenkinsJobUrl(name)).Returns(url);
            jenkinsRepository.Setup(x => x.GetJobDetail(url)).ReturnsAsync(jobDetail);
            applicationReaderService.Setup(x => x.GetApplicationByJenkinsJobName(name)).Returns(application);

            // Act
            var result = service.GetJobDetailAsync(name);

            // Assert
            result.Result.Should().BeSameAs(jobDetail);
        }
    }
}