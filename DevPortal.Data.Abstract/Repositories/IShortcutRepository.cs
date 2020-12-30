using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract.Repositories
{
    public interface IShortcutRepository
    {
        ICollection<Link> GetFavouriteRedmineWikiPages();

        ICollection<Link> GetFavouriteRedmineProjects();

        ICollection<Link> GetToolPages();
    }
}