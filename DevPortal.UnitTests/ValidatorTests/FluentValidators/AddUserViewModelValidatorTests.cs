using DevPortal.Validation.FluentValidators;
using DevPortal.Web.Library.Model;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class AddUserViewModelValidatorTests
    {
        #region members & setup

        AddUserViewModelValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new AddUserViewModelValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_Password_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Password, "");
            validator.ShouldHaveValidationErrorFor(x => x.Password, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Password, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.Password, "12345");
            validator.ShouldHaveValidationErrorFor(x => x.Password, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void Validate_ConfirmPassword_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, "");
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, "12345");
            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            var addUserViewModel = new AddUserViewModel
            {
                Password = "123456",
                ConfirmPassword = "123456",
            };

            validator.ShouldNotHaveValidationErrorFor(x => x.Password, addUserViewModel);
            validator.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword, addUserViewModel);
        }

        #endregion
    }
}