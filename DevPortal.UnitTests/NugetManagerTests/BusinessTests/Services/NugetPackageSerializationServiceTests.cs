using AB.Framework.UnitTests;
using DevPortal.Data.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.NugetManager.Business;
using DevPortal.NugetManager.Business.Abstract;
using DevPortal.NugetManager.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevPortal.UnitTests.NugetManagerTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class NugetPackageSerializationServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<INugetServerRepository> nugetServerRepository;

        StrictMock<ISerializer> serializer;

        StrictMock<INugetFactory> nugetFactory;

        StrictMock<ISettings> settings;

        NugetPackageSerializationService service;

        [SetUp]
        public void Initialize()
        {
            nugetServerRepository = new StrictMock<INugetServerRepository>();
            serializer = new StrictMock<ISerializer>();
            nugetFactory = new StrictMock<INugetFactory>();
            settings = new StrictMock<ISettings>();

            service = new NugetPackageSerializationService(
                nugetServerRepository.Object,
                serializer.Object,
                nugetFactory.Object,
                settings.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            nugetServerRepository.VerifyAll();
            serializer.VerifyAll();
            nugetFactory.VerifyAll();
            settings.VerifyAll();
        }

        #endregion

        [Test]
        public void GetNugetPackages_SerializedListNull_ReturnEmptyList()
        {
            // Arrange
            var skip = 0;
            var package1 = new NugetPackage { Id = "package-1" };
            var package2 = new NugetPackage { Id = "package-2" };
            var packages = new List<NugetPackage> {
                package1,
                package2
            };

            string packagesXml = "packages-xml";
            NugetPackageList serializedPackages = null;
            var expectedPackageList = new List<NugetPackage>();

            nugetServerRepository.Setup(x => x.GetPackages(skip)).Returns(packagesXml);
            serializer.Setup(x => x.Deserialize<NugetPackageList>(packagesXml)).Returns(serializedPackages);

            //Act
            var result = service.GetNugetPackages(skip, packages);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }

        [Test]
        public void GetNugetPackages_NugetPackageListParameterNullAndCreatedModelNull_ReturnEmptyList()
        {
            // Arrange
            var skip = 0;
            List<NugetPackage> packages = null;

            string packagesXml = "packages-xml";
            var serializedPackages = new NugetPackageList();
            List<NugetPackage> packagesModel = null;

            var expectedPackageList = new List<NugetPackage>();

            nugetServerRepository.Setup(x => x.GetPackages(skip)).Returns(packagesXml);
            serializer.Setup(x => x.Deserialize<NugetPackageList>(packagesXml)).Returns(serializedPackages);
            nugetFactory.Setup(x => x.CreateNugetPackageModel()).Returns(packagesModel);

            //Act
            var result = service.GetNugetPackages(skip, packages);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }

        [Test]
        public void GetNugetPackages_SerializedListEmpty_ReturnPackageListParameter()
        {
            // Arrange
            var skip = 0;
            List<NugetPackage> packages = null;

            string packagesXml = "packages-xml";

            var package1 = new NugetPackage { Id = "package-1" };
            var serializedPackages = new NugetPackageList();

            var packagesModel = new List<NugetPackage> { package1 };
            int pageSize = 1;

            var expectedPackageList = packagesModel;

            nugetServerRepository.Setup(x => x.GetPackages(skip)).Returns(packagesXml);
            serializer.Setup(x => x.Deserialize<NugetPackageList>(packagesXml)).Returns(serializedPackages);
            nugetFactory.Setup(x => x.CreateNugetPackageModel()).Returns(packagesModel);
            settings.Setup(x => x.NumberOfNugetPackagePaging).Returns(pageSize);

            //Act
            var result = service.GetNugetPackages(skip, packages);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }

        [Test]
        public void GetNugetPackages_SerializedListNotEmpty_ReturnPackageListParameterAndSerializedListCombined()
        {
            // Arrange
            var skip = 0;
            List<NugetPackage> packages = null;

            string packagesXml = "packages-xml";
            string packagesXml2 = "packages-xml-2";

            var package1 = new NugetPackage { Id = "package-1" };
            var serializedPackages = new NugetPackageList
            {
                NugetPackageModelList = new Collection<NugetPackage> { package1 }
            };
            NugetPackageList serializedPackages2 = null;

            var packagesModel = new List<NugetPackage> { package1 };
            int pageSize = 1;

            packagesModel.AddRange(serializedPackages.NugetPackageModelList);
            var expectedPackageList = packagesModel;

            nugetServerRepository.Setup(x => x.GetPackages(skip)).Returns(packagesXml);
            serializer.Setup(x => x.Deserialize<NugetPackageList>(packagesXml)).Returns(serializedPackages);
            nugetFactory.Setup(x => x.CreateNugetPackageModel()).Returns(packagesModel);
            settings.Setup(x => x.NumberOfNugetPackagePaging).Returns(pageSize);

            nugetServerRepository.Setup(x => x.GetPackages(skip + pageSize)).Returns(packagesXml2);
            serializer.Setup(x => x.Deserialize<NugetPackageList>(packagesXml2)).Returns(serializedPackages2);

            //Act
            var result = service.GetNugetPackages(skip, packages);

            // Assert
            result.Should().BeEquivalentTo(expectedPackageList);
        }
    }
}