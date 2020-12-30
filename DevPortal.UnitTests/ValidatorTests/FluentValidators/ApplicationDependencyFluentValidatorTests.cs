using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationDependencyFluentValidatorTests
    {
        #region members & setup

        ApplicationDependencyValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationDependencyValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_Description_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Description,
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa" +
                "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        [Test]
        public void Validate_ApplicationGroupId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationGroupId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.ApplicationGroupId, 0);
        }

        [Test]
        public void Validate_DependedApplicationId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.DependedApplicationId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.DependedApplicationId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, "Description");
            validator.ShouldNotHaveValidationErrorFor(x => x.ApplicationGroupId, 1);
            validator.ShouldNotHaveValidationErrorFor(x => x.DependentApplicationId, 1);
        }

        #endregion
    }
}