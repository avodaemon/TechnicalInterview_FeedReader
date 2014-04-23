using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebMatrix.WebData;
using TechnicalInterview_FeedReader.Models;

namespace TechnicalInterview_FeedReader.Controllers
{
    [Authorize]
    public class NewsFeedsController : Controller
    {
        private NewsFeedDBContext db = new NewsFeedDBContext();

        //
        // GET: /NewsFeeds/

        public ActionResult Index()
        {
            var newsFeeds =
                from nf in db.NewsFeeds
                where nf.UserId == WebSecurity.CurrentUserId
                orderby nf.Title ascending
                select nf;
            return View(newsFeeds.ToList());
        }

        //
        // GET: /NewsFeeds/Create

        public ActionResult Create()
        {
            NewsFeed newsFeed = new NewsFeed();
            newsFeed.UserId = WebSecurity.CurrentUserId;
            newsFeed.Color = "#0000FF";
            return View(newsFeed);
        }

        //
        // POST: /NewsFeeds/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewsFeed newsfeed)
        {
            if (ModelState.IsValid)
            {
                db.NewsFeeds.Add(newsfeed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(newsfeed);
        }

        //
        // POST: /NewsFeeds/VerifyFeed

        [HttpPost]
        public ActionResult VerifyFeed(string url)
        {
            try
            {
                using (XmlReader reader = XmlReader.Create(url))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    string title = "";
                    if (feed.Title != null)
                    {
                        title = feed.Title.Text ?? "";
                    }

                    string description = "";
                    if (feed.Description != null)
                    {
                        description = feed.Description.Text ?? "";
                    }

                    string imageUrl = "";
                    if (feed.ImageUrl != null)
                    {
                        imageUrl = feed.ImageUrl.ToString();
                    }

                    return Json(new
                        {
                            Title = title,
                            Description = description,
                            ImageUrl = imageUrl
                        });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Result = "Invalid Feed URL" });
            }
        }

        //
        // GET: /NewsFeeds/Edit/5

        public ActionResult Edit(int id = 0)
        {
            NewsFeed newsfeed = db.NewsFeeds.Find(id);
            if (newsfeed == null)
            {
                return HttpNotFound();
            }
            else if (newsfeed.UserId != WebSecurity.CurrentUserId)
            {
                return RedirectToAction("Index");
            }
            return View(newsfeed);
        }

        //
        // POST: /NewsFeeds/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NewsFeed newsfeed)
        {
            if (ModelState.IsValid)
            {
                db.Entry(newsfeed).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newsfeed);
        }

        //
        // GET: /NewsFeeds/Delete/5

        public ActionResult Delete(int id = 0)
        {
            NewsFeed newsfeed = db.NewsFeeds.Find(id);
            if (newsfeed == null)
            {
                return HttpNotFound();
            }
            else if (newsfeed.UserId != WebSecurity.CurrentUserId)
            {
                return RedirectToAction("Index");
            }
            return View(newsfeed);
        }

        //
        // POST: /NewsFeeds/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NewsFeed newsfeed = db.NewsFeeds.Find(id);
            db.NewsFeeds.Remove(newsfeed);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}