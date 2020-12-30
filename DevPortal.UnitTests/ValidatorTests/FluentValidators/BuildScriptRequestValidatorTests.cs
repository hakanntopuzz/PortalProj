using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class BuildScriptRequestValidatorTests
    {
        #region members & setup

        BuildScriptRequestValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new BuildScriptRequestValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_ApplicationId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationId, 0);
        }

        [Test]
        public void Validate_BuildTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.BuildTypeId, 0);
        }

        [Test]
        public void Validate_JobTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.JobTypeId, 0);
        }

        [Test]
        public void Validate_EnvironmentId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.EnvironmentId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueLengthInRange_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.ApplicationId, 1);
            validator.ShouldNotHaveValidationErrorFor(x => x.BuildTypeId, 1);
            validator.ShouldNotHaveValidationErrorFor(x => x.JobTypeId, 1);
            validator.ShouldNotHaveValidationErrorFor(x => x.EnvironmentId, 1);
        }

        #endregion
    }
}