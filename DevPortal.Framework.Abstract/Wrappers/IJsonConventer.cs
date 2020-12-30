namespace DevPortal.Framework.Abstract
{
    public interface IJsonConventer
    {
        T DeserializeObject<T>(string value);

        string SerializeObject(object value);
    }
}