using DevPortal.Business.Abstract;
using System.Text;

namespace DevPortal.Business.Services
{
    public class WikiTextService : IWikiTextService
    {
        public string GenerateH2(string headerText)
        {
            return $"h2. {headerText}\r\n\r\n";
        }

        public string GenerateTableHeader(string[] columns)
        {
            if (columns == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var column in columns)
            {
                builder.Append($"|*{column}*");
            }

            builder.Append("|\r\n");

            return builder.ToString();
        }

        public string GenerateTableRow(string[] rowData)
        {
            if (rowData == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var cell in rowData)
            {
                builder.Append($"|{cell}");
            }

            builder.Append("|\r\n");

            return builder.ToString();
        }

        public string GenerateWikiLink(string redmineProjectName, string linkText)
        {
            return $"[[{redmineProjectName}:|{linkText}]]";
        }
    }
}