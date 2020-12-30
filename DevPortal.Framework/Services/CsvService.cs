using DevPortal.Framework.Abstract;
using System.Collections.Generic;

namespace DevPortal.Framework.Services
{
    public class CsvService : ICsvService
    {
        #region ctor

        readonly ICsvSerializer csvSerializer;

        readonly IEncoding encoding;

        public CsvService(ICsvSerializer csvSerializer, IEncoding encoding)
        {
            this.csvSerializer = csvSerializer;
            this.encoding = encoding;
        }

        #endregion

        public byte[] ExportToCsv<T>(IEnumerable<T> data, string[] columnNames)
        {
            var textData = csvSerializer.SerializeArray(data, columnNames);

            return encoding.GetBytes(textData);
        }
    }
}