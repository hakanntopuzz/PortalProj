using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Model;
using DevPortal.SvnAdmin.Business;
using DevPortal.SvnAdmin.Data.Abstract;
using DevPortal.SvnAdmin.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.SvnAdminTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnAdminServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ILoggingService> loggingService;

        StrictMock<ISvnAdminRepository> svnAdminRepository;

        SvnAdminService service;

        [SetUp]
        public void Initialize()
        {
            loggingService = new StrictMock<ILoggingService>();
            svnAdminRepository = new StrictMock<ISvnAdminRepository>();

            service = new SvnAdminService(
                loggingService.Object,
                svnAdminRepository.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            svnAdminRepository.VerifyAll();
            loggingService.VerifyAll();
        }

        #endregion

        #region get repositories by last updated order

        [Test]
        public void GetRepositoriesByLastUpdatedOrder_RemoteOperationFailsWithSshClientException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var exception = new SshClientException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetRepositoriesByLastUpdatedOrder)}";

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByLastUpdatedOrder()).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetRepositoriesByLastUpdatedOrder();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void GetRepositoriesByLastUpdatedOrder_RemoteOperationFailsWithSshCommandResultParseException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var exception = new SshCommandResultParseException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetRepositoriesByLastUpdatedOrder)}";

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByLastUpdatedOrder()).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetRepositoriesByLastUpdatedOrder();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void GetRepositoriesByLastUpdatedOrder_RemoteOperationSuccess_ReturnSuccessResult()
        {
            // Arrange
            var repositoryResult = new List<SvnRepositoryFolderListItem>
            {
                new SvnRepositoryFolderListItem
                {
                    Name = "mahmut"
                }
            };

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByLastUpdatedOrder()).Returns(repositoryResult);

            // Act
            var result = service.GetRepositoriesByLastUpdatedOrder();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Value.Should().BeSameAs(repositoryResult);
        }

        #endregion

        #region get repositories by alphabetical order

        [Test]
        public void GetRepositoriesByAlphabeticalOrder_RemoteOperationFailsWithSshClientException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var exception = new SshClientException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetRepositoriesByAlphabeticalOrder)}";

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetRepositoriesByAlphabeticalOrder();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void GetRepositoriesByAlphabeticalOrder_RemoteOperationFailsWithSshCommandResultParseException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var exception = new SshCommandResultParseException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetRepositoriesByAlphabeticalOrder)}";

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = service.GetRepositoriesByAlphabeticalOrder();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
            result.Value.Should().BeEmpty();
        }

        [Test]
        public void GetRepositoriesByAlphabeticalOrder_RemoteOperationSuccess_ReturnSuccessResult()
        {
            // Arrange
            var repositoryResult = new List<SvnRepositoryFolderListItem>
            {
                new SvnRepositoryFolderListItem
                {
                    Name = "mahmut"
                }
            };

            svnAdminRepository.Setup(x => x.GetRepositoryFoldersByAlphabeticalOrder()).Returns(repositoryResult);

            // Act
            var result = service.GetRepositoriesByAlphabeticalOrder();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Value.Should().BeSameAs(repositoryResult);
        }

        #endregion

        #region get last updated repositories

        [Test]
        public void GetLastUpdatedRepositories_RemoteOperationFailsWithSshClientException_LogErrorAndReturnEmptyList()
        {
            // Arrange
            var key = CachedSvnAdminService.LastUpdatedRepositoriesCacheKey;
            var cacheTime = CachedSvnAdminService.LastUpdatedRepositoriesCacheTimeInMinutes;
            var exception = new SshClientException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetLastUpdatedRepositories)}";

            svnAdminRepository.Setup(x => x.GetLastUpdatedRepositoryFolders()).Throws(exception);
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
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetLastUpdatedRepositories)}";

            svnAdminRepository.Setup(x => x.GetLastUpdatedRepositoryFolders()).Throws(exception);
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

            svnAdminRepository.Setup(x => x.GetLastUpdatedRepositoryFolders()).Returns(items);

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
            var exception = new SshClientException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetRepositoryCount)}";

            svnAdminRepository.Setup(x => x.GetRepositoryFolderCount()).Throws(exception);
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
            var exception = new SshCommandResultParseException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.GetRepositoryCount)}";

            svnAdminRepository.Setup(x => x.GetRepositoryFolderCount()).Throws(exception);
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
            const int repositoryResult = 99;

            svnAdminRepository.Setup(x => x.GetRepositoryFolderCount()).Returns(repositoryResult);

            // Act
            var result = service.GetRepositoryCount();

            // Assert
            result.Should().Be(repositoryResult);
        }

        #endregion

        #region create svn repository folder

        [Test]
        public async Task CreateSvnRepositoryFolderAsync_SvnRepositoryFolderAlreadyExists_ReturnErrorResult()
        {
            // Arrange
            const string folderName = "mahmut";
            SvnRepositoryFolder folder = new SvnRepositoryFolder
            {
                Name = folderName
            };
            var exception = new SshClientException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.CreateSvnRepositoryFolderAsync)}";

            svnAdminRepository.Setup(x => x.DoesSvnRepositoryFolderExistAsync(folderName)).ReturnsAsync(true);

            // Act
            var result = await service.CreateSvnRepositoryFolderAsync(folder);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().Be(Messages.SvnRepositoryFolderAlreadyExists);
        }

        [Test]
        public async Task CreateSvnRepositoryFolderAsync_RemoteOperationFailsWithSshClientException_LogErrorAndReturnErrorResult()
        {
            // Arrange
            const string folderName = "mahmut";
            SvnRepositoryFolder folder = new SvnRepositoryFolder
            {
                Name = folderName
            };
            var exception = new SshClientException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.CreateSvnRepositoryFolderAsync)}";

            svnAdminRepository.Setup(x => x.DoesSvnRepositoryFolderExistAsync(folderName)).ReturnsAsync(false);
            svnAdminRepository.Setup(x => x.CreateSvnRepositoryFolderAsync(folder)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = await service.CreateSvnRepositoryFolderAsync(folder);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task CreateSvnRepositoryFolderAsync_RemoteOperationFailsWithSshCommandResultParseException_LogErrorAndReturnErrorResult()
        {
            // Arrange
            const string folderName = "mahmut";
            SvnRepositoryFolder folder = new SvnRepositoryFolder
            {
                Name = folderName
            };
            var exception = new SshCommandResultParseException();
            string methodName = $"{nameof(SvnAdminService)}.{nameof(SvnAdminService.CreateSvnRepositoryFolderAsync)}";

            svnAdminRepository.Setup(x => x.DoesSvnRepositoryFolderExistAsync(folderName)).ReturnsAsync(false);
            svnAdminRepository.Setup(x => x.CreateSvnRepositoryFolderAsync(folder)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, SetupAny<string>(), exception));

            // Act
            var result = await service.CreateSvnRepositoryFolderAsync(folder);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Message.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task CreateSvnRepositoryFolderAsync_RemoteOperationSuccess_ReturnSuccessResult()
        {
            // Arrange
            const string folderName = "mahmut";
            SvnRepositoryFolder folder = new SvnRepositoryFolder
            {
                Name = folderName
            };

            svnAdminRepository.Setup(x => x.DoesSvnRepositoryFolderExistAsync(folderName)).ReturnsAsync(false);
            svnAdminRepository.Setup(x => x.CreateSvnRepositoryFolderAsync(folder)).ReturnsAsync(true);

            // Act
            var result = await service.CreateSvnRepositoryFolderAsync(folder);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        #endregion
    }
}