using DevPortal.Validation.FluentValidators;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace DevPortal.UnitTests.ValidatorTests.FluentValidators
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnRepositoryFluentValidationTests
    {
        #region members & setup

        SvnRepositoryValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new SvnRepositoryValidator();
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
            validator.ShouldHaveValidationErrorFor(x => x.Name, Icerik);
        }

        [Test]
        public void Validate_SvnRepositoryTypeId_ValueEmpty_HaveError()
        {
            validator.ShouldHaveValidationErrorFor(x => x.SvnRepositoryTypeId, -1);
            validator.ShouldHaveValidationErrorFor(x => x.SvnRepositoryTypeId, 0);
        }

        #endregion

        #region valid conditions

        [Test]
        public void Validate_ValueNotEmpty_DoNotHaveError()
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, "Name");
            validator.ShouldNotHaveValidationErrorFor(x => x.SvnRepositoryTypeId, 1);
        }

        #endregion
    }

    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SvnRepositoryFolderFluentValidationTests
    {
        #region members & setup

        SvnRepositoryFolderValidator validator;

        [SetUp]
        public void Initialize()
        {
            validator = new SvnRepositoryFolderValidator();
        }

        #endregion

        #region invalid conditions

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("a")] // min. 2 karakter sınırı aşılamadığında
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // maks. 30 karakter sınırı aşıldığında
        [TestCase("ABC")] // büyük harf içeremez
        [TestCase("şinasi")] // türkçe karakterler içeremez
        [TestCase("test*?.,;:{}()[]\"'^+$%½&/")] // sembol ve noktalama işareti içeremez
        public void Validate_InvalidFolderNamesInTestCases_HaveError(string folderName)
        {
            validator.ShouldHaveValidationErrorFor(x => x.Name, folderName);
        }

        #endregion

        #region valid conditions

        [Test]
        [TestCase("ab")] // min. 2 karakter olabilir
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")] // maks. 30 karakter olabilir
        [TestCase("abc")] // küçük harf içerebilir
        [TestCase("abc009")] // rakam içerebilir
        [TestCase("abc-_")] // tire ve alt-tire içerebilir
        public void Validate_ValidFolderNamesInTestCases_DoNotHaveError(string folderName)
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.Name, folderName);
        }

        #endregion
    }
}