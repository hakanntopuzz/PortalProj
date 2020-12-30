using DevPortal.Framework.Abstract;
using DevPortal.Model;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    public class BaseController : Controller
    {
        #region ctor

        readonly IUserSessionService userSessionService;

        public BaseController(IUserSessionService userSessionService)
        {
            this.userSessionService = userSessionService;
        }

        #endregion

        #region get current user id

        protected int CurrentUserId
        {
            get
            {
                return userSessionService.GetCurrentUserId();
            }
        }

        #endregion

        #region set temp data

        protected void SetResultMessageTempData(bool isSuccess, string message)
        {
            if (isSuccess)
            {
                SetSuccessResultMessageTempData(message);
            }
            else
            {
                SetErrorResultMessageTempData(message);
            }
        }

        protected void SetSuccessResultMessageTempData(string message)
        {
            SetResultMessageTempData(MessageType.Success, message);
        }

        protected void SetErrorResultMessageTempData(string message)
        {
            SetResultMessageTempData(MessageType.Error, message);
        }

        void SetResultMessageTempData(MessageType messageType, string message)
        {
            TempData[TempDataKeys.ResultMessage] = new Dictionary<string, string>
            {
                [TempDataKeys.MessageType] = messageType.ToString(),
                [TempDataKeys.Message] = message
            };
        }

        #endregion

        protected static bool ApplicationExists(Application application)
        {
            return application != null;
        }

        protected IActionResult ApplicationNotFoundResult()
        {
            SetErrorResultMessageTempData(Messages.ApplicationNotFound);

            return RedirectToAction(ApplicationControllerActionNames.Index, ControllerNames.Application);
        }

    }
}