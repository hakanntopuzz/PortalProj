using DevPortal.Model;
using System.Collections.Generic;

namespace DevPortal.Data.Abstract
{
    public interface IBuildScriptRepository
    {
        IEnumerable<BuildType> BuildTypes { get; }
    }
}