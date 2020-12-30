using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FavouritePageFluentValidatorTests
    {
        #region members & setup

        FavouritePageValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new FavouritePageValidator();
        }

        #endregion

        public string Icerik => "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

        #region invalid conditions

        [Test]
        public void Validate_PageName_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.PageName, "");
            validator.ShouldHaveValidationErrorFor(x => x.PageName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.PageName, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.PageName, Icerik);
        }

        [Test]
        public void Validate_PageUrl_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.PageUrl, "");
            validator.ShouldHaveValidationErrorFor(x => x.PageUrl, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.PageUrl, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.PageUrl, Icerik + Icerik + Icerik + Icerik);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.PageName, "NamePageName");
            validator.ShouldNotHaveValidationErrorFor(x => x.PageUrl, "PageUrl");
        }

        #endregion
    }
}