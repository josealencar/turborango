namespace TurboRango.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FeedBacks", "DataFeedBack", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeedBacks", "DataFeedBack", c => c.DateTime(nullable: false));
        }
    }
}
