using DevPortal.Model;

namespace DevPortal.Web.Library.Model
{
    public class ApplicationNugetPackageViewModel : AuthorizedBaseViewModel
    {
        public ApplicationNugetPackage ApplicationNugetPackage { get; set; }

        public string NugetPackageUrl { get; set; }
    }
}