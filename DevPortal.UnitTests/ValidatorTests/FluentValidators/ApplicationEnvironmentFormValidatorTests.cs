using AB.Framework.UnitTests;
using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationEnvironmentFormValidatorTests
    {
        #region members & setup

        ApplicationEnvironmentFormValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationEnvironmentFormValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_PhysicalPath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.PhysicalPath, "");
            validator.ShouldHaveValidationErrorFor(x => x.PhysicalPath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.PhysicalPath, string.Empty);
        }

        [Test]
        public void Validate_LogFilePath_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.LogFilePath, "");
            validator.ShouldHaveValidationErrorFor(x => x.LogFilePath, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.LogFilePath, string.Empty);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.PhysicalPath, "physical path");
            validator.ShouldNotHaveValidationErrorFor(x => x.LogFilePath, "log file path");
        }

        #endregion
    }
}