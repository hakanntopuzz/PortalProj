using AB.Framework.UnitTests;
using DevPortal.Business.Abstract;
using DevPortal.Business.Services;
using DevPortal.Framework.Extensions;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FullApplicationReaderServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IApplicationReaderService> applicationReaderService;

        StrictMock<IApplicationDependencyReaderService> applicationDependencyReaderService;

        StrictMock<IApplicationEnvironmentService> applicationEnvironmentService;

        StrictMock<IApplicationSonarqubeProjectService> applicationSonarqubeProjectService;

        StrictMock<IDatabaseDependencyService> databaseDependencyService;

        StrictMock<IApplicationNugetPackageService> applicationNugetPackageService;

        StrictMock<IApplicationFactory> applicationFactory;

        FullApplicationReaderService service;

        [SetUp]
        public void Initialize()
        {
            applicationReaderService = new StrictMock<IApplicationReaderService>();
            applicationDependencyReaderService = new StrictMock<IApplicationDependencyReaderService>();
            applicationEnvironmentService = new StrictMock<IApplicationEnvironmentService>();
            applicationSonarqubeProjectService = new StrictMock<IApplicationSonarqubeProjectService>();
            databaseDependencyService = new StrictMock<IDatabaseDependencyService>();
            applicationNugetPackageService = new StrictMock<IApplicationNugetPackageService>();
            applicationFactory = new StrictMock<IApplicationFactory>();

            service = new FullApplicationReaderService(
                applicationReaderService.Object,
                applicationDependencyReaderService.Object,
                applicationEnvironmentService.Object,
                applicationSonarqubeProjectService.Object,
                databaseDependencyService.Object,
                applicationNugetPackageService.Object,
                applicationFactory.Object
                );
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            applicationReaderService.VerifyAll();
            applicationDependencyReaderService.VerifyAll();
            applicationEnvironmentService.VerifyAll();
            applicationSonarqubeProjectService.VerifyAll();
            databaseDependencyService.VerifyAll();
            applicationNugetPackageService.VerifyAll();
            applicationFactory.VerifyAll();
        }

        #endregion

        #region get application with all members

        [Test]
        public void GetApplicationWithAllMembers_ApplicationTypeNugetPackage_ReturnPackageList()
        {
            // Arrange
            var applicationId = 1;
            var applicationFullModel = new ApplicationFullModel();
            var application = new Application { ApplicationTypeId = ApplicationTypeEnum.NugetPackage.ToInt32() };
            var environments = new List<ApplicationEnvironment>();
            var jenkinsJobs = new List<JenkinsJob>();
            var sonarqubeProjects = new List<SonarqubeProject>();
            var svnRepositories = new List<SvnRepository>();
            var applicationDependencies = new List<ApplicationDependency>();
            var databaseDependencies = new List<DatabaseDependency>();
            var externalDependencies = new List<ExternalDependency>();
            var nugetDependencies = new List<NugetPackageDependency>();
            var nugetPackages = new List<ApplicationNugetPackage>();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            applicationReaderService.Setup(x => x.GetSvnRepositories(applicationId)).Returns(svnRepositories);
            applicationReaderService.Setup(x => x.GetJenkinsJobs(applicationId)).Returns(jenkinsJobs);
            applicationSonarqubeProjectService.Setup(x => x.GetSonarqubeProjects(applicationId)).Returns(sonarqubeProjects);
            applicationDependencyReaderService.Setup(x => x.GetApplicationDependencies(applicationId)).Returns(applicationDependencies);
            databaseDependencyService.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(databaseDependencies);
            applicationDependencyReaderService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalDependencies);
            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetDependencies);
            applicationNugetPackageService.Setup(x => x.GetNugetPackages(applicationId)).Returns(nugetPackages);

            applicationFactory.Setup(x => x.CreateNugetApplicationFullModel(
                application,
                nugetPackages,
                jenkinsJobs,
                sonarqubeProjects,
                svnRepositories,
                applicationDependencies,
                databaseDependencies,
                externalDependencies,
                nugetDependencies)).Returns(applicationFullModel);

            // Act
            var result = service.GetApplicationWithAllMembers(applicationId);

            // Assert
            result.Should().BeEquivalentTo(applicationFullModel);
        }

        [Test]
        public void GetApplicationWithAllMembers_ApplicationTypeNotNugetPackage_ReturnPackageList()
        {
            // Arrange
            var applicationId = 1;
            var applicationFullModel = new ApplicationFullModel();
            var application = new Application { ApplicationTypeId = ApplicationTypeEnum.None.ToInt32() };
            var environments = new List<ApplicationEnvironment>();
            var jenkinsJobs = new List<JenkinsJob>();
            var sonarqubeProjects = new List<SonarqubeProject>();
            var svnRepositories = new List<SvnRepository>();
            var applicationDependencies = new List<ApplicationDependency>();
            var databaseDependencies = new List<DatabaseDependency>();
            var externalDependencies = new List<ExternalDependency>();
            var nugetDependencies = new List<NugetPackageDependency>();
            var nugetPackages = new List<ApplicationNugetPackage>();

            applicationReaderService.Setup(x => x.GetApplication(applicationId)).Returns(application);
            applicationReaderService.Setup(x => x.GetSvnRepositories(applicationId)).Returns(svnRepositories);
            applicationReaderService.Setup(x => x.GetJenkinsJobs(applicationId)).Returns(jenkinsJobs);
            applicationSonarqubeProjectService.Setup(x => x.GetSonarqubeProjects(applicationId)).Returns(sonarqubeProjects);
            applicationDependencyReaderService.Setup(x => x.GetApplicationDependencies(applicationId)).Returns(applicationDependencies);
            databaseDependencyService.Setup(x => x.GetDatabaseDependenciesByApplicationId(applicationId)).Returns(databaseDependencies);
            applicationDependencyReaderService.Setup(x => x.GetExternalDependencies(applicationId)).Returns(externalDependencies);
            applicationDependencyReaderService.Setup(x => x.GetNugetPackageDependencies(applicationId)).Returns(nugetDependencies);
            applicationEnvironmentService.Setup(x => x.GetApplicationEnvironmentsHasLog(applicationId)).Returns(environments);

            applicationFactory.Setup(x => x.CreateApplicationFullModel(
                application,
                environments,
                jenkinsJobs,
                sonarqubeProjects,
                svnRepositories,
                applicationDependencies,
                databaseDependencies,
                externalDependencies,
                nugetDependencies)).Returns(applicationFullModel);

            // Act
            var result = service.GetApplicationWithAllMembers(applicationId);

            // Assert
            result.Should().BeEquivalentTo(applicationFullModel);
        }

        #endregion
    }
}