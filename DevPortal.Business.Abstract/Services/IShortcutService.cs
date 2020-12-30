using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Business.Abstract
{
    public interface IShortcutService
    {
        ICollection<Link> GetFavouriteRedmineProjects();

        ICollection<Link> GetFavouriteRedmineWikiPages();

        ICollection<Link> GetToolPages();
    }
}