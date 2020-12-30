using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.Security.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Cryptography.Business.Abstract.Wrappers;
using DevPortal.Cryptography.Business.Services;
using DevPortal.Cryptography.Model;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DevPortal.UnitTests.CryptoManagerTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SecurityServiceTests : BaseTestFixture
    {
        #region members & setup

        SecurityService service;

        StrictMock<IAesService> aesService;

        StrictMock<ISha1Service> sha1Service;

        StrictMock<ISha512Service> sha512Service;

        StrictMock<ILoggingService> loggingService;

        StrictMock<IPasswordGeneratorWrapper> generatePasswordWrapper;

        StrictMock<IGuidService> guidService;

        [SetUp]
        public void Initialize()
        {
            aesService = new StrictMock<IAesService>();
            sha1Service = new StrictMock<ISha1Service>();
            sha512Service = new StrictMock<ISha512Service>();
            loggingService = new StrictMock<ILoggingService>();
            generatePasswordWrapper = new StrictMock<IPasswordGeneratorWrapper>();
            guidService = new StrictMock<IGuidService>();

            service = new SecurityService(
                aesService.Object,
                sha1Service.Object,
                sha512Service.Object,
                loggingService.Object,
                generatePasswordWrapper.Object,
                guidService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            aesService.VerifyAll();
            sha1Service.VerifyAll();
            sha512Service.VerifyAll();
            loggingService.VerifyAll();
            generatePasswordWrapper.VerifyAll();
            guidService.VerifyAll();
        }

        #endregion

        #region encrypt

        [Test]
        public void Encrypt_NoCondition_ReturnEncryptedText()
        {
            // Arrange

            const string text = "sample-text";
            var passPhrase = "125CC33DAC384373ADB841121F985304";
            var expectedResult = "2C5B3F4AB53778B0D6824B3996AC69C2";
            var siteId = 1;

            aesService.Setup(x => x.Encrypt(text, passPhrase)).Returns(expectedResult);

            // Act
            var result = service.Encrypt(text, siteId);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public void Decrypt_NoCondition_ReturnDecryptedText()
        {
            // Arrange

            const string text = "2C5B3F4AB53778B0D6824B3996AC69C2";
            var passPhrase = "125CC33DAC384373ADB841121F985304";
            var expectedResult = "sample-text";
            var siteId = 1;

            aesService.Setup(x => x.Decrypt(text, passPhrase)).Returns(expectedResult);

            // Act
            var result = service.Decrypt(text, siteId);

            // Assert
            result.Should().Be(expectedResult);
        }

        #endregion

        #region create hash

        [Test]
        public void CreateHash_Fails_ReturnHash()
        {
            // Arrange
            var text = "test";
            var hashType = HashTypes.SHA512;

            var methodName = "SecurityService.CreateHash";
            var message = $"{hashType.ToString()} hashleme işlemi sırasında bir hata oluştu.";
            var exception = new Exception();

            sha512Service.Setup(x => x.Hash(text)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, message, exception));

            // Act
            var result = service.CreateHash(text, hashType);

            // Assert
            result.Should().Be(message);
        }

        [Test]
        public void CreateHash_HashTypeSha1_ReturnHash()
        {
            // Arrange
            var text = "test";
            var hashedText = "hashed-text";
            var hashType = HashTypes.SHA1;

            sha1Service.Setup(x => x.Hash(text)).Returns(hashedText);

            // Act
            var result = service.CreateHash(text, hashType);

            // Assert
            result.Should().Be(hashedText);
        }

        [Test]
        public void CreateHash_HashTypeSha512_ReturnHash()
        {
            // Arrange
            var text = "test";
            var hashedText = "hashed-text";
            var hashType = HashTypes.SHA512;

            sha512Service.Setup(x => x.Hash(text)).Returns(hashedText);

            // Act
            var result = service.CreateHash(text, hashType);

            // Assert
            result.Should().Be(hashedText);
        }

        #endregion

        #region check hash

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CheckHash_CheckParametersInTestCasesSha1_ReturnCheckResult(bool checkResult)
        {
            // Arrange
            var text = "test";
            var hashedText = "hashed-text";
            var hashType = HashTypes.SHA1;

            sha1Service.Setup(x => x.CheckHash(hashedText, text)).Returns(checkResult);

            // Act
            var result = service.CheckHash(hashedText, text, hashType);

            // Assert
            result.Should().Be(checkResult);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CheckHash_CheckParametersInTestCasesSha512_ReturnCheckResult(bool checkResult)
        {
            // Arrange
            var text = "test";
            var hashedText = "hashed-text";
            var hashType = HashTypes.SHA512;

            sha512Service.Setup(x => x.CheckHash(hashedText, text)).Returns(checkResult);

            // Act
            var result = service.CheckHash(hashedText, text, hashType);

            // Assert
            result.Should().Be(checkResult);
        }

        #endregion

        #region get pass phrase

        [Test]
        public void GetPassPhrase_SiteIdOne_ReturnPassPhrase()
        {
            //Arrange
            var siteId = 1;
            var expectedResult = "125CC33DAC384373ADB841121F985304";

            //Act
            var result = SecurityService.GetPassPhrase(siteId);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public void GetPassPhrase_SiteIdTwo_ReturnPassPhrase()
        {
            //Arrange
            var siteId = 2;
            var expectedResult = "05F763CB9FDB7847117BF496BE954E19";

            //Act
            var result = SecurityService.GetPassPhrase(siteId);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Test]
        public void GetPassPhrase_NoCondition_ReturnPassPhrase()
        {
            //Arrange
            var siteId = 3;
            var expectedResult = string.Empty;

            //Act
            var result = SecurityService.GetPassPhrase(siteId);

            //Assert
            result.Should().Be(expectedResult);
        }

        #endregion

        #region generate guid

        [Test]
        public void GenerateGuid_IsPasswordModelNotValid_ReturnPassword()
        {
            //Arrange
            var guid = Guid.NewGuid().ToString();

            guidService.Setup(x => x.NewGuidString()).Returns(guid);

            //Act
            var result = service.GenerateGuid();

            //Assert
            result.Should().Be(guid);
        }

        #endregion

        #region generate password

        [Test]
        public void GeneratePassword_IsPasswordModelNotValid_ReturnPassword()
        {
            //Arrange
            var passwordModel = new PasswordModel
            {
                IncludeLowerCase = false,
                IncludeNumeric = false
            };

            //Act
            var result = service.GeneratePassword(passwordModel);

            //Assert
            result.Should().Be(Messages.GeneratePasswordRequireCharacterType);
        }

        [Test]
        public void GeneratePassword_IsPasswordModelValid_ReturnPassword()
        {
            //Arrange
            var password = "h12345";
            var passwordModel = new PasswordModel
            {
                IncludeLowerCase = true,
                IncludeNumeric = true
            };

            generatePasswordWrapper.Setup(s => s.GeneratePassword(passwordModel)).Returns(password);

            //Act
            var result = service.GeneratePassword(passwordModel);

            //Assert
            result.Should().Be(password);
        }

        [Test]
        public void GeneratePassword_Fails_ReturnPassword()
        {
            //Arrange
            var passwordModel = new PasswordModel
            {
                IncludeLowerCase = true,
                IncludeNumeric = true
            };

            var methodName = "SecurityService.GeneratePassword";
            var message = Messages.GeneralError;
            var exception = new Exception();

            generatePasswordWrapper.Setup(s => s.GeneratePassword(passwordModel)).Throws(exception);
            loggingService.Setup(x => x.LogError(methodName, message, exception));

            //Act
            var result = service.GeneratePassword(passwordModel);

            //Assert
            result.Should().Be(message);
        }

        #endregion
    }
}