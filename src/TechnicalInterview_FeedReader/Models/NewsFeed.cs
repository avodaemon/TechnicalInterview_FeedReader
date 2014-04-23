using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Mvc;

namespace TechnicalInterview_FeedReader.Models
{
    public class NewsFeed
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "Feed URL", ShortName = "URL")]
        [Required]
        public string Url { get; set; }

        [DataType(DataType.ImageUrl)]
        [HiddenInput(DisplayValue = true)]
        public string ImageUrl { get; set; }

        [HiddenInput(DisplayValue = true)]
        public int UserId { get; set; }

        [StringLength(7, MinimumLength = 7)]
        [Display(Name = "Tag Color")]
        [Required]
        [HiddenInput(DisplayValue = true)]
        public string Color { get; set; }
    }

    public class NewsFeedDBContext : DbContext
    {
        public DbSet<NewsFeed> NewsFeeds { get; set; }
    }
}