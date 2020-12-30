using System;
using System.Linq;

namespace DevPortal.Framework.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToNullOrString(this object o)
        {
            return o?.ToString();
        }

        /// <summary>
        /// Verilen nesne değerini Convert.Boolean metodunu kullanarak System.Boolean tipine çevirir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static bool ToBoolean(this object value)
        {
            return Convert.ToBoolean(value);
        }

        /// <summary>
        /// Verilen nesne değerini Convert.ToInt32 metodunu kullanarak System.Int32 tipine çevirir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        /// Verilen nesne değerini Convert.ToDouble metodunu kullanarak System.Double tipine çevirir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static double ToDouble(this object value)
        {
            return Convert.ToDouble(value);
        }

        /// <summary>
        /// Verilen nesne değerini Convert.ToByte metodunu kullanarak System.Byte tipine çevirir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static byte ToByte(this object value)
        {
            return Convert.ToByte(value);
        }

        /// <summary>
        /// Verilen nesne değerini Convert.ToDateTime metodunu kullanarak System.DateTime tipine çevirir.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>string</returns>
        public static DateTime ToDateTime(this object value)
        {
            return Convert.ToDateTime(value);
        }

        public static string ToFirstLetterUpper(this string value) => value.First().ToString().ToUpper() + value.Substring(1);

        /// <summary>
        /// bir sınıfın, verilen alan ismine ait değeri döndürür.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetValue(this object obj, string propertyName)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();
            var property = properties.FirstOrDefault(x => x.Name == propertyName);

            if (property == null)
            {
                return null;
            }

            return property.GetValue(obj, null);
        }
    }
}