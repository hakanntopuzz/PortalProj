using System.Collections.Generic;

namespace DevPortal.Framework.Abstract
{
    public interface ICsvService
    {
        byte[] ExportToCsv<T>(IEnumerable<T> data, string[] columnNames);
    }
}