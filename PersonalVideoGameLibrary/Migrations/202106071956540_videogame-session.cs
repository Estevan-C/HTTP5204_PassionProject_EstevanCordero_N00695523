namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videogamesession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoGames", "SessionID", c => c.Int(nullable: false));
            CreateIndex("dbo.VideoGames", "SessionID");
            AddForeignKey("dbo.VideoGames", "SessionID", "dbo.Sessions", "SessionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoGames", "SessionID", "dbo.Sessions");
            DropIndex("dbo.VideoGames", new[] { "SessionID" });
            DropColumn("dbo.VideoGames", "SessionID");
        }
    }
}
