using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechnicalInterview_FeedReader.Models
{
    public class NewsItem
    {
        public NewsFeed NewsFeed { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}