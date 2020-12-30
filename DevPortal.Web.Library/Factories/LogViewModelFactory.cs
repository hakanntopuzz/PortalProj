using DevPortal.Model;
using DevPortal.Web.Library.Abstract;
using DevPortal.Web.Library.Model;
using System.Collections.Generic;

namespace DevPortal.Web.Library
{
    public class LogViewModelFactory : ILogViewModelFactory
    {
        #region ctor

        readonly IBreadCrumbFactory breadCrumbFactory;

        public LogViewModelFactory(
            IBreadCrumbFactory breadCrumbFactory)
        {
            this.breadCrumbFactory = breadCrumbFactory;
        }

        #endregion

        #region log manager

        public LogViewModel CreateLogViewModel()
        {
            return new LogViewModel();
        }

        public LogViewModel CreateLogViewModel(ICollection<ApplicationGroup> applicationGroups, string physicalPath)
        {
            return new LogViewModel
            {
                ApplicationGroups = applicationGroups,
                LogFilePath = physicalPath,
                BreadCrumbViewModel = breadCrumbFactory.CreateLogListModel()
            };
        }

        public LogFileViewModel CreateLogFileViewModel(LogFileModel logFile)
        {
            return new LogFileViewModel
            {
                LogFile = logFile,
                BreadCrumbViewModel = breadCrumbFactory.CreateLogDetailModel(logFile.FilePath)
            };
        }

        #endregion
    }
}