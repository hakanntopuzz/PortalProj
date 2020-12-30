using AB.Framework.UnitTests;
using DevPortal.Web.Controllers;
using FluentAssertions.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Net;

namespace MxKobi.UnitTests.WebLibrary.ControllerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ErrorControllerTests : LooseBaseTestFixture
    {
        #region setup

        ErrorController controller;

        [SetUp]
        public void Init()
        {
            controller = new ErrorController();
        }

        #endregion

        #region http errors

        [Test]
        public void Error_NoCondition_Display404Page()
        {
            //Arrange

            //Act
            var result = controller.Index();

            //Assert
            result.Should().BeViewResult().WithViewName(Convert.ToInt32(HttpStatusCode.NotFound).ToString());
        }

        [Test]
        public void HttpError_ErrorNotSpecified_RedirectTo400ErrorPage()
        {
            //Arrange

            //Act
            var result = controller.Error();

            //Assert
            result.Should().BeViewResult().WithViewName(Convert.ToInt32(HttpStatusCode.NotFound).ToString());
        }

        [Test]
        public void HttpError_Error404_RedirectTo404ErrorPage()
        {
            //Arrange
            const int err = 404;

            //Act
            var result = controller.Error(err);

            //Assert
            result.Should().BeViewResult().WithViewName(Convert.ToInt32(HttpStatusCode.NotFound).ToString());
        }

        [Test]
        public void HttpError_Error500_RedirectTo500ErrorPage()
        {
            //Arrange
            const int err = 500;

            //Act
            var result = controller.Error(err);

            //Assert
            result.Should().BeViewResult().WithViewName(Convert.ToInt32(HttpStatusCode.InternalServerError).ToString());
        }

        [Test]
        public void HttpError_ErrorWithNoCustomView_RedirectToGeneralErrorPage()
        {
            //Arrange
            const int err = 403;

            //Act
            var result = controller.Error(err);

            //Assert
            result.Should().BeViewResult().WithViewName(Convert.ToInt32(HttpStatusCode.Forbidden).ToString());
        }

        #endregion

        #region page not found

        [Test]
        public void PageNotFound_NoCondition_RedirectToNotFoundPage()
        {
            //Arrange - none

            //Act
            var result = controller.PageNotFound();

            //Assert
            result.Should().BeViewResult().WithViewName(Convert.ToInt32(HttpStatusCode.NotFound).ToString());
        }

        #endregion
    }
}