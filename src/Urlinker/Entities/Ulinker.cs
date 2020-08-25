using System;

namespace Urlinker.Entities
{
    public class Ulinker
    {
        public Guid id { get; set; }
        public string UrlShortName { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenUrl { get; set; }
        public DateTime createdDate { get; set; }
    }
}