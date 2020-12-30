using System;

namespace DevPortal.Framework.Abstract
{
    public interface IGuidService
    {
        string NewGuidString();

        Guid NewGuid();
    }
}