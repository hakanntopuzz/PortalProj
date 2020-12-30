using DevPortal.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DevPortal.Web.Library.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        #region ctor

        readonly IMenuService menuService;

        public NavbarViewComponent(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        #endregion

        public IViewComponentResult Invoke()
        {
            var menu = menuService.GetMenuListAsParentChild();

            return View(menu);
        }
    }
}