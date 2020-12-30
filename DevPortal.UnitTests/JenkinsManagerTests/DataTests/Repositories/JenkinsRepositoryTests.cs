using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data;
using DevPortal.Data.Abstract;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class JenkinsRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IJsonConventer> jsonConventer;

        StrictMock<IHttpClient> httpClient;

        StrictMock<ILoggingService> loggingService;

        StrictMock<IXmlFeedReader> xmlFeedReader;

        StrictMock<IJenkinsFactory> jenkinsFactory;

        JenkinsRepository repository;

        [SetUp]
        public void Initialize()
        {
            jsonConventer = new StrictMock<IJsonConventer>();
            httpClient = new StrictMock<IHttpClient>();
            loggingService = new StrictMock<ILoggingService>();
            xmlFeedReader = new StrictMock<IXmlFeedReader>();
            jenkinsFactory = new StrictMock<IJenkinsFactory>();

            repository = new JenkinsRepository(
                httpClient.Object,
                jsonConventer.Object,
                loggingService.Object,
                xmlFeedReader.Object,
                jenkinsFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            jsonConventer.VerifyAll();
            httpClient.VerifyAll();
            loggingService.VerifyAll();
            xmlFeedReader.VerifyAll();
            jenkinsFactory.VerifyAll();
        }

        #endregion

        #region get jobs

        [Test]
        public void GetJobs_ServiceFailed_LogErrorAndReturnNull()
        {
            // Arrange
            var url = new Uri("http://url.com");
            var exception = new Exception();

            httpClient.Setup(x => x.GetJsonStringAsync(url)).Throws(exception);
            loggingService.Setup(x => x.LogError(VerifyAny<string>(), VerifyAny<string>(), exception));

            // Act
            var result = repository.GetJobs(url);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeEmpty();
        }

        [Test]
        public void GetJobs_AllSuccess_ReturnJobs()
        {
            // Arrange
            var jobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Yellow };
            var successJobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Blue };
            var disabledJobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Disabled };
            var failedJob = new JenkinsJobItem { Color = JenkinsStatusColorNames.Red };
            var jobs = new List<JenkinsJobItem> { jobItem, successJobItem, disabledJobItem, failedJob };
            var url = new Uri("http://url.com");
            var jsonResult = "";
            var jobsModel = new JenkinsJobItemList { Jobs = jobs };

            httpClient.Setup(x => x.GetJsonStringAsync(url)).ReturnsAsync(jsonResult);
            jsonConventer.Setup(x => x.DeserializeObject<JenkinsJobItemList>(jsonResult)).Returns(jobsModel);

            // Act
            var result = repository.GetJobs(url);

            // Assert
            result.Result.Should().BeSameAs(jobsModel.Jobs);
        }

        #endregion

        #region get failed jobs

        [Test]
        public void GetFailedJobs_ReadingXmlFails_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var url = new Uri("http://url.com");
            var exception = new Exception();

            xmlFeedReader.Setup(x => x.GetFeedItems(url)).Throws(exception);
            loggingService.Setup(x => x.LogError(VerifyAny<string>(), VerifyAny<string>(), exception));

            // Act
            var result = repository.GetFailedJobs(url);

            // Assert
            result.Should().HaveCount(0);
        }

        [Test]
        public void GetFailedJobs_ReadingXmlSucceeds_ReturnFailedJobs()
        {
            // Arrange
            var url = new Uri("http://url.com");
            var feedItem = new SyndicationItem
            {
                Title = new TextSyndicationContent("MXDCMS Web Uygulaması #711 (broken since build #710)"),
                Links = { new SyndicationLink { Uri = new Uri("http://jenkins.activebuilder.local:8080/job/mxdcms/711/") } }
            };

            var feedItems = new List<SyndicationItem> { feedItem };
            var failedJobs = new List<JenkinsJobItem>
            {
                new JenkinsJobItem { Color = JenkinsStatusColorNames.Red }
            };
            var jenkinsJobItem = new JenkinsJobItem { Color = JenkinsStatusColorNames.Red };
            var jenkinsJobItems = new List<JenkinsJobItem> { jenkinsJobItem };

            xmlFeedReader.Setup(x => x.GetFeedItems(url)).Returns(feedItems);
            jenkinsFactory.Setup(x => x.CreateJenkinsJobItem(feedItem.Title.Text, feedItem.Links[0].Uri, JenkinsStatusColorNames.Red)).Returns(jenkinsJobItem);

            // Act
            var result = repository.GetFailedJobs(url);

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

        #region get job detail

        [Test]
        public void GetJobDetail_ServiceFailed_LogErrorAndReturnEmptyJobDetail()
        {
            // Arrange
            var url = new Uri("http://url.com");
            var exception = new Exception();

            httpClient.Setup(x => x.GetJsonStringAsync(url)).Throws(exception);
            loggingService.Setup(x => x.LogError(VerifyAny<string>(), VerifyAny<string>(), exception));

            // Act
            var result = repository.GetJobDetail(url);

            // Assert
            result.Should().NotBeNull();
        }

        [Test]
        public void GetJobDetail_AllSuccess_ReturnJobDetail()
        {
            // Arrange
            var lastBuild = new JenkinsJobBuild { Number = 1, Url = new Uri("http://url.com") };
            var lastSuccessfulBuild = new JenkinsJobBuild();
            var lastFailedBuild = new JenkinsJobBuild();
            var healtReport = new JenkinsJobHealthReport { Description = "description", IconUrl = new Uri("http://url.com"), Score = "100" };
            var healtReportList = new List<JenkinsJobHealthReport> { healtReport };
            var jobDetail = new JenkinsJobDetail
            {
                Url = new Uri("http://url.com"),
                Description = "description",
                FullDisplayName = "job-name-sq-daily",
                HealthReport = healtReportList,
                LastBuild = lastBuild,
                LastFailedBuild = lastFailedBuild,
                LastSuccessfulBuild = lastSuccessfulBuild
            };
            var url = new Uri("http://url.com");
            var jsonResult = "";

            httpClient.Setup(x => x.GetJsonStringAsync(url)).ReturnsAsync(jsonResult);
            jsonConventer.Setup(x => x.DeserializeObject<JenkinsJobDetail>(jsonResult)).Returns(jobDetail);

            // Act
            var result = repository.GetJobDetail(url);

            // Assert
            result.Result.Should().BeSameAs(jobDetail);
        }

        #endregion
    }
}