using AB.Framework.UnitTests;
using DevPortal.Framework.Extensions;
using DevPortal.SvnAdmin.Data;
using DevPortal.SvnAdmin.Data.Abstract;
using DevPortal.SvnAdmin.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.SvnAdminTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnAdminRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<ISshClient> sshClient;

        StrictMock<ISshCommandResultParser> sshCommandResultParser;

        SvnAdminRepository repository;

        [SetUp]
        public void Initialize()
        {
            sshClient = new StrictMock<ISshClient>();
            sshCommandResultParser = new StrictMock<ISshCommandResultParser>();

            repository = new SvnAdminRepository(sshClient.Object, sshCommandResultParser.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            sshClient.VerifyAll();
            sshCommandResultParser.VerifyAll();
        }

        #endregion

        #region get repositories by last updated order

        [Test]
        public void GetRepositoriesByLastUpdatedOrder_ExecuteCommandFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryListByLastUpdatedOrder;
            const string message = "mahmut-tuncer";
            var exception = new SshClientException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetRepositoryFoldersByLastUpdatedOrder());
        }

        [Test]
        public void GetRepositoriesByLastUpdatedOrder_ParseRepositoryListFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryListByLastUpdatedOrder;
            const string commandResult = "mahmut-tuncer";
            const string message = "mustafa-keser";
            var exception = new SshCommandResultParseException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseRepositoryList(commandResult)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetRepositoryFoldersByLastUpdatedOrder());
        }

        [Test]
        public void GetRepositoriesByLastUpdatedOrder_ParseSuccess_ReturnRepositories()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryListByLastUpdatedOrder;
            const string commandResult = "mahmut-tuncer";
            var repositories = new List<SvnRepositoryFolderListItem> {
                new SvnRepositoryFolderListItem
                {
                    Name = "mustafa-keser"
                }
            };

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseRepositoryList(commandResult)).Returns(repositories);

            // Act
            var result = repository.GetRepositoryFoldersByLastUpdatedOrder();

            // Assert
            result.Should().BeSameAs(repositories);
        }

        #endregion

        #region get repositories by alphabetical order

        [Test]
        public void GetRepositoriesByAlphabeticalOrder_ExecuteCommandFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryListByAlphabeticalOrder;
            const string message = "mahmut-tuncer";
            var exception = new SshClientException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetRepositoryFoldersByAlphabeticalOrder());
        }

        [Test]
        public void GetRepositoriesByAlphabeticalOrder_ParseRepositoryListFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryListByAlphabeticalOrder;
            const string commandResult = "mahmut-tuncer";
            const string message = "mustafa-keser";
            var exception = new SshCommandResultParseException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseRepositoryList(commandResult)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetRepositoryFoldersByAlphabeticalOrder());
        }

        [Test]
        public void GetRepositoriesByAlphabeticalOrder_ParseSuccess_ReturnRepositories()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryListByAlphabeticalOrder;
            const string commandResult = "mahmut-tuncer";
            var repositories = new List<SvnRepositoryFolderListItem> {
                new SvnRepositoryFolderListItem
                {
                    Name = "mustafa-keser"
                }
            };

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseRepositoryList(commandResult)).Returns(repositories);

            // Act
            var result = repository.GetRepositoryFoldersByAlphabeticalOrder();

            // Assert
            result.Should().BeSameAs(repositories);
        }

        #endregion

        #region get last updated repositories

        [Test]
        public void GetLastUpdatedRepositories_ExecuteCommandFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.LastUpdatedRepositoryList;
            const string message = "mahmut-tuncer";
            var exception = new SshClientException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetLastUpdatedRepositoryFolders());
        }

        [Test]
        public void GetLastUpdatedRepositories_ParseRepositoryListFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.LastUpdatedRepositoryList;
            const string commandResult = "mahmut-tuncer";
            const string message = "mustafa-keser";
            var exception = new SshCommandResultParseException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseRepositoryList(commandResult)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetLastUpdatedRepositoryFolders());
        }

        [Test]
        public void GetLastUpdatedRepositories_ParseSuccess_ReturnRepositories()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.LastUpdatedRepositoryList;
            const string commandResult = "mahmut-tuncer";
            var repositories = new List<SvnRepositoryFolderListItem> {
                new SvnRepositoryFolderListItem
                {
                    Name = "mustafa-keser"
                }
            };

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseRepositoryList(commandResult)).Returns(repositories);

            // Act
            var result = repository.GetLastUpdatedRepositoryFolders();

            // Assert
            result.Should().BeSameAs(repositories);
        }

        #endregion

        #region get repository count

        [Test]
        public void GetRepositoryCount_ExecuteCommandFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryCount;
            const string message = "mahmut-tuncer";
            var exception = new SshClientException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetRepositoryFolderCount());
        }

        [Test]
        public void GetRepositoryCount_ParseRepositoryListFails_ThrowException()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryCount;
            const string commandResult = "mahmut-tuncer";
            const string message = "mustafa-keser";
            var exception = new SshCommandResultParseException(message);

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseInt32(commandResult)).Throws(exception);

            // Act

            // Assert
            Assert.Throws(exception.GetType(), () => repository.GetRepositoryFolderCount());
        }

        [Test]
        public void GetRepositoryCount_ParseSuccess_ReturnRepositories()
        {
            // Arrange
            const string commandText = SshClientCommandTexts.RepositoryCount;
            const string commandResult = "mahmut-tuncer";
            const int count = 99;

            sshClient.Setup(x => x.ExecuteCommand(commandText)).Returns(commandResult);
            sshCommandResultParser.Setup(x => x.ParseInt32(commandResult)).Returns(count);

            // Act
            var result = repository.GetRepositoryFolderCount();

            // Assert
            result.Should().Be(count);
        }

        #endregion

        #region create svn repository folder

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task CreateSvnRepositoryFolderAsync_NoCondition_ReturnParsedResult(bool parsedResult)
        {
            // Arrange
            var folder = new SvnRepositoryFolder
            {
                Name = "mahmut",
                PostCommitHookEnabled = true,
                PreRevpropChangeHookEnabled = true
            };
            var commandText = string.Format(SshClientCommandTexts.CreateSvnRepositoryFolder,
                folder.Name,
                folder.PreRevpropChangeHookEnabled.ToInt32(),
                folder.PostCommitHookEnabled.ToInt32());

            const string commandResult = "result";

            sshClient.Setup(x => x.ExecuteCommandAsync(commandText)).ReturnsAsync(commandResult);
            sshCommandResultParser.Setup(x => x.ParseBool(commandResult)).Returns(parsedResult);

            // Act
            var result = await repository.CreateSvnRepositoryFolderAsync(folder);

            // Assert
            result.Should().Be(parsedResult);
        }

        #endregion

        #region does svn repository folder exist

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task DoesSvnRepositoryFolderExistAsync_NoCondition_ReturnParsedResult(bool parsedResult)
        {
            // Arrange
            const string folderName = "mahmut";
            var commandText = string.Format(SshClientCommandTexts.DoesSvnRepositoryFolderExist,
                folderName);

            const string commandResult = "result";

            sshClient.Setup(x => x.ExecuteCommandAsync(commandText)).ReturnsAsync(commandResult);
            sshCommandResultParser.Setup(x => x.ParseBool(commandResult)).Returns(parsedResult);

            // Act
            var result = await repository.DoesSvnRepositoryFolderExistAsync(folderName);

            // Assert
            result.Should().Be(parsedResult);
        }

        #endregion
    }
}