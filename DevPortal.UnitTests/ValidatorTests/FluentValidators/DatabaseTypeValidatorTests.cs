using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseTypeValidatorTests
    {
        #region members & setup

        DatabaseTypeValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new DatabaseTypeValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_Name_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, "");
            validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Name, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueLengthInRange_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        #endregion
    }
}