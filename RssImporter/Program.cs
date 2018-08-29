using System;
using Microsoft.SyndicationFeed;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Xml.Serialization;
using System.Web;

namespace RssImporter
{
    class Program
    {

        static void Main(string[] args)
        {
            ReadRss();
            Console.ReadLine();
        }

            static async void ReadRss()
            {

                using (var xmlReader = XmlReader.Create("C:/Users/Developer/Desktop/brisbane-city-council.rss", new XmlReaderSettings() { Async = true }))
                {
                    var feedReader = new RssFeedReader(xmlReader);

                    while (await feedReader.Read())
                    {
                        switch (feedReader.ElementType)
                        {

                            // Read Item
                            case SyndicationElementType.Item:
                                ISyndicationItem item = await feedReader.ReadItem();
                                Console.WriteLine(item.Title + Environment.NewLine + item.Description + Environment.NewLine + item.Links + 
                                Environment.NewLine + item.Categories + Environment.NewLine + Environment.NewLine);
                            break;
                    }
                    }
                }

            }
    }
}
