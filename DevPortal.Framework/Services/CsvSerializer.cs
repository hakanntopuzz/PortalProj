using DevPortal.Framework.Abstract;
using DevPortal.Framework.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevPortal.Framework.Services
{
    public class CsvSerializer : ICsvSerializer
    {
        const string SEPERATOR = ",";

        public string SerializeArray<T>(IEnumerable<T> array, IEnumerable<string> columnNames)
        {
            var builder = new StringBuilder();
            var properties = typeof(T).GetProperties();
            builder.AppendLine(string.Join(SEPERATOR, columnNames));

            foreach (var element in array)
            {
                var line = string.Join(SEPERATOR, properties.Select(p => p.GetValue(element, null)));
                builder.AppendLine(line.RemoveLineBreaks());
            }

            return builder.ToString();
        }
    }
}