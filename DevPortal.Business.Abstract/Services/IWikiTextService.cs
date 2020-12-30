namespace DevPortal.Business.Abstract
{
    public interface IWikiTextService
    {
        string GenerateH2(string headerText);

        string GenerateTableHeader(string[] columns);

        string GenerateTableRow(string[] rowData);

        string GenerateWikiLink(string redmineProjectName, string linkText);
    }
}