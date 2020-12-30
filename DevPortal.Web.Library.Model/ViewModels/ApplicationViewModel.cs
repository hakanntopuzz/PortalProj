using DevPortal.Model;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationViewModel : AuthorizedBaseViewModel
    {
        public Application Application { get; set; }

        public bool IsNugetPackage
        {
            get
            {
                return Application.ApplicationTypeId == (int)ApplicationTypeEnum.NugetPackage;
            }
        }
    }
}