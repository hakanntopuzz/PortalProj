using System;

namespace DevPortal.Framework.Abstract
{
    public interface ICacheWrapper
    {
        T Get<T>(string key);

        /// <summary>
        /// Verilen anahtarın objesi kasada yoksa factory metodundan temin edip kasaya ekler ve döndürür.
        /// <br />
        /// SlidingExpiration kullanır, verilen süre boyunca anahtara erişim sağlanmazsa kasadan silinir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="cacheTimeInMinutes"></param>
        /// <returns></returns>
        T GetOrCreateWithSlidingExpiration<T>(string key, Func<T> factory, int cacheTimeInMinutes);

        void Remove(string key);

        /// <summary>
        /// Verilen süre dolduğunda silinecek olan bir öğe ekler.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void AddWithAbsoluteExpiration(string key, object data, int cacheTime);
    }
}