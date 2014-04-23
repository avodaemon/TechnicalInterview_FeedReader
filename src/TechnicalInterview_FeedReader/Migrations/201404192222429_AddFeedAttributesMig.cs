namespace TechnicalInterview_FeedReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeedAttributesMig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NewsFeeds", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.NewsFeeds", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.NewsFeeds", "Url", c => c.String(nullable: false));
            AlterColumn("dbo.NewsFeeds", "Color", c => c.String(nullable: false, maxLength: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NewsFeeds", "Color", c => c.String());
            AlterColumn("dbo.NewsFeeds", "Url", c => c.String());
            AlterColumn("dbo.NewsFeeds", "Description", c => c.String());
            AlterColumn("dbo.NewsFeeds", "Title", c => c.String());
        }
    }
}
