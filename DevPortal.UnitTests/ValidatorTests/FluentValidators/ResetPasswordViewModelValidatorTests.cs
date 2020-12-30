using DevPortal.Validation.FluentValidators;
using DevPortal.Web.Library.Model;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ResetPasswordViewModelValidatorTests
    {
        #region members & setup

        ResetPasswordViewModelValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ResetPasswordViewModelValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_NewPassword_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.NewPassword, "");
            validator.ShouldHaveValidationErrorFor(x => x.NewPassword, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.NewPassword, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.NewPassword, "12345");
        }

        [Test]
        public void Validate_ConfirmPassword_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, "");
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, "12345");
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            var resetPasswordViewModel = new ResetPasswordViewModel
            {
                NewPassword = "123456",
                ConfirmPassword = "123456",
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.NewPassword, resetPasswordViewModel);
            validator.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword, resetPasswordViewModel);
        }

        #endregion
    }
}