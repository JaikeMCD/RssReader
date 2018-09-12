using System;
using Microsoft.SyndicationFeed;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Xml.Serialization;
using System.Web;
using System.Net;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Collections.Generic;

namespace RssImporter
{

    class CustomFieldProcessor
    {
        public IEnumerable<ISyndicationContent> CustomFields { get; set; }

        public CustomFieldProcessor(IEnumerable<ISyndicationContent> customFields)
        {
            CustomFields = customFields;
            
        }
    } 
    class Program
    {

        static void Main(string[] args)
        {
            ReadRss();
            Console.ReadLine();
        }

        static async void ReadRss()
        {

            using (var xmlReader = XmlReader.Create("C:/Users/Developer/Desktop/brisbane-city-council.rss",
            new XmlReaderSettings() { Async = true }))
            {
                var parser = new RssParser();
                var feedReader = new RssFeedReader(xmlReader, parser);
                string[] attValues = new string[] { "title", "link", "ealink", "location", "category",
                "localstart", "localend", "cdo-alldayevent", "description"};
                string[] custValues = new string[] { "Event Type", "Venue", "Cost", "Age" };


                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == SyndicationElementType.Item)
                    {

                        ISyndicationContent content = await feedReader.ReadContent();

                        ISyndicationItem item = parser.CreateItem(content);

                        for (int i = 0; i < attValues.Length; i++)
                        {
                            ISyndicationContent value = content.Fields.FirstOrDefault(f => f.Name == attValues[i]);
                            if (attValues[i] == "description")
                            {
                                string decodedDesc = WebUtility.HtmlDecode(value.Value);
                                Console.WriteLine($"{value.Name}: {decodedDesc}");
                            }
                            else
                            {
                                Console.WriteLine($"{value.Name}: {value.Value}");
                            }
                        }

                        var customFields = content.Fields.Where(f => f.Name == "customfield");

                        for (int i = 0; i < custValues.Length; i++)
                        {
                            var customatt = CustomFieldValue(customFields, custValues[i]);
                            Console.WriteLine(custValues[i] + ": " + customatt);
                        }

                        Console.WriteLine(Environment.NewLine);
                        
                    }
                }
            }
        }

        public static string CustomFieldValue(IEnumerable<ISyndicationContent> customFields, string name)
        {
            var field = GetCustomFieldForName(customFields, name);
            return field == null ? string.Empty : field.Value;
        }

        public static ISyndicationContent GetCustomFieldForName(IEnumerable<ISyndicationContent> customFields, string name)
        {
            foreach (var field in customFields)
            {
                foreach (var attribute in field.Attributes)               
                {
                    if (attribute.Name == "name" && attribute.Value == name)
                        return field;
                    else break;
                }
            }

            return null;
        }
    }
}