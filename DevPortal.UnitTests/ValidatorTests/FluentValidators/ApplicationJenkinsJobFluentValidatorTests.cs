using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationJenkinsJobFluentValidatorTests
    {
        #region members & setup

        ApplicationJenkinsJobValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationJenkinsJobValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_JenkinsJobName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsJobName, "");
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsJobName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsJobName, string.Empty);
        }

        [Test]
        public void Validate_JenkinsJobTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsJobTypeId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.JenkinsJobTypeId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.JenkinsJobName, "JenkinsJobName");
            validator.ShouldNotHaveValidationErrorFor(x => x.JenkinsJobTypeId, 1);
        }

        #endregion
    }
}