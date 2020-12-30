using System.Collections.Generic;

namespace DevPortal.Framework.Abstract
{
    public interface ICsvSerializer
    {
        /// <summary>
        /// Verilen T tipindeki öğelerden oluşan koleksiyonu virgül ayıracı ile CSV formatındaki metne çevirir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        string SerializeArray<T>(IEnumerable<T> array, IEnumerable<string> columnNames);
    }
}