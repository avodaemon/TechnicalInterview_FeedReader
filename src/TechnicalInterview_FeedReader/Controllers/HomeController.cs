using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebMatrix.WebData;
using TechnicalInterview_FeedReader.Models;

namespace TechnicalInterview_FeedReader.Controllers
{
    public class HomeController : Controller
    {
        private NewsFeedDBContext db = new NewsFeedDBContext();

        [Authorize]
        public ActionResult Index(string feedName, string searchString)
        {
            ViewBag.Message = "Find out whatsUP";

            // Load the list of Feed Names for filter support
            var feedNameLst = new List<string>();

            var feedNameQry =
                from nf in db.NewsFeeds
                where nf.UserId == WebSecurity.CurrentUserId
                orderby nf.Title ascending
                select nf.Title;
            feedNameLst.AddRange(feedNameQry.Distinct());
            ViewBag.feedName = new SelectList(feedNameLst);

            // Load this user's feeds and latest feed items
            var newsFeeds =
               from nf in db.NewsFeeds
               where nf.UserId == WebSecurity.CurrentUserId
               orderby nf.Title ascending
               select nf;

            List<NewsItem> items = new List<NewsItem>();
            foreach (NewsFeed nf in newsFeeds)
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(nf.Url))
                    {
                        int counter = 0;
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        foreach (SyndicationItem item in feed.Items)
                        {
                            NewsItem i = new NewsItem();
                            i.NewsFeed = nf;
                            i.Title = item.Title.Text;
                            i.Summary = item.Summary.Text;
                            i.PublishDate = item.PublishDate;
                            items.Add(i);

                            // Only show the most recent 10 items for now from
                            // each feed to prevent a massive list being returned
                            counter++;
                            if (counter == 10)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }

            var newsItems = from ni in items
                            select ni;
            if (!String.IsNullOrEmpty(searchString))
            {
                // Split the search string in case they entered multiple
                // words and require all words to be in the item
                string[] searchStrings = searchString.Split(' ');
                newsItems = newsItems.Where(
                    s => searchStrings.All(
                        t => (s.Title.ToUpper().Contains(t.ToUpper()) 
                            || s.Summary.ToUpper().Contains(t.ToUpper())
                        )
                    )
                );
            }
            if (!String.IsNullOrEmpty(feedName))
            {
                newsItems = newsItems.Where(s => s.NewsFeed.Title == feedName);
            }
            newsItems = newsItems.OrderByDescending(s => s.PublishDate);

            return View(newsItems);
        }
    }
}
