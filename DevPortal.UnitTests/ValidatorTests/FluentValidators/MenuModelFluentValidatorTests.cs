using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    class MenuModelFluentValidatorTests
    {
        #region members & setup

        MenuModelValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new MenuModelValidator();
        }

        #endregion

        public string Icerik => "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        #region invalid conditions

        [Test]
        public void Validate_Name_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, "");
            validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
        }

        [Test]
        public void Validate_Link_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Link, Icerik + Icerik);
        }

        [Test]
        public void Validate_Description_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Description, Icerik + Icerik);
        }

        [Test]
        public void Validate_Icon_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Icon, Icerik);
        }

        [Test]
        public void Validate_NugetUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.MenuGroupId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.MenuGroupId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
            validator.ShouldNotHaveValidationErrorFor(x => x.Link, "Link");
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, "Description");
            validator.ShouldNotHaveValidationErrorFor(x => x.Icon, "Icon");
            validator.ShouldNotHaveValidationErrorFor(x => x.MenuGroupId, 1);
        }

        #endregion
    }
}