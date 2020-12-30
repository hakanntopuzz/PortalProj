using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ApplicationNugetPackageValidatorTests
    {
        #region members & setup

        ApplicationNugetPackageValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new ApplicationNugetPackageValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        public void Validate_Name_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageName, "");
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageName, null as string);
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageName, string.Empty);
            validator.ShouldHaveValidationErrorFor(x => x.NugetPackageName, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        #endregion
    }
}