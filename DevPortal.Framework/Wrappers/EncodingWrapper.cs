using DevPortal.Framework.Abstract;
using System.Text;

namespace DevPortal.Framework.Wrappers
{
    public class EncodingWrapper : IEncoding
    {
        public byte[] GetBytes(string text)
        {
            return Encoding.GetEncoding("windows-1254").GetBytes(text);
        }
    }
}