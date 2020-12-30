using AB.Framework.UnitTests;
using DevPortal.Business.Factories;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevPortal.UnitTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        ApplicationFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new ApplicationFactory();
        }

        #endregion

        [Test]
        public void CreateApplicationFullModel_NoCondition_ReturnApplicationFullModel()
        {
            // Arrange
            var application = new Application();
            var applicationEnvironments = new List<ApplicationEnvironment>();
            var jenkinsJobs = new List<JenkinsJob>();
            var sonarqubeProjects = new List<SonarqubeProject>();
            var svnRepositories = new List<SvnRepository>();
            var applicationDependencies = new List<ApplicationDependency>();
            var databaseDependencies = new List<DatabaseDependency>();
            var externalDependencies = new List<ExternalDependency>();
            var nugetPackageDependencies = new List<NugetPackageDependency>();

            var applicationFullModel = new ApplicationFullModel
            {
                Application = application,
                ApplicationEnvironments = applicationEnvironments,
                JenkinsJobs = jenkinsJobs,
                SonarqubeProjects = sonarqubeProjects,
                SvnRepositories = svnRepositories,
                ApplicationDependencies = applicationDependencies,
                DatabaseDependencies = databaseDependencies,
                ExternalDependencies = externalDependencies,
                NugetPackageDependencies = nugetPackageDependencies
            };

            // Act
            var result = factory.CreateApplicationFullModel(application, applicationEnvironments, jenkinsJobs, sonarqubeProjects, svnRepositories,
                applicationDependencies, databaseDependencies, externalDependencies, nugetPackageDependencies);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(applicationFullModel);
        }

        [Test]
        public void CreateNugetApplicationFullModel_NoCondition_ReturnApplicationFullModel()
        {
            // Arrange
            var application = new Application();
            var nugetPackages = new List<ApplicationNugetPackage>();
            var jenkinsJobs = new List<JenkinsJob>();
            var sonarqubeProjects = new List<SonarqubeProject>();
            var svnRepositories = new List<SvnRepository>();
            var applicationDependencies = new List<ApplicationDependency>();
            var databaseDependencies = new List<DatabaseDependency>();
            var externalDependencies = new List<ExternalDependency>();
            var nugetPackageDependencies = new List<NugetPackageDependency>();

            var applicationFullModel = new ApplicationFullModel
            {
                Application = application,
                ApplicationNugetPackages = nugetPackages,
                JenkinsJobs = jenkinsJobs,
                SonarqubeProjects = sonarqubeProjects,
                SvnRepositories = svnRepositories,
                ApplicationDependencies = applicationDependencies,
                DatabaseDependencies = databaseDependencies,
                ExternalDependencies = externalDependencies,
                NugetPackageDependencies = nugetPackageDependencies
            };

            // Act
            var result = factory.CreateNugetApplicationFullModel(application, nugetPackages, jenkinsJobs, sonarqubeProjects, svnRepositories,
                applicationDependencies, databaseDependencies, externalDependencies, nugetPackageDependencies);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(applicationFullModel);
        }

        [Test]
        public void CreateDependencyFullModel_NoCondition_ReturnExportListItems()
        {
            // Arrange
            var applicationDependenciesExportListItems = new List<ApplicationDependenciesExportListItem>
            {
                 new ApplicationDependenciesExportListItem
                {
                    Name = "application",
                    ApplicationGroupName = "application-group",
                    Description = "description",
                    CreatedDate =         new System.DateTime(2020, 6, 20, 1, 30, 24),
                    CreatedUserEmail = "CreatedUserEmail",
                    ModifiedDate = new System.DateTime(2020, 6, 20, 1, 30, 24),
                    ModifiedUserEmail = "ModifiedUserEmail"
                }
            };

            var databaseDependenciesExportListItems = new List<DatabaseDependenciesExportListItem>
            {
                 new DatabaseDependenciesExportListItem
                {
                    Name = "database",
                    DatabaseGroupName = "database-group",
                    Description = "description",
                    CreatedDate =         new System.DateTime(2020, 6, 20, 1, 30, 24),
                    CreatedUserEmail = "CreatedUserEmail",
                    ModifiedDate = new System.DateTime(2020, 6, 20, 1, 30, 24),
                    ModifiedUserEmail = "ModifiedUserEmail"
                  }
            };

            var externalDependenciesExportListItems = new List<ExternalDependenciesExportListItem>
            {
                 new ExternalDependenciesExportListItem
                {
                    Name = "external",
                    Description = "description",
                    CreatedDate =         new System.DateTime(2020, 6, 20, 1, 30, 24),
                    CreatedUserEmail = "CreatedUserEmail",
                    ModifiedDate = new System.DateTime(2020, 6, 20, 1, 30, 24),
                    ModifiedUserEmail = "ModifiedUserEmail"
    }
            };

            var dependenciesExportListItem = new DependenciesExportListItem
            {
                ApplicationDependenciesExportListItem = applicationDependenciesExportListItems,
                DatabaseDependenciesExportListItem = databaseDependenciesExportListItems,
                ExternalDependenciesExportListItem = externalDependenciesExportListItems
            };

            // Act
            var result = factory.CreateDependencyFullModel(applicationDependenciesExportListItems, databaseDependenciesExportListItems, externalDependenciesExportListItems);

            // Assert
            result.Should().BeEquivalentTo(dependenciesExportListItem);
        }

        [Test]
        public void CreateApplicationExportListItems_NoCondition_ReturnExportListItems()
        {
            // Arrange
            var application1 = new ApplicationListItem
            {
                Id = 45,
                Name = "application-1",
                ApplicationGroupName = "application-group-1",
                ApplicationTypeName = "application-type-1",
                ModifiedDate = new System.DateTime(2020, 6, 20, 1, 30, 24),
                Status = "active",
                RedmineProjectName = "pplication-1"
            };

            var applications = new List<ApplicationListItem> {
                application1
            };

            var applicationExportListItems = new List<ApplicationExportListItem> {
                new ApplicationExportListItem{
                    Name = application1.Name,
                    ApplicationGroupName = application1.ApplicationGroupName,
                    ApplicationTypeName = application1.ApplicationTypeName,
                    Status = application1.Status
                }
            };

            // Act
            var result = factory.CreateApplicationExportListItems(applications);

            // Assert
            result.Should().BeEquivalentTo(applicationExportListItems);
        }

        [Test]
        public void CreateApplicationWikiExportListItems_NoCondition_ReturnExportListItems()
        {
            // Arrange
            var application1 = new ApplicationListItem
            {
                Id = 45,
                Name = "application-1",
                ApplicationGroupName = "application-group-1",
                ApplicationTypeName = "application-type-1",
                ModifiedDate = new System.DateTime(2020, 6, 20, 1, 30, 24),
                Status = "active",
                RedmineProjectName = "application-1"
            };
            var application2 = new ApplicationListItem
            {
                Id = 46,
                Name = "application-2",
                ApplicationGroupName = "application-group-2",
                ApplicationTypeName = "application-type-2",
                ModifiedDate = new System.DateTime(2020, 6, 21, 2, 30, 24),
                Status = "passive",
                RedmineProjectName = "application-2"
            };

            var applications = new List<ApplicationListItem> {
                application1,
                application2
            };

            var applicationsWikiExportItems = new List<ApplicationWikiExportListItem> {
                new ApplicationWikiExportListItem{
                    Name = application1.Name,
                    ApplicationGroupName = application1.ApplicationGroupName,
                    ApplicationTypeName = application1.ApplicationTypeName,
                    Status = application1.Status,
                    RedmineProjectName = application1.RedmineProjectName
                },
                 new ApplicationWikiExportListItem{
                    Name = application2.Name,
                    ApplicationGroupName = application2.ApplicationGroupName,
                    ApplicationTypeName = application2.ApplicationTypeName,
                    Status = application2.Status,
                    RedmineProjectName = application2.RedmineProjectName
                },
            };

            // Act
            var result = factory.CreateApplicationWikiExportListItems(applications);

            // Assert
            result.Should().BeEquivalentTo(applicationsWikiExportItems);
        }

        [Test]
        public void CreateNugetApplicationFullModel_NoConditionM_ReturnApplicationFullModel()
        {
            // Arrange
            var application = new Application
            {
                Status = "Status",
                RedmineProjectUrl = "RedmineProjectUrl"
            };

            var applicationNugetPackage = new List<ApplicationNugetPackage> {
               new ApplicationNugetPackage
               {
                   NugetPackageId=1,
                   NugetPackageName="packageName",
                   ApplicationName="applicationName",
                   ApplicationId=1,
                   IsDeleted=false,
                   PackageUrl="packageURl"
               }
            };

            var applicationsJenkinsJob = new List<JenkinsJob> {
                new JenkinsJob{
                   JenkinsJobName="JenkinsJobName"
                }
            };

            var sonarqubeProject = new List<SonarqubeProject> {
                new SonarqubeProject{
                   SonarqubeProjectName="SonarqubeProjectName"
                }
            };

            var svnRepository = new List<SvnRepository> {
                new SvnRepository{
                  ApplicationName="applicationName"
                }
            };

            var applicationDependencies = new List<ApplicationDependency>();
            var databaseDependencies = new List<DatabaseDependency>();
            var externalDependencies = new List<ExternalDependency>();
            var nugetPackageDependencies = new List<NugetPackageDependency>();

            var ApplicationFullModel = new ApplicationFullModel
            {
                Application = application,
                ApplicationNugetPackages = applicationNugetPackage,
                JenkinsJobs = applicationsJenkinsJob,
                SonarqubeProjects = sonarqubeProject,
                SvnRepositories = svnRepository,
                ApplicationDependencies = applicationDependencies,
                ExternalDependencies = externalDependencies,
                DatabaseDependencies = databaseDependencies,
                NugetPackageDependencies = nugetPackageDependencies
            };

            // Act
            var result = factory.CreateNugetApplicationFullModel(application, applicationNugetPackage, applicationsJenkinsJob, sonarqubeProject, svnRepository,
                applicationDependencies, databaseDependencies, externalDependencies, nugetPackageDependencies);

            // Assert
            result.Should().BeEquivalentTo(ApplicationFullModel);
        }

        [Test]
        public void CreateApplicationFullModel_NoConditionN_ReturnApplicationFullModel()
        {
            // Arrange
            var application = new Application
            {
                Status = "Status",
                RedmineProjectUrl = "RedmineProjectUrl"
            };

            var applicationEnvironment = new List<ApplicationEnvironment> {
               new ApplicationEnvironment
               {
                   ApplicationName="ApplicationName",
                   EnvironmentId=1,
                   ApplicationId=1,
                   IsDeleted=false
               }
            };

            var applicationsJenkinsJob = new List<JenkinsJob> {
                new JenkinsJob{
                   JenkinsJobName="JenkinsJobName"
                }
            };

            var sonarqubeProject = new List<SonarqubeProject> {
                new SonarqubeProject{
                   SonarqubeProjectName="SonarqubeProjectName"
                }
            };

            var svnRepository = new List<SvnRepository> {
                new SvnRepository{
                  ApplicationName="applicationName"
                }
            };

            var applicationDependencies = new List<ApplicationDependency>();
            var databaseDependencies = new List<DatabaseDependency>();
            var externalDependencies = new List<ExternalDependency>();
            var nugetPackageDependencies = new List<NugetPackageDependency>();

            var ApplicationFullModel = new ApplicationFullModel
            {
                Application = application,
                ApplicationEnvironments = applicationEnvironment,
                JenkinsJobs = applicationsJenkinsJob,
                SonarqubeProjects = sonarqubeProject,
                SvnRepositories = svnRepository,
                ApplicationDependencies = applicationDependencies,
                ExternalDependencies = externalDependencies,
                DatabaseDependencies = databaseDependencies,
                NugetPackageDependencies = nugetPackageDependencies
            };

            // Act
            var result = factory.CreateApplicationFullModel(application, applicationEnvironment, applicationsJenkinsJob, sonarqubeProject, svnRepository,
                applicationDependencies, databaseDependencies, externalDependencies, nugetPackageDependencies);

            // Assert
            result.Should().BeEquivalentTo(ApplicationFullModel);
        }
    }
}