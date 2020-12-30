using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseDependencyFluentValidatorTests
    {
        #region members & setup

        DatabaseDependencyValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new DatabaseDependencyValidator();
        }

        #endregion

        public string Icerik => "DescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescription";

        #region invalid conditions

        [Test]
        public void Validate_Description_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Description, Icerik + Icerik + Icerik + Icerik + Icerik);
        }

        [Test]
        public void Validate_DatabaseId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Description, "Description");
            validator.ShouldNotHaveValidationErrorFor(x => x.DatabaseId, 1);
        }

        #endregion
    }
}