using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class DatabaseValidatorTests
    {
        #region members & setup

        DatabaseValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new DatabaseValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_Name_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, "");
            validator.ShouldHaveValidationErrorFor(x => x.Name, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.Name, string.Empty);
        }

        [Test]
        public void Validate_DatabaseTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseTypeId, 0);
        }

        [Test]
        public void Validate_DatabaseGroupId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.DatabaseGroupId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueLengthInRange_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            validator.ShouldNotHaveValidationErrorFor(x => x.DatabaseTypeId, 1000);
            validator.ShouldNotHaveValidationErrorFor(x => x.DatabaseGroupId, 1000);
        }

        #endregion
    }
}