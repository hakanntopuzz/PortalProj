using DevPortal.Model;
using DevPortal.Web.Library.Model;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DevPortal.UnitTests.TestHelpers
{
    public static class AssertHelpers
    {
        public static void AssertViewResult(ApplicationEnvironmentViewModel viewModel, string viewName, IActionResult result)
        {
            var resultModel = result.Should().BeViewResult().WithViewName(viewName).ModelAs<ApplicationEnvironmentViewModel>();
            resultModel.Should().Be(viewModel);
        }

        public static void AssertViewResult(ApplicationJenkinsJobViewModel viewModel, string viewName, IActionResult result)
        {
            var resultModel = result.Should().BeViewResult().WithViewName(viewName).ModelAs<ApplicationJenkinsJobViewModel>();
            resultModel.Should().Be(viewModel);
        }

        public static void AssertClientDataResult(IActionResult result, ClientDataResult expectedResult)
        {
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<ClientDataResult>();
            resultModel.Should().BeEquivalentTo(expectedResult);
        }

        public static void AssertRedirectableClientActionResult(IActionResult result, RedirectableClientActionResult expectedResult)
        {
            var resultModel = result.Should().BeOfType<JsonResult>().Which.Value.As<RedirectableClientActionResult>();
            resultModel.Should().BeEquivalentTo(expectedResult);
        }

        public static void AssertResultMessageTempData(Controller controller, Dictionary<string, string> expectedResultMessage)
        {
            controller.TempData.Should().ContainKey(nameof(TempDataKeys.ResultMessage)).WhichValue.Should().BeEquivalentTo(expectedResultMessage);
        }

        public static void AssertViewResult(ApplicationSonarQubeProjectViewModel viewModel, string viewName, IActionResult result)
        {
            var resultModel = result.Should().BeViewResult().WithViewName(viewName).ModelAs<ApplicationSonarQubeProjectViewModel>();
            resultModel.Should().Be(viewModel);
        }

        public static void AssertViewResult(ApplicationSvnViewModel viewModel, string viewName, IActionResult result)
        {
            var resultModel = result.Should().BeViewResult().WithViewName(viewName).ModelAs<ApplicationSvnViewModel>();
            resultModel.Should().Be(viewModel);
        }

        public static void AssertCsvServiceResult(byte[] encodedBytes, CsvServiceResult result)
        {
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeNull();
            result.Value.Should().BeSameAs(encodedBytes);
        }

        public static void AssertRedirectToAction(IActionResult result, string actionName, string controllerName)
        {
            result.Should().BeRedirectToActionResult().WithActionName(actionName).WithControllerName(controllerName);
        }
    }
}