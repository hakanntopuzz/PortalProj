using DevPortal.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class UserFluentValidationTests
    {
        #region members & setup

        UserValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new UserValidator();
        }

        #endregion

        public string Icerik => "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        #region invalid conditions

        [Test]
        public void Validate_FirstName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, "");
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.FirstName, Icerik);
        }

        [Test]
        public void Validate_LastName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.LastName, "");
            validator.ShouldHaveValidationErrorFor(x => x.LastName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.LastName, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.LastName, Icerik);
        }

        [Test]
        public void Validate_SvnUserName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SvnUserName, Icerik);
        }

        [Test]
        public void Validate_EmailAddress_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "");
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, "EmailAddress");
            validator.ShouldHaveValidationErrorFor(x => x.EmailAddress, Icerik + Icerik);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.FirstName, "FirstName");
            validator.ShouldNotHaveValidationErrorFor(x => x.LastName, "LastName");
            validator.ShouldNotHaveValidationErrorFor(x => x.SvnUserName, "SvnUserName");
            validator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, "EmailAddress@test.com");
            validator.ShouldNotHaveValidationErrorFor(x => x.UserTypeId, 1);
            validator.ShouldNotHaveValidationErrorFor(x => x.UserStatusId, 1);
        }

        #endregion
    }
}