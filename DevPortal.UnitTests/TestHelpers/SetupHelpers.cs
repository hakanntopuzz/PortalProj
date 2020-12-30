using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Collections.Generic;

namespace DevPortal.UnitTests.TestHelpers
{
    public static class SetupHelpers
    {
        public static ITempDataDictionary CreateResultMessageTempData(Dictionary<string, string> tempDataKeyValuePairs)
        {
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            tempData[TempDataKeys.ResultMessage] = tempDataKeyValuePairs;

            return tempData;
        }

        public static Dictionary<string, string> CreateResultMessageForTempData(MessageType messageType, string message)
        {
            return new Dictionary<string, string>
            {
                [TempDataKeys.MessageType] = messageType.ToString(),
                [TempDataKeys.Message] = message
            };
        }
    }
}