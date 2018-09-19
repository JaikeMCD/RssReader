namespace RssImporter
{
    public class RssContent
    {
        public static string Title { get; set; }
        public static string Description { get; set; }
        public static string Link { get; set; }
        public static string StartDate { get; set; }
        public static string StartDateTime { get; set; }
        public static string EndDateTime { get; set; }
        public static bool AllDay { get; set; }
        public static string Cost { get; set; }
        public static string Age { get; set; }
        public static string Venue { get; set; }
        public static string VenueAddress { get; set; }

        public static void FillContent (string impTitle, string impdesc, string impLink, string impStart, string impStarttime,
            string impend, bool impAllday, string impCost, string impAge, string impVenue, string impAddress)
        {
            Title = impTitle;
            Description = impdesc;
            Link = impLink;
            StartDate = impStart;
            StartDateTime = impStarttime;
            EndDateTime = impend;
            AllDay = impAllday;
            Cost = impCost;
            Age = impAge;
            Venue = impVenue;
            VenueAddress = impAddress;
        }
    }
}