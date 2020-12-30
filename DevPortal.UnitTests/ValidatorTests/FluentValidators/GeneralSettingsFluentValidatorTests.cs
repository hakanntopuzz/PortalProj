using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class GeneralSettingsFluentValidatorTests
    {
        #region members & setup

        GeneralSettingsValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new GeneralSettingsValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_RedmineUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.RedmineUrl, "");
            validator.ShouldHaveValidationErrorFor(x => x.RedmineUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.RedmineUrl, string.Empty);
        }

        [Test]
        public void Validate_SvnUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SvnUrl, "");
            validator.ShouldHaveValidationErrorFor(x => x.SvnUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.SvnUrl, string.Empty);
        }

        [Test]
        public void Validate_JenkinsUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsUrl, "");
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsUrl, string.Empty);
        }

        [Test]
        public void Validate_SonarQubeUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SonarQubeUrl, "");
            validator.ShouldHaveValidationErrorFor(x => x.SonarQubeUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.SonarQubeUrl, string.Empty);
        }

        [Test]
        public void Validate_NugetUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.NugetUrl, "");
            validator.ShouldHaveValidationErrorFor(x => x.NugetUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.NugetUrl, string.Empty);
        }

        [Test]
        public void Validate_NugetApiKey_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.NugetApiKey, "");
            validator.ShouldHaveValidationErrorFor(x => x.NugetApiKey, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.NugetApiKey, string.Empty);
        }

        [Test]
        public void Validate_NugetPackageArchiveFolderPath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageArchiveFolderPath, "");
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageArchiveFolderPath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageArchiveFolderPath, string.Empty);
        }

        [Test]
        public void Validate_ApplicationVersionPackageProdFolderPath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackageProdFolderPath, "");
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackageProdFolderPath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackageProdFolderPath, string.Empty);
        }

        [Test]
        public void Validate_ApplicationVersionPackagePreProdFolderPath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackagePreProdFolderPath, "");
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackagePreProdFolderPath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackagePreProdFolderPath, string.Empty);
        }

        [Test]
        public void Validate_DatabaseDeploymentPackageProdFolderPath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackageProdFolderPath, "");
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackageProdFolderPath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackageProdFolderPath, string.Empty);
        }

        [Test]
        public void Validate_DatabaseDeploymentPackagePreProdFolderPath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackagePreProdFolderPath, "");
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackagePreProdFolderPath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackagePreProdFolderPath, string.Empty);
        }

        [Test]
        public void Validate_MaximumCharacterExceeded_HaveError()
        {
            var twoHundredAndFiftyOneCharacterString = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567891";

            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackageProdFolderPath, twoHundredAndFiftyOneCharacterString);
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationVersionPackagePreProdFolderPath, twoHundredAndFiftyOneCharacterString);
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackageProdFolderPath, twoHundredAndFiftyOneCharacterString);
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseDeploymentPackagePreProdFolderPath, twoHundredAndFiftyOneCharacterString);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.RedmineUrl, "redmine");
            validator.ShouldNotHaveValidationErrorFor(x => x.SvnUrl, "svn");
            validator.ShouldNotHaveValidationErrorFor(x => x.JenkinsUrl, "jenkins");
            validator.ShouldNotHaveValidationErrorFor(x => x.SonarQubeUrl, "sonarqube");
            validator.ShouldNotHaveValidationErrorFor(x => x.NugetUrl, "nuget");
            validator.ShouldNotHaveValidationErrorFor(x => x.NugetApiKey, "nuget api key");
            validator.ShouldNotHaveValidationErrorFor(x => x.NugetPackageArchiveFolderPath, "nuget arşiv");
            validator.ShouldNotHaveValidationErrorFor(x => x.ApplicationVersionPackageProdFolderPath, "başarılı versiyon paketleri prod");
            validator.ShouldNotHaveValidationErrorFor(x => x.ApplicationVersionPackagePreProdFolderPath, "başarılı versiyon paketleri preprod");
            validator.ShouldNotHaveValidationErrorFor(x => x.DatabaseDeploymentPackageProdFolderPath, "veritabanı aktarım prod");
            validator.ShouldNotHaveValidationErrorFor(x => x.DatabaseDeploymentPackagePreProdFolderPath, "veritabanı aktarım preprod");
        }

        #endregion
    }
}