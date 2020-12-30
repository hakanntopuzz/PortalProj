using DevPortal.Framework.Abstract;
using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPortal.Framework.Wrappers
{
    public class XmlFeedReaderWrapper : IXmlFeedReader
    {
        public IEnumerable<SyndicationItem> GetFeedItems(Uri uri)
        {
            if (uri == null)
            {
                return new List<SyndicationItem>();
            }

            var reader = XmlReader.Create(uri.ToString());
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            return feed.Items;
        }
    }
}