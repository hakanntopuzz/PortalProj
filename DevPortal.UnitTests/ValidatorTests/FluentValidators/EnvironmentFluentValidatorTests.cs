using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class EnvironmentFluentValidatorTests
    {
        #region members & setup

        EnvironmentValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new EnvironmentValidator();
        }

        #endregion

        public string Icerik => "DescriptionDescriptionDescriptionDescription";

        #region invalid conditions

        [Test]
        public void Validate_Name_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, Icerik);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
        }

        #endregion
    }
}