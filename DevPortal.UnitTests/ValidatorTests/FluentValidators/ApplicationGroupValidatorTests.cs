using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationGroupValidatorTests
    {
        #region members & setup

        ApplicationGroupValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationGroupValidator();
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
        public void Validate_StatusId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.StatusId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueLengthInRange_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "name");
            validator.ShouldNotHaveValidationErrorFor(x => x.StatusId, 1000);
        }

        #endregion
    }
}