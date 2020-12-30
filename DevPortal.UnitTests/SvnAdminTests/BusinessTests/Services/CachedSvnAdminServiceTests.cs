using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Framework.Abstract;
using DevPortal.SvnAdmin.Business;
using DevPortal.SvnAdmin.Data.Abstract;
using DevPortal.SvnAdmin.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.SvnAdminTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CachedSvnAdminServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ILoggingService> loggingService;

        StrictMock<ISvnAdminRepository> svnAdminRepository;

        StrictMock<ICacheWrapper> cache;

        CachedSvnAdminService service;

        [SetUp]
        public void Initialize()
        {
            loggingService = new StrictMock<ILoggingService>();
            svnAdminRepository = new StrictMock<ISvnAdminRepository>();
            cache = new StrictMock<ICacheWrapper>();

            service = new CachedSvnAdminService(
                loggingService.Object,
                svnAdminRepository.Object,
                cache.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            svnAdminRepository.VerifyAll();
            cache.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region overrides

        [Test]
        public void GetLastUpdatedRepositories_RemoteOperationFailsWithSshClientException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var key = CachedSvnAdminService.LastUpdatedRepositoriesCacheKey;
            var cacheTime = CachedSvnAdminService.LastUpdatedRepositoriesCacheTimeInMinutes;
            var exception = new SshClientException();
            string methodName = $"{nameof(CachedSvnAdminService)}.{nameof(CachedSvnAdminService.GetLastUpdatedRepositories)}";

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration<ICollection<SvnRepositoryFolderListItem>>(
                key, SetupAny<Func<ICollection<SvnRepositoryFolderListItem>>>(), cacheTime)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetLastUpdatedRepositories();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void GetLastUpdatedRepositories_RemoteOperationFailsWithSshCommandResultParseException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var key = CachedSvnAdminService.LastUpdatedRepositoriesCacheKey;
            var cacheTime = CachedSvnAdminService.LastUpdatedRepositoriesCacheTimeInMinutes;
            var exception = new SshCommandResultParseException();
            string methodName = $"{nameof(CachedSvnAdminService)}.{nameof(CachedSvnAdminService.GetLastUpdatedRepositories)}";

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration<ICollection<SvnRepositoryFolderListItem>>(
                key, SetupAny<Func<ICollection<SvnRepositoryFolderListItem>>>(), cacheTime)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetLastUpdatedRepositories();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void GetLastUpdatedRepositories_RemoteOperationSuccess_ReturnSuccessResult()
        {
            // Arrange
            var key = CachedSvnAdminService.LastUpdatedRepositoriesCacheKey;
            var cacheTime = CachedSvnAdminService.LastUpdatedRepositoriesCacheTimeInMinutes;
            var items = new List<SvnRepositoryFolderListItem>
            {
                new SvnRepositoryFolderListItem
                {
                    Name = "mahmut"
                }
            };

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration<ICollection<SvnRepositoryFolderListItem>>(
                key, SetupAny<Func<ICollection<SvnRepositoryFolderListItem>>>(), cacheTime)).Returns(items);

            // Act
            var result = service.GetLastUpdatedRepositories();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Value.Should().BeSameAs(items);
        }

        #endregion

        #region get repository count

        [Test]
        public void GetRepositoryCount_RemoteOperationFailsWithSshClientException_LogErrorAndReturnEmptyResult()
        {
            // Arrange
            var cacheKey = CachedSvnAdminService.RepositoryCountCacheKey;
            var cacheTime = CachedSvnAdminService.RepositoryCountCacheTimeInMinutes;
            var exception = new SshClientException();
            string methodName = $"{nameof(CachedSvnAdminService)}.{nameof(CachedSvnAdminService.GetRepositoryCount)}";

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration<int>(
                cacheKey,
                SetupAny<Func<int>>(),
                cacheTime)
            ).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetRepositoryCount();

            // Assert
            result.Should().Be(0);
        }

        [Test]
        public void GetRepositoryCount_RemoteOperationFailsWithSshCommandResultParseException_LogErrorAndReturnEmptyResult()
        {
            // Arrange
            var cacheKey = CachedSvnAdminService.RepositoryCountCacheKey;
            var cacheTime = CachedSvnAdminService.RepositoryCountCacheTimeInMinutes;
            var exception = new SshCommandResultParseException();
            string methodName = $"{nameof(CachedSvnAdminService)}.{nameof(CachedSvnAdminService.GetRepositoryCount)}";

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration<int>(
                cacheKey,
                SetupAny<Func<int>>(),
                cacheTime)
            ).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetRepositoryCount();

            // Assert
            result.Should().Be(0);
        }

        [Test]
        public void GetRepositoryCount_RemoteOperationSuccess_ReturnSuccessResult()
        {
            // Arrange
            var cacheKey = CachedSvnAdminService.RepositoryCountCacheKey;
            var cacheTime = CachedSvnAdminService.RepositoryCountCacheTimeInMinutes;
            const int repositoryResult = 99;

            cache.Setup(x => x.GetOrCreateWithSlidingExpiration<int>(
                cacheKey,
                SetupAny<Func<int>>(),
                cacheTime)
            ).Returns(repositoryResult);

            // Act
            var result = service.GetRepositoryCount();

            // Assert
            result.Should().Be(repositoryResult);
        }

        #endregion
    }
}