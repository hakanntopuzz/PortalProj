using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ExternalDependencyFluentValidatorTests
    {
        #region members & setup

        ExternalDependencyValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ExternalDependencyValidator();
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
            validator.ShouldHaveValidationErrorFor(x => x.Name, Icerik + Icerik);
        }

        [Test]
        public void Validate_Description_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Description, Icerik + Icerik + Icerik + Icerik);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, "Description");
        }

        #endregion
    }
}