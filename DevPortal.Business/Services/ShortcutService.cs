using DevPortal.Business.Abstract;
using DevPortal.Data.Abstract.Repositories;
using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Services
{
    public class ShortcutService : IShortcutService
    {
        #region ctor

        readonly IGeneralSettingsService generalSettingsService;

        readonly IShortcutRepository shortcutRepository;

        public ShortcutService(IGeneralSettingsService generalSettingsService, IShortcutRepository shortcutRepository)
        {
            this.generalSettingsService = generalSettingsService;
            this.shortcutRepository = shortcutRepository;
        }

        #endregion

        #region get favourite redmine project

        public ICollection<Link> GetFavouriteRedmineProjects()
        {
            var projects = shortcutRepository.GetFavouriteRedmineProjects();

            foreach (var project in projects)
            {
                project.Url = $"{generalSettingsService.GetRedmineProjectUrl(project.Url)}";
            }

            return projects;
        }

        #endregion

        #region get favourite redmine wiki pages

        public ICollection<Link> GetFavouriteRedmineWikiPages()
        {
            var redmineUrl = generalSettingsService.GetRedmineUrl();
            var pages = shortcutRepository.GetFavouriteRedmineWikiPages();

            foreach (var page in pages)
            {
                page.Url = $"{redmineUrl}{page.Url}";
            }

            return pages;
        }

        #endregion

        #region get tool pages

        public ICollection<Link> GetToolPages()
        {
            return shortcutRepository.GetToolPages();
        }

        #endregion
    }
}