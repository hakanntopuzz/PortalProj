using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LoginViewModelValidatorTests
    {
        #region members & setup

        LoginViewModelValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new LoginViewModelValidator();
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

        [Test]
        public void Validate_Password_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Password, "");
            validator.ShouldHaveValidationErrorFor(x => x.Password, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Password, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Password, "12345");
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, "test@test.com");
            validator.ShouldNotHaveValidationErrorFor(x => x.Password, "123456");
        }

        #endregion
    }
}