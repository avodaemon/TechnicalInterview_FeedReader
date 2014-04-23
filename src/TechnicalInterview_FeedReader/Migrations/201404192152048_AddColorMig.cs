namespace TechnicalInterview_FeedReader.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColorMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NewsFeeds", "Color", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NewsFeeds", "Color");
        }
    }
}
