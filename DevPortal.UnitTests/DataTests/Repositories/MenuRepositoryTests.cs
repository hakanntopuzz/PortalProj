using AB.Data.DapperClient.Abstract;
using AB.Data.DapperClient.Model.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Data.Abstract.Factories;
using DevPortal.Data.Factories;
using DevPortal.Data.Repositories;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevPortal.UnitTests.DataTests.Repositories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class MenuRepositoryTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IDataClient> dataClient;

        StrictMock<IApplicationDataRequestFactory> dataRequestFactory;

        StrictMock<ISettings> settings;

        MenuRepository repository;

        [SetUp]
        public void Initialize()
        {
            dataClient = new StrictMock<IDataClient>();
            dataRequestFactory = new StrictMock<IApplicationDataRequestFactory>();
            settings = new StrictMock<ISettings>();
            SetupDataClient();
            repository = new MenuRepository(dataClient.Object, dataRequestFactory.Object, settings.Object);
        }

        void SetupDataClient()
        {
            const string devPortalDbConnectionString = "devPortalDbConnectionString";
            settings.SetupGet(x => x.DevPortalDbConnectionString).Returns(devPortalDbConnectionString);
            dataClient.Setup(x => x.SetConnectionString(devPortalDbConnectionString));
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            dataClient.VerifyAll();
            dataRequestFactory.VerifyAll();
            settings.VerifyAll();
        }

        #endregion

        #region get filtered menu list

        [Test]
        public async Task GetFilteredMenuListAsync_NoCondition_ReturnMenuList()
        {
            //Arrange
            var defaultValue = new List<MenuModel>();
            var tableParam = new MenuTableParam();
            ICollection<MenuModel> expectedValue = null;
            var dataRequest = new Mock<IDataRequest>();

            dataRequestFactory.Setup(x => x.GetFilteredMenuList(tableParam)).Returns(dataRequest.Object);
            dataClient.Setup(x => x.GetCollectionAsync<MenuModel, RecordUpdateInfo, MenuModel>(
                dataRequest.Object,
                DataClientMapFactory.MenusMap,
                defaultValue,
                dataRequest.Object.SplitOnParameters)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetFilteredMenuListAsync(tableParam);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region get menu list

        [Test]
        public async Task GetMenuListAsync_NoCondition_ReturnMenuList()
        {
            //Arrange
            IDataRequest dataRequest = null;
            var defaultValue = new List<MenuModel>();
            ICollection<MenuModel> expectedValue = null;

            dataRequestFactory.Setup(x => x.GetMenuList()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetMenuListAsync();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region add menu

        [Test]
        public async Task AddMenuAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var menuModel = new MenuModel();
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.AddMenu(menuModel)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.AddMenuAsync(menuModel);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region get menu

        [Test]
        public async Task GetMenuAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var id = 3;
            IDataRequest dataRequest = null;
            MenuModel defaultValue = null;
            var expectedValue = new MenuModel();

            dataRequestFactory.Setup(x => x.GetMenu(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetMenuAsync(id);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region get sub menu list

        [Test]
        public async Task GetSubMenuListAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var id = 3;
            IDataRequest dataRequest = null;
            ICollection<MenuModel> defaultValue = new List<MenuModel>();
            var expectedValue = new List<MenuModel>();

            dataRequestFactory.Setup(x => x.GetSubMenuList(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetSubMenuListAsync(id);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region update menu

        [Test]
        public async Task UpdateMenuAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var menuModel = new MenuModel();
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.UpdateMenu(menuModel)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.UpdateMenuAsync(menuModel);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region delete menu

        [Test]
        public async Task DeleteMenuAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var menuId = 1;
            IDataRequest dataRequest = null;
            var defaultValue = false;
            var expectedValue = true;

            dataRequestFactory.Setup(x => x.DeleteMenu(menuId)).Returns(dataRequest);
            dataClient.Setup(x => x.GetScalarAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.DeleteMenuAsync(menuId);

            //Assert
            result.Should().Be(expectedValue);
        }

        #endregion

        #region get menu groups async

        [Test]
        public async Task GetMenuGroupsAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            IDataRequest dataRequest = null;
            ICollection<MenuGroup> defaultValue = new List<MenuGroup>();
            var expectedValue = new List<MenuGroup>();

            dataRequestFactory.Setup(x => x.GetMenuGroups()).Returns(dataRequest);
            dataClient.Setup(x => x.GetCollectionAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetMenuGroupsAsync();

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion

        #region get menu update info async

        [Test]
        public async Task GetMenuUpdateInfoAsync_NoCondition_ReturnSuccessResult()
        {
            //Arrange
            var id = 3;
            IDataRequest dataRequest = null;
            RecordUpdateInfo defaultValue = null;
            var expectedValue = new RecordUpdateInfo();

            dataRequestFactory.Setup(x => x.GetMenuUpdateInfo(id)).Returns(dataRequest);
            dataClient.Setup(x => x.GetItemAsync(dataRequest, defaultValue)).ReturnsAsync(expectedValue);

            //Act
            var result = await repository.GetMenuUpdateInfoAsync(id);

            //Assert
            result.Should().BeSameAs(expectedValue);
        }

        #endregion
    }
}