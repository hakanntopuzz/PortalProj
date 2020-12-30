using DevPortal.Framework.Abstract;
using Newtonsoft.Json;

namespace DevPortal.Framework.Wrappers
{
    public class JsonConventerWrapper : IJsonConventer
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}