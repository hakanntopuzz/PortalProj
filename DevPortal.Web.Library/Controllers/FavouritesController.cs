using DevPortal.Business.Abstract;
using DevPortal.Framework.Abstract;
using DevPortal.JenkinsManager.Model;
using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Controllers
{
    public class FavouritesController : BaseController
    {
        #region ctor

        readonly IFavouritePageService favouritePageService;

        readonly IFavouritePageViewModelFactory viewModelFactory;

        public FavouritesController(IFavouritePageService favouritePageService,
             IUserSessionService userSessionService,
             IFavouritePageViewModelFactory viewModelFactory)
                : base(userSessionService)
        {
            this.favouritePageService = favouritePageService;
            this.viewModelFactory = viewModelFactory;
        }

        #endregion

        #region index

        public IActionResult Index()
        {
            var favouritePages = GetFavouritePages();
            var viewModel = viewModelFactory.CreateFavouritePagesViewModel(favouritePages);

            return View(ViewNames.Index, viewModel);
        }

        #endregion

        public IActionResult List()
        {
            var favouritePages = GetFavouritePages();
            var viewModel = viewModelFactory.CreateFavouritePagesPartialViewModel(favouritePages);

            return PartialView(PartialViewNames.List, viewModel);
        }

        ICollection<FavouritePage> GetFavouritePages()
        {
            return favouritePageService.GetFavouritePages(CurrentUserId);
        }

        #region add

        [HttpPost]
        public IActionResult Add([FromBody] FavouritePage favouritePage)
        {
            if (!ModelState.IsValid)
            {
                return AddErrorJson();
            }

            SetUserInfoForAdd(favouritePage);

            var addResult = favouritePageService.AddFavouritePage(favouritePage);

            return Json(addResult);
        }

        IActionResult AddErrorJson()
        {
            return Json(ServiceResult.Error(Messages.AddingFails));
        }

        void SetUserInfoForAdd(FavouritePage favouritePage)
        {
            int userId = CurrentUserId;
            favouritePage.UserId = userId;
        }

        #endregion

        #region delete

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var deleteResult = favouritePageService.DeleteFavouritePage(id);

            return Json(deleteResult);
        }

        #endregion

        #region get favourite page by user id and page url

        [HttpGet]
        public IActionResult GetFavouritePageByUserIdAndPageUrl(string pageUrl)
        {
            var favouritePage = favouritePageService.GetFavouritePage(CurrentUserId, pageUrl);

            if (FavouritePageExist(favouritePage))
            {
                return Json(ClientDataResult.Error());
            }

            return Json(ClientDataResult.Success(favouritePage));
        }

        static bool FavouritePageExist(FavouritePage favouritePage)
        {
            return favouritePage == null;
        }

        #endregion

        #region add/remove/check favourites for jenkins and nuget module

        [HttpPost]
        [Route("/add-to-favourites")]
        public IActionResult AddToFavourites([FromBody] AddToFavouritesRequest request)
        {
            if (InvalidAddRequest(request))
            {
                return BadRequest(Messages.InvalidRequest);
            }

            var favouritePage = viewModelFactory.CreateAddFavouritePageModel(request.PageTitle, request.PageUrl, CurrentUserId);

            var addResult = favouritePageService.AddFavouritePage(favouritePage);

            return Ok(addResult);
        }

        static bool InvalidAddRequest(AddToFavouritesRequest request)
        {
            return request == null || string.IsNullOrEmpty(request.PageTitle) || string.IsNullOrEmpty(request.PageUrl.ToString());
        }

        [HttpPost]
        [Route("/remove-from-favourites")]
        public IActionResult RemoveFromFavourites([FromBody] RemoveFromFavouritesRequest request)
        {
            if (InvalidRemoveRequest(request))
            {
                return BadRequest(Messages.InvalidRequest);
            }

            return Delete(request.Id);
        }

        static bool InvalidRemoveRequest(RemoveFromFavouritesRequest request)
        {
            return request == null || request.Id <= 0;
        }

        [HttpGet("{pageUrl}")]
        [Route("/check-page-is-favourites")]
        public IActionResult CheckPageIsFavourite(Uri pageUrl)
        {
            if (pageUrl == null)
            {
                return BadRequest(Messages.InvalidRequest);
            }

            return GetFavouritePageByUserIdAndPageUrl(pageUrl.ToString());
        }

        #endregion

        [HttpPost]
        public IActionResult SortPages(Dictionary<string, List<int>> data)
        {
            var result = favouritePageService.SortFavouritePages(data);

            if (result.IsSuccess)
            {
                return Json(ClientActionResult.Success(result.Message));
            }

            return Json(ClientActionResult.Error(result.Message));
        }
    }
}