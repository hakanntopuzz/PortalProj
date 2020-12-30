using AB.Framework.Logger.Nlog.Abstract;
using AB.Framework.UnitTests;
using DevPortal.Cryptography.Business.Abstract;
using DevPortal.Cryptography.Business.Abstract.Wrappers;
using DevPortal.Cryptography.Business.Services;
using DevPortal.Cryptography.Model;
using DevPortal.Framework.Abstract;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevPortal.UnitTests.CryptoManagerTests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class CryptographyServiceTests : BaseTestFixture
    {
        #region members & setup

        StrictMock<IAesCryptography> aesCryptography;

        StrictMock<ITripleDesCryptography> tripleDesCryptography;

        StrictMock<ICryptographyConfigModelMapper> configModelMapper;

        StrictMock<ISettings> settings;

        StrictMock<ILoggingService> logger;

        StrictMock<ISecurityService> securityService;

        CryptographyService service;

        [SetUp]
        public void Initialize()
        {
            aesCryptography = new StrictMock<IAesCryptography>();
            tripleDesCryptography = new StrictMock<ITripleDesCryptography>();
            configModelMapper = new StrictMock<ICryptographyConfigModelMapper>();
            settings = new StrictMock<ISettings>();
            logger = new StrictMock<ILoggingService>();
            securityService = new StrictMock<ISecurityService>();

            service = new CryptographyService(
                aesCryptography.Object,
                tripleDesCryptography.Object,
                configModelMapper.Object,
                settings.Object,
                logger.Object,
                securityService.Object);
        }

        #endregion

        #region verify mocks

        protected override void VerifyMocks()
        {
            aesCryptography.VerifyAll();
            tripleDesCryptography.VerifyAll();
            configModelMapper.VerifyAll();
            settings.VerifyAll();
            logger.VerifyAll();
            securityService.VerifyAll();
        }

        #endregion

        [Test]
        public void GetSingleCryptography_Hasex_ReturnEmptyValue()
        {
            // Arrange
            var text = "xxxxx";
            var regPath = "path";
            var cryptographyType = CryptographyTypes.Aes;
            var convertType = ConvertTypes.Decrypt;
            var ex = new Exception();
            var methodName = "CryptographyService.GetSingleCryptography";
            var message = "Kripto işlemi sırasında bir hata oluştu.";

            settings.Setup(x => x.RegPathByAes).Returns(regPath);
            aesCryptography.Setup(x => x.Decrypt(text, regPath)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().Be(Messages.InvalidParameterError);
            result.Should().NotBeNullOrEmpty();
        }

        #region get single cryptography with aes

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithAesAndConvertTypeWithDecrypt_ReturnDecryptedValue()
        {
            // Arrange
            var text = "xxxxx";
            var regPath = "path";
            var decryptedText = "qwerty";
            var cryptographyType = CryptographyTypes.Aes;
            var convertType = ConvertTypes.Decrypt;

            settings.Setup(x => x.RegPathByAes).Returns(regPath);
            aesCryptography.Setup(x => x.Decrypt(text, regPath)).Returns(decryptedText);

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(decryptedText);
        }

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithAesAndConvertTypeWithEncrypt_ReturnEncryptedValue()
        {
            // Arrange
            var text = "asdf";
            var regPath = "path";
            var encryptedText = "xxx";
            var cryptographyType = CryptographyTypes.Aes;
            var convertType = ConvertTypes.Encrypt;

            settings.Setup(x => x.RegPathByAes).Returns(regPath);
            aesCryptography.Setup(x => x.Encrypt(text, regPath)).Returns(encryptedText);

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(encryptedText);
        }

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithInvalidArgument_ReturnErrorMessage()
        {
            // Arrange
            var text = "asdf";
            var cryptographyType = CryptographyTypes.InvalidType;
            var convertType = ConvertTypes.Encrypt;

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithAesAndConvertTypeWithInvalidArgument_ReturnErrorMessage()
        {
            // Arrange
            var text = "asdf";
            var regPath = "path";
            var cryptographyType = CryptographyTypes.Aes;
            var convertType = ConvertTypes.InvalidType;

            settings.Setup(x => x.RegPathByAes).Returns(regPath);

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        #endregion

        #region get single cryptography with 3des

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithTripleDesAndConvertTypeWithDecrypt_ReturnDecryptedValue()
        {
            // Arrange
            var text = "xxxxx";
            var regPath = "path";
            var decryptedText = "qwerty";
            var cryptographyType = CryptographyTypes.TripleDes;
            var convertType = ConvertTypes.Decrypt;

            settings.Setup(x => x.RegPathByTripleDes).Returns(regPath);
            tripleDesCryptography.Setup(x => x.Decrypt(text, regPath)).Returns(decryptedText);

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(decryptedText);
        }

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithTripleAndConvertTypeWithEncrypt_ReturnEncryptedValue()
        {
            // Arrange
            var text = "asdf";
            var regPath = "path";
            var encryptedText = "xxx";
            var cryptographyType = CryptographyTypes.TripleDes;
            var convertType = ConvertTypes.Encrypt;

            settings.Setup(x => x.RegPathByTripleDes).Returns(regPath);
            tripleDesCryptography.Setup(x => x.Encrypt(text, regPath)).Returns(encryptedText);

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(encryptedText);
        }

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithInvalidArgumentByTripleDes_ReturnErrorMessage()
        {
            // Arrange
            var text = "asdf";
            var cryptographyType = CryptographyTypes.InvalidType;
            var convertType = ConvertTypes.Encrypt;

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void GetSingleCryptography_CryptographyTypeWithTripleDesAndConvertTypeWithInvalidArgument_ReturnErrorMessage()
        {
            // Arrange
            var text = "asdf";
            var regPath = "path";
            var cryptographyType = CryptographyTypes.TripleDes;
            var convertType = ConvertTypes.InvalidType;

            settings.Setup(x => x.RegPathByTripleDes).Returns(regPath);

            // Act
            var result = service.GetSingleCryptography(cryptographyType, convertType, text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        #endregion

        #region multiple encrypted with aes256

        [Test]
        public void MultipleEncryptedWithAes256_Throwex_ReturnEmptyValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var ex = new Exception();
            var methodName = "CryptographyService.MultipleEncryptedWithAes256";
            var message = "Kripto işlemi sırasında bir hata oluştu.";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.MultipleEncryptedWithAes256(text);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void MultipleEncryptedWithAes256_InvalidMatchesFormat_ReturnInvalidMatchesFormatMessage()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            List<ConfigModel> configModels = null;

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);

            // Act
            var result = service.MultipleEncryptedWithAes256(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void MultipleEncryptedWithAes256_NoCondition_ReturnEncryptedValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var configModels = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="aaa"
                }
            };

            var encryptedList = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="xxx"
                }
            };

            var regPath = "path";
            var connectionList = "connections";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);
            settings.Setup(x => x.RegPathByAes).Returns(regPath);
            aesCryptography.Setup(x => x.MultipleEncrypt(configModels, regPath)).Returns(encryptedList);
            configModelMapper.Setup(x => x.CreateMultipleValueFormat(encryptedList)).Returns(connectionList);

            // Act
            var result = service.MultipleEncryptedWithAes256(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(connectionList);
        }

        #endregion

        #region multiple decrypted with aes256

        [Test]
        public void MultipleDecryptedWithAes256_Throwex_ReturnEmptyValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var ex = new Exception();
            var methodName = "CryptographyService.MultipleDecryptedWithAes256";
            var message = "Kripto işlemi sırasında bir hata oluştu.";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.MultipleDecryptedWithAes256(text);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void MultipleDecryptedWithAes256_InvalidMatchesFormat_ReturnInvalidMatchesFormatMessage()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            List<ConfigModel> configModels = null;

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);

            // Act
            var result = service.MultipleDecryptedWithAes256(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void MultipleDecryptedWithAes256_NoCondition_ReturnEncryptedValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var configModels = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="aaa"
                }
            };

            var encryptedList = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="xxx"
                }
            };

            var regPath = "path";
            var connectionList = "connections";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);
            settings.Setup(x => x.RegPathByAes).Returns(regPath);
            aesCryptography.Setup(x => x.MultipleDecrypt(configModels, regPath)).Returns(encryptedList);
            configModelMapper.Setup(x => x.CreateMultipleValueFormat(encryptedList)).Returns(connectionList);

            // Act
            var result = service.MultipleDecryptedWithAes256(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(connectionList);
        }

        #endregion

        #region multiple encrypted with 3des

        [Test]
        public void MultipleEncryptedWithTripleDes_Throwex_ReturnEmptyValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var ex = new Exception();
            var methodName = "CryptographyService.MultipleEncryptedWithTripleDes";
            var message = "Kripto işlemi sırasında bir hata oluştu.";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.MultipleEncryptedWithTripleDes(text);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void MultipleEncryptedWithTripleDes_InvalidMatchesFormat_ReturnInvalidMatchesFormatMessage()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            List<ConfigModel> configModels = null;

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);

            // Act
            var result = service.MultipleEncryptedWithTripleDes(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void MultipleEncryptedWithTripleDes_NoCondition_ReturnEncryptedValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var configModels = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="aaa"
                }
            };

            var encryptedList = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="xxx"
                }
            };

            var regPath = "path";
            var connectionList = "connections";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);
            settings.Setup(x => x.RegPathByTripleDes).Returns(regPath);
            tripleDesCryptography.Setup(x => x.MultipleEncrypt(configModels, regPath)).Returns(encryptedList);
            configModelMapper.Setup(x => x.CreateMultipleValueFormat(encryptedList)).Returns(connectionList);

            // Act
            var result = service.MultipleEncryptedWithTripleDes(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(connectionList);
        }

        #endregion

        #region multiple decrypted with 3des

        [Test]
        public void MultipleDecryptedWithTripleDes_Throwex_ReturnEmptyValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var ex = new Exception();
            var methodName = "CryptographyService.MultipleDecryptedWithTripleDes";
            var message = "Kripto işlemi sırasında bir hata oluştu.";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.MultipleDecryptedWithTripleDes(text);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void MultipleDecryptedWithTripleDes_InvalidMatchesFormat_ReturnInvalidMatchesFormatMessage()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            List<ConfigModel> configModels = null;

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);

            // Act
            var result = service.MultipleDecryptedWithTripleDes(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void MultipleDecryptedWithTripleDes_NoCondition_ReturnEncryptedValue()
        {
            // Arrange
            var text = "<add name=\"Prod\" connectionString=\"prod connection\" />";
            var configModels = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="aaa"
                }
            };

            var encryptedList = new List<ConfigModel>
            {
                new ConfigModel
                {
                    Name="qqq",
                    Value="xxx"
                }
            };

            var regPath = "path";
            var connectionList = "connections";

            configModelMapper.Setup(x => x.TextToConfigModelConvert(text)).Returns(configModels);
            settings.Setup(x => x.RegPathByTripleDes).Returns(regPath);
            tripleDesCryptography.Setup(x => x.MultipleDecrypt(configModels, regPath)).Returns(encryptedList);
            configModelMapper.Setup(x => x.CreateMultipleValueFormat(encryptedList)).Returns(connectionList);

            // Act
            var result = service.MultipleDecryptedWithTripleDes(text);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(connectionList);
        }

        #endregion

        #region  aes

        [Test]
        public void DoAesAction_Hasex_ReturnEmptyValue()
        {
            // Arrange
            var text = "xxxxx";
            var convertType = ConvertTypes.Decrypt;
            var siteId = 1;
            var ex = new Exception();
            var methodName = "CryptographyService.DoAesAction";
            var message = "AES Kripto işlemi sırasında bir hata oluştu.";

            securityService.Setup(x => x.Decrypt(text, siteId)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.DoAesAction(convertType, text, siteId);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        [Test]
        public void DoAesAction_ConvertTypeEncrypt_ReturnEncryptValue()
        {
            // Arrange
            var text = "xxxxx";
            var convertType = ConvertTypes.Encrypt;
            var siteId = 1;
            var ex = new Exception();
            var methodName = "CryptographyService.DoAesAction";
            var message = "AES Kripto işlemi sırasında bir hata oluştu.";

            securityService.Setup(x => x.Encrypt(text, siteId)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.DoAesAction(convertType, text, siteId);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void DoAesAction_ConvertTypeDecrypt_ReturnDecryptValue()
        {
            // Arrange
            var text = "xxxxx";
            var convertType = ConvertTypes.Decrypt;
            var siteId = 1;
            var ex = new Exception();
            var methodName = "CryptographyService.DoAesAction";
            var message = "AES Kripto işlemi sırasında bir hata oluştu.";

            securityService.Setup(x => x.Decrypt(text, siteId)).Throws(ex);
            logger.Setup(x => x.LogError(methodName, message, ex));

            // Act
            var result = service.DoAesAction(convertType, text, siteId);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void DoAesAction_ConvertTypeInvalidType_ReturnEmptyValue()
        {
            // Arrange
            var text = "xxxxx";
            var convertType = ConvertTypes.InvalidType;
            var siteId = 1;

            // Act
            var result = service.DoAesAction(convertType, text, siteId);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Be(Messages.InvalidParameterError);
        }

        #endregion
    }
}