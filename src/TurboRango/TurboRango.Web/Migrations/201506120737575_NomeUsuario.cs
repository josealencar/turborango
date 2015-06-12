namespace TurboRango.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NomeUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeedBacks", "Usuario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeedBacks", "Usuario");
        }
    }
}
