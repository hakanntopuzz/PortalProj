using DevPortal.Cryptography.Business.Abstract;
using DevPortal.Cryptography.Model;
using DevPortal.Framework.Abstract;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.Controllers
{
    public class SecurityController : BaseController
    {
        #region ctor

        readonly ICryptographyService cryptographyService;

        readonly ISecurityService securityService;

        readonly ISecurityViewModelFactory securityViewModelFactory;

        public SecurityController(
            IUserSessionService userSessionService,
            ICryptographyService cryptographyService,
            ISecurityService securityService,
            ISecurityViewModelFactory viewModelFactory) : base(userSessionService)
        {
            this.cryptographyService = cryptographyService;
            this.securityService = securityService;
            this.securityViewModelFactory = viewModelFactory;
        }

        #endregion

        #region single cryptography

        public IActionResult SingleCryptography()
        {
            var model = securityViewModelFactory.CreateCryptoViewModel();

            return View(ViewNames.SingleCryptography, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SingleCryptography(CryptoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.SingleCryptography, model);
            }

            model.ConvertedText = cryptographyService.GetSingleCryptography(model.CryptographyType, model.ConvertType, model.ConvertibleText);

            return View(ViewNames.SingleCryptography, model);
        }

        #endregion

        #region multiple encrypt with aes256

        public IActionResult MultipleEncryptWithAes256()
        {
            var model = securityViewModelFactory.CreateCryptoViewModel();

            return View(ViewNames.MultipleEncryptWithAes256, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MultipleEncryptWithAes256(CryptoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.MultipleEncryptWithAes256, model);
            }

            model.ConvertedText = cryptographyService.MultipleEncryptedWithAes256(model.ConvertibleText);

            return View(ViewNames.MultipleEncryptWithAes256, model);
        }

        #endregion

        #region multiple decrypt with aes256

        public IActionResult MultipleDecryptWithAes256()
        {
            var model = securityViewModelFactory.CreateCryptoViewModel();

            return View(ViewNames.MultipleDecryptWithAes256, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MultipleDecryptWithAes256(CryptoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.MultipleDecryptWithAes256, model);
            }

            model.ConvertedText = cryptographyService.MultipleDecryptedWithAes256(model.ConvertibleText);

            return View(ViewNames.MultipleDecryptWithAes256, model);
        }

        #endregion

        #region multiple encrypt with 3Des

        public IActionResult MultipleEncryptWithTripleDes()
        {
            var model = securityViewModelFactory.CreateCryptoViewModel();

            return View(ViewNames.MultipleEncryptWithTripleDes, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MultipleEncryptWithTripleDes(CryptoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.MultipleEncryptWithTripleDes, model);
            }

            model.ConvertedText = cryptographyService.MultipleEncryptedWithTripleDes(model.ConvertibleText);

            return View(ViewNames.MultipleEncryptWithTripleDes, model);
        }

        #endregion

        #region multiple decrypt with 3Des

        public IActionResult MultipleDecryptWithTripleDes()
        {
            var model = securityViewModelFactory.CreateCryptoViewModel();

            return View(ViewNames.MultipleDecryptWithTripleDes, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MultipleDecryptWithTripleDes(CryptoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.MultipleDecryptWithTripleDes, model);
            }

            model.ConvertedText = cryptographyService.MultipleDecryptedWithTripleDes(model.ConvertibleText);

            return View(ViewNames.MultipleDecryptWithTripleDes, model);
        }

        #endregion

        #region aes cryptography

        public IActionResult Aes()
        {
            var model = securityViewModelFactory.CreateAesViewModel();

            return View(ViewNames.Aes, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Aes(AesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return Aes();
            }

            var convertedText = cryptographyService.DoAesAction(viewModel.ConvertType, viewModel.ConvertibleText, viewModel.SiteId);
            viewModel = securityViewModelFactory.CreateAesViewModel(convertedText);

            return View(ViewNames.Aes, viewModel);
        }

        #endregion

        #region  hash

        public IActionResult Hash()
        {
            var model = securityViewModelFactory.CreateHashViewModel();

            return View(ViewNames.Hash, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Hash(HashViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Hash, model);
            }

            var hashedText = securityService.CreateHash(model.HashToText, model.HashType);
            model.HashedText = hashedText;

            return View(ViewNames.Hash, model);
        }

        #endregion

        #region  guid

        public IActionResult Guid()
        {
            var model = securityViewModelFactory.CreateGuidViewModel();

            return View(ViewNames.Guid, model);
        }

        public IActionResult GenerateGuid()
        {
            var guid = securityService.GenerateGuid();
            var model = securityViewModelFactory.CreateGuidViewModel(guid);

            return View(ViewNames.Guid, model);
        }

        #endregion

        #region password

        public IActionResult Password()
        {
            var model = securityViewModelFactory.CreateGeneratePasswordViewModel();

            return View(ViewNames.Password, model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(PasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(ViewNames.Password, model);
            }

            var pass = securityService.GeneratePassword(model);
            var generatePasswordModel = securityViewModelFactory.CreateGeneratePasswordViewModel(pass);

            return View(ViewNames.Password, generatePasswordModel);
        }

        #endregion
    }
}