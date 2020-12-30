using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class HashViewModelValidatorTests
    {
        #region members & setup

        HashViewModelValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new HashViewModelValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_HashToText_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.HashToText, "");
            validator.ShouldHaveValidationErrorFor(x => x.HashToText, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.HashToText, string.Empty);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.HashToText, "123456");
        }

        #endregion
    }
}