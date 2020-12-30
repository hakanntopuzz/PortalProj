using DevPortal.Business.Abstract;
using System.Text;

namespace DevPortal.Business
{
    public abstract class BaseWikiService

    {
        #region ctor

        protected readonly IWikiTextService wikiTextService;

        protected BaseWikiService(IWikiTextService wikiTextService)
        {
            this.wikiTextService = wikiTextService;
        }

        #endregion

        protected void AppendHeader(StringBuilder builder, string headerText)
        {
            if (builder == null)
            {
                return;
            }

            var header = wikiTextService.GenerateH2(headerText);
            builder.Append(header);
        }

        protected void AppendTableHeader(StringBuilder builder, string[] columns)
        {
            if (builder == null)
            {
                return;
            }

            var headerRow = wikiTextService.GenerateTableHeader(columns);
            builder.Append(headerRow);
        }

        protected void AppendTableRow(StringBuilder builder, string[] rowData)
        {
            if (builder == null)
            {
                return;
            }

            var row = wikiTextService.GenerateTableRow(rowData);
            builder.Append(row);
        }
    }
}