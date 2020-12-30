using DevPortal.Model;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library.Abstract
{
    public interface ILogViewModelFactory
    {
        LogViewModel CreateLogViewModel();

        LogViewModel CreateLogViewModel(ICollection<ApplicationGroup> applicationGroups, string physicalPath);

        LogFileViewModel CreateLogFileViewModel(LogFileModel logFile);
    }
}