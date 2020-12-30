using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationValidatorTests
    {
        #region members & setup

        ApplicationValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_Name_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, "");
            validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
        }

        [Test]
        public void Validate_ApplicationGroupId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationGroupId, 0);
        }

        [Test]
        public void Validate_StatusId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.StatusId, 0);
        }

        [Test]
        public void Validate_ApplicationTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationTypeId, 0);
        }

        [Test]
        public void Validate_RedmineProjectName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.RedmineProjectName, "");
            validator.ShouldHaveValidationErrorFor(x => x.RedmineProjectName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.RedmineProjectName, string.Empty);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueLengthInRange_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "name");
            validator.ShouldNotHaveValidationErrorFor(x => x.ApplicationGroupId, 1000);
            validator.ShouldNotHaveValidationErrorFor(x => x.StatusId, 1000);
            validator.ShouldNotHaveValidationErrorFor(x => x.ApplicationTypeId, 1000);
            validator.ShouldNotHaveValidationErrorFor(x => x.RedmineProjectName, "redmineProjectName");
        }

        #endregion
    }
}