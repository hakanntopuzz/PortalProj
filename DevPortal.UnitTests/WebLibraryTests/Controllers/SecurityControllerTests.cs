using AB.Framework.UnitTests;
using DevPortal.Cryptography.Business.Abstract;
using DevPortal.Cryptography.Model;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Controllers;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;

namespace DevPortal.UnitTests.WebLibraryTests.Controllers
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SecurityControllerTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IUserSessionService> userSessionService;

        StrictMock<ICryptographyService> cryptographyService;

        StrictMock<ISecurityService> securityService;

        StrictMock<ISecurityViewModelFactory> securityViewModelFactory;

        SecurityController controller;

        [SetUp]
        public void Initialize()
        {
            userSessionService = new StrictMock<IUserSessionService>();
            cryptographyService = new StrictMock<ICryptographyService>();
            securityService = new StrictMock<ISecurityService>();
            securityViewModelFactory = new StrictMock<ISecurityViewModelFactory>();

            controller = new SecurityController(
                userSessionService.Object,
                cryptographyService.Object,
                securityService.Object,
                securityViewModelFactory.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            cryptographyService.VerifyAll();
            securityViewModelFactory.VerifyAll();
            securityService.VerifyAll();
        }

        #endregion

        #region single cryptography

        [Test]
        public void SingleCryptography_CryptoViewModelIsNull_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var cryptoViewModel = new CryptoViewModel();
            securityViewModelFactory.Setup(x => x.CreateCryptoViewModel()).Returns(cryptoViewModel);

            //Act
            var result = controller.SingleCryptography();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.SingleCryptography).ModelAs<CryptoViewModel>();
        }

        [Test]
        public void SingleCryptography_IsValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertedText = "";
            var convertibleText = "";
            var cryptoViewModel = new CryptoViewModel { CryptographyType = CryptographyTypes.Aes, ConvertType = ConvertTypes.Decrypt, ConvertibleText = convertibleText };
            cryptographyService.Setup(x => x.GetSingleCryptography(cryptoViewModel.CryptographyType, cryptoViewModel.ConvertType, cryptoViewModel.ConvertibleText)).Returns(convertedText);

            //Act
            var result = controller.SingleCryptography(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.SingleCryptography).ModelAs<CryptoViewModel>();
            resultModel.ConvertedText.Should().Be(convertedText);
            resultModel.ConvertibleText.Should().Be(convertibleText);
            resultModel.CryptographyType.Should().Be(cryptoViewModel.CryptographyType);
            resultModel.ConvertType.Should().Be(cryptoViewModel.ConvertType);
        }

        [Test]
        public void SingleCryptography_IsNotValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertibleText = "";
            controller.ModelState.AddModelError("", "");
            var cryptoViewModel = new CryptoViewModel { CryptographyType = CryptographyTypes.Aes, ConvertType = ConvertTypes.Decrypt, ConvertibleText = convertibleText };

            //Act
            var result = controller.SingleCryptography(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.SingleCryptography).ModelAs<CryptoViewModel>();
            resultModel.ConvertibleText.Should().Be(convertibleText);
            resultModel.CryptographyType.Should().Be(cryptoViewModel.CryptographyType);
            resultModel.ConvertType.Should().Be(cryptoViewModel.ConvertType);
        }

        #endregion

        #region multiple encrypt with ees256

        [Test]
        public void MultipleEncryptWithAes256_NoCondition_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            CryptoViewModel cryptoViewModel = new CryptoViewModel();
            securityViewModelFactory.Setup(x => x.CreateCryptoViewModel()).Returns(cryptoViewModel);

            //Act
            var result = controller.MultipleEncryptWithAes256();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleEncryptWithAes256).ModelAs<CryptoViewModel>();
        }

        [Test]
        public void MultipleEncryptWithAes256_ModelIsValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertedText = "";
            CryptoViewModel cryptoViewModel = new CryptoViewModel { ConvertedText = convertedText };
            cryptographyService.Setup(x => x.MultipleEncryptedWithAes256(cryptoViewModel.ConvertibleText)).Returns(convertedText);

            //Act
            var result = controller.MultipleEncryptWithAes256(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.MultipleEncryptWithAes256).ModelAs<CryptoViewModel>();
            resultModel.ConvertedText.Should().Be(convertedText);
        }

        [Test]
        public void MultipleEncryptWithAes256_ModelIsNotValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertedText = "";
            controller.ModelState.AddModelError("", "");
            CryptoViewModel cryptoViewModel = new CryptoViewModel { ConvertedText = convertedText };

            //Act
            var result = controller.MultipleEncryptWithAes256(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.MultipleEncryptWithAes256).ModelAs<CryptoViewModel>();
            resultModel.ConvertedText.Should().Be(convertedText);
        }

        #endregion

        #region multiple decrypt with ees256

        [Test]
        public void MultipleDecryptWithAes256_NoCondition_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            CryptoViewModel cryptoViewModel = new CryptoViewModel();
            securityViewModelFactory.Setup(x => x.CreateCryptoViewModel()).Returns(cryptoViewModel);

            //Act
            var result = controller.MultipleDecryptWithAes256();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleDecryptWithAes256).ModelAs<CryptoViewModel>();
        }

        [Test]
        public void MultipleDecryptWithAes256_ModelIsValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertibleText = " ";
            var convertedText = " ";
            CryptoViewModel cryptoViewModel = new CryptoViewModel { ConvertedText = convertedText, ConvertibleText = convertibleText };
            cryptographyService.Setup(x => x.MultipleDecryptedWithAes256(convertibleText)).Returns(convertedText);

            //Act
            var result = controller.MultipleDecryptWithAes256(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.MultipleDecryptWithAes256).ModelAs<CryptoViewModel>();
            resultModel.ConvertibleText.Should().Be(convertibleText);
            resultModel.ConvertedText.Should().Be(convertedText);
        }

        [Test]
        public void MultipleDecryptWithAes256_ModelIsNotValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            controller.ModelState.AddModelError("", "");
            CryptoViewModel cryptoViewModel = new CryptoViewModel { };

            //Act
            var result = controller.MultipleDecryptWithAes256(cryptoViewModel);

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleDecryptWithAes256).ModelAs<CryptoViewModel>();
        }

        #endregion

        #region multiple encrypt with 3Des

        [Test]
        public void MultipleEncryptWithTripleDes_NoCondition_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            CryptoViewModel cryptoViewModel = new CryptoViewModel();
            securityViewModelFactory.Setup(x => x.CreateCryptoViewModel()).Returns(cryptoViewModel);

            //Act
            var result = controller.MultipleEncryptWithTripleDes();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleEncryptWithTripleDes).ModelAs<CryptoViewModel>();
        }

        [Test]
        public void MultipleEncryptWithTripleDes_ModelIsValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertibleText = " ";
            var convertedText = " ";
            CryptoViewModel cryptoViewModel = new CryptoViewModel { ConvertedText = convertedText, ConvertibleText = convertibleText };
            cryptographyService.Setup(x => x.MultipleEncryptedWithTripleDes(convertibleText)).Returns(convertedText);

            //Act
            var result = controller.MultipleEncryptWithTripleDes(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.MultipleEncryptWithTripleDes).ModelAs<CryptoViewModel>();
            resultModel.ConvertibleText.Should().Be(convertibleText);
            resultModel.ConvertedText.Should().Be(convertedText);
        }

        [Test]
        public void MultipleEncryptWithTripleDes_ModelIsNotValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            controller.ModelState.AddModelError("", "");
            CryptoViewModel cryptoViewModel = new CryptoViewModel { };

            //Act
            var result = controller.MultipleEncryptWithTripleDes(cryptoViewModel);

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleEncryptWithTripleDes).ModelAs<CryptoViewModel>();
        }

        #endregion

        #region multiple decrypt with 3Des

        [Test]
        public void MultipleDecryptWithTripleDes_NoCondition_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            CryptoViewModel cryptoViewModel = new CryptoViewModel();
            securityViewModelFactory.Setup(x => x.CreateCryptoViewModel()).Returns(cryptoViewModel);

            //Act
            var result = controller.MultipleDecryptWithTripleDes();

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleDecryptWithTripleDes).ModelAs<CryptoViewModel>();
        }

        [Test]
        public void MultipleDecryptWithTripleDes_ModelIsValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertibleText = " ";
            var convertedText = " ";
            CryptoViewModel cryptoViewModel = new CryptoViewModel { ConvertedText = convertedText, ConvertibleText = convertibleText };
            cryptographyService.Setup(x => x.MultipleDecryptedWithTripleDes(convertibleText)).Returns(convertedText);

            //Act
            var result = controller.MultipleDecryptWithTripleDes(cryptoViewModel);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.MultipleDecryptWithTripleDes).ModelAs<CryptoViewModel>();
            resultModel.ConvertibleText.Should().Be(convertibleText);
            resultModel.ConvertedText.Should().Be(convertedText);
        }

        [Test]
        public void MultipleDecryptWithTripleDes_ModelIsNotValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            controller.ModelState.AddModelError("", "");
            CryptoViewModel cryptoViewModel = new CryptoViewModel { };

            //Act
            var result = controller.MultipleDecryptWithTripleDes(cryptoViewModel);

            // Assert
            result.Should().BeViewResult().WithViewName(ViewNames.MultipleDecryptWithTripleDes).ModelAs<CryptoViewModel>();
        }

        #endregion

        #region aes

        [Test]
        public void Aes_CryptoViewModelIsNull_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var model = new AesViewModel();
            string convertedText = null;
            securityViewModelFactory.Setup(x => x.CreateAesViewModel(convertedText)).Returns(model);

            //Act
            var result = controller.Aes();

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Aes).ModelAs<AesViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Aes_IsValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            var convertedText = "";
            var convertibleText = "";
            var model = new AesViewModel
            {
                CryptographyType = CryptographyTypes.Aes,
                ConvertType = ConvertTypes.Decrypt,
                ConvertibleText = convertibleText,
                SiteId = 5
            };

            cryptographyService.Setup(x => x.DoAesAction(model.ConvertType, model.ConvertibleText, model.SiteId)).Returns(convertedText);
            securityViewModelFactory.Setup(x => x.CreateAesViewModel(convertedText)).Returns(model);

            //Act
            var result = controller.Aes(model);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Aes).ModelAs<AesViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Aes_IsNotValid_ReturnViewNamesAndCryptoViewModel()
        {
            // Arrange
            string convertedText = null;
            var convertibleText = "";
            controller.ModelState.AddModelError("", "");
            var model = new AesViewModel
            {
                CryptographyType = CryptographyTypes.Aes,
                ConvertType = ConvertTypes.Decrypt,
                ConvertibleText = convertibleText
            };

            securityViewModelFactory.Setup(x => x.CreateAesViewModel(convertedText)).Returns(model);

            //Act
            var result = controller.Aes(model);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Aes).ModelAs<AesViewModel>();

            resultModel.Should().BeEquivalentTo(model);
        }

        #endregion

        #region hash

        [Test]
        public void Hash_HashViewModelIsNull_ReturnViewNamesAndHashViewModel()
        {
            // Arrange
            var model = new HashViewModel();
            string hashedText = null;
            securityViewModelFactory.Setup(x => x.CreateHashViewModel(hashedText)).Returns(model);

            //Act
            var result = controller.Hash();

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Hash).ModelAs<HashViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Hash_IsValid_ReturnViewNamesAndHashViewModel()
        {
            // Arrange
            var hashedText = "";
            var hashToText = "";
            var model = new HashViewModel
            {
                HashType = HashTypes.SHA1,
                HashToText = hashToText
            };

            securityService.Setup(x => x.CreateHash(model.HashToText, model.HashType)).Returns(hashedText);

            //Act
            var result = controller.Hash(model);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Hash).ModelAs<HashViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Hash_IsNotValid_ReturnViewNamesAndHashViewModel()
        {
            // Arrange
            var hashToText = "";
            controller.ModelState.AddModelError("", "");
            var model = new HashViewModel
            {
                HashType = HashTypes.SHA1,
                HashToText = hashToText
            };

            //Act
            var result = controller.Hash(model);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Hash).ModelAs<HashViewModel>();

            resultModel.Should().BeEquivalentTo(model);
        }

        #endregion

        #region guid

        [Test]
        public void Guid_GuidViewModelIsNull_ReturnViewNamesAndGuidViewModel()
        {
            // Arrange
            var model = new GuidViewModel();
            string guid = null;
            securityViewModelFactory.Setup(x => x.CreateGuidViewModel(guid)).Returns(model);

            //Act
            var result = controller.Guid();

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Guid).ModelAs<GuidViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void GenerateGuid_GuidViewModelIsNotNull_ReturnViewNamesAndGuidViewModel()
        {
            // Arrange
            string guid = "guid";
            var model = new GuidViewModel
            {
                Guid = guid
            };

            securityService.Setup(x => x.GenerateGuid()).Returns(guid);

            securityViewModelFactory.Setup(x => x.CreateGuidViewModel(guid)).Returns(model);

            //Act
            var result = controller.GenerateGuid();

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Guid).ModelAs<GuidViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        #endregion

        #region password

        [Test]
        public void Password_PasswordViewModelIsNull_ReturnViewNamesAndPasswordViewModel()
        {
            // Arrange
            string password = null;
            var model = new GeneratePasswordViewModel
            {
                Password = password
            };

            securityViewModelFactory.Setup(x => x.CreateGeneratePasswordViewModel(password)).Returns(model);

            //Act
            var result = controller.Password();

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Password).ModelAs<GeneratePasswordViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        [Test]
        public void Password_PasswordViewModelIsNotNull_ReturnViewNamesAndPasswordViewModel()
        {
            // Arrange
            string password = "1234Ha4578!";
            var passwordModel = new PasswordModel
            {
                IncludeNumeric = true,
                IncludeSpecialCharacters = true,
                IncludeLowerCase = true,
                IncludeUpperCase = true
            };

            var model = new GeneratePasswordViewModel
            {
                Password = password,
                PasswordModel = passwordModel
            };

            securityService.Setup(x => x.GeneratePassword(model.PasswordModel)).Returns(password);

            securityViewModelFactory.Setup(x => x.CreateGeneratePasswordViewModel(password)).Returns(model);

            //Act
            var result = controller.Password(model);

            // Assert
            var resultModel = result.Should().BeViewResult().WithViewName(ViewNames.Password).ModelAs<GeneratePasswordViewModel>();
            resultModel.Should().BeEquivalentTo(model);
        }

        #endregion
    }
}