using System;
using System.IO;

namespace RssImporter
{
    public class RssContent
    {

        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string startDate { get; set; }
        public string startDateTime { get; set; }
        public string endDateTime { get; set; }
        public bool allDay { get; set; }
        public string cost { get; set; }
        public string age { get; set; }
        public string venue { get; set; }
        public string venueAddress { get; set; }

    }
}