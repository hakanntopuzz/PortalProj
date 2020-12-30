using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace DevPortal.Framework.Abstract
{
    public interface IXmlFeedReader
    {
        IEnumerable<SyndicationItem> GetFeedItems(Uri uri);
    }
}