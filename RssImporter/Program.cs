using System;
using Microsoft.SyndicationFeed;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
            string title = null;
            string description = null;
            string link = null;
            string category = null;
            string localstart = null;
            string localend = null;
            bool allday = false;
            string age = null;
            string cost = null;
            string venue = null;
            string venueaddress = null;

            using (var xmlReader = XmlReader.Create("C:/Users/Developer/Desktop/mob.rss",
            new XmlReaderSettings() { Async = true }))
            {
                var parser = new RssParser();
                var feedReader = new RssFeedReader(xmlReader, parser);
                string[] attValues = new string[] {"title", "description", "link", "category", "localstart", "localend", "cdo-alldayevent"};
                string[] custValues = new string[] {"Cost", "Age", "Venue", "Venue address"};

                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == SyndicationElementType.Item)
                    {

                        ISyndicationContent content = await feedReader.ReadContent();

                        ISyndicationItem item = parser.CreateItem(content);

                        for (int i = 0; i < attValues.Length; i++)
                        {
                            ISyndicationContent value = content.Fields.FirstOrDefault(f => f.Name == attValues[i]);

                            if (attValues[i] == "title"){
                                title = value.Value;
                            }
                            else if (attValues[i] == "description"){
                                description = WebUtility.HtmlDecode(value.Value);
                                description = Regex.Replace(description, "<.*?>", String.Empty);

                            }
                            else if (attValues[i] == "link"){
                                link = value.Value;
                            }
                            else if (attValues[i] == "category"){
                                category = value.Value;
                            }
                            else if (attValues[i] == "localstart"){
                                localstart = value.Value;
                            }
                            else if (attValues[i] == "localend"){
                                localend = value.Value;
                            }
                            else if (attValues[i] == "cdo-alldayevent"){
                                allday = Convert.ToBoolean(value.Value);
                            }
                        }

                        var customFields = content.Fields.Where(f => f.Name == "customfield");

                        for (int i = 0; i < custValues.Length; i++)
                        {
                            var customatt = CustomFieldValue(customFields, custValues[i]); {

                                if (custValues[i] == "Cost"){
                                    cost = customatt;
                                }
                                if (custValues[i] == "Age"){
                                    age = customatt;
                                }
                                if (custValues[i] == "Venue"){
                                    venue = customatt;
                                }
                                if (custValues[i] == "Venue address"){
                                    venueaddress = customatt;
                                }
                            }
                        }

                        RssContent.FillContent(title, description, link, category, localstart, localend, allday, cost, age, venue, venueaddress);

                        Console.WriteLine("Title: " + RssContent.Title + Environment.NewLine + "Description: " + RssContent.Description + 
                            Environment.NewLine + "Link: " + RssContent.Link + Environment.NewLine + "Start Date: " + RssContent.StartDate + 
                            Environment.NewLine + "Start DateTime: " + RssContent.StartDateTime + Environment.NewLine + "End DateTime: " + 
                            RssContent.EndDateTime + Environment.NewLine + "All Day: " + RssContent.AllDay + Environment.NewLine +  "Cost: " + 
                            RssContent.Cost +  Environment.NewLine + "Age: " + RssContent.Age + Environment.NewLine + "Venue: " + 
                            RssContent.Venue + Environment.NewLine + "Address: " + RssContent.VenueAddress + Environment.NewLine);
                    }
                }
            }

            string CustomFieldValue(IEnumerable<ISyndicationContent> customFields, string name)
            {
                var field = GetCustomFieldForName(customFields, name);
                return field == null ? string.Empty : field.Value;
            }

            ISyndicationContent GetCustomFieldForName(IEnumerable<ISyndicationContent> customFields, string name)
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
}