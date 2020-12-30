using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationSonarQubeProjectFluentValidatorTests
    {
        #region members & setup

        ApplicationSonarQubeProjectValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationSonarQubeProjectValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_SonarqubeProjectName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SonarqubeProjectName, "");
            validator.ShouldHaveValidationErrorFor(x => x.SonarqubeProjectName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.SonarqubeProjectName, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.SonarqubeProjectName, "SonarqubeProjectNameSonarqubeProjectNameSonarqubeProjectNameSonarqubeProjectName");
        }

        [Test]
        public void Validate_SonarqubeProjectTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SonarqubeProjectTypeId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.SonarqubeProjectTypeId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.SonarqubeProjectName, "SonarqubeProjectName");
            validator.ShouldNotHaveValidationErrorFor(x => x.SonarqubeProjectTypeId, 1);
        }

        #endregion
    }
}