using DevPortal.Framework.Abstract;
using System;

namespace DevPortal.Framework.Services
{
    public class GuidService : IGuidService
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }

        public string NewGuidString()
        {
            var guid = NewGuid();

            return guid.ToString();
        }
    }
}