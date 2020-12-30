using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ForgotPasswordViewModelValidatorTests
    {
        #region members & setup

        ForgotPasswordViewModelValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ForgotPasswordViewModelValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void ValidateEmailAddress_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "");
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "test@example");
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "test");
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, "test@test.com");
        }

        #endregion
    }
}