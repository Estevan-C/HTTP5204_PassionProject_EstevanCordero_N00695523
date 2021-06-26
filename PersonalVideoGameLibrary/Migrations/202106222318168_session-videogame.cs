namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessionvideogame : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.VideoGames", "SessionID", "dbo.Sessions");
            DropIndex("dbo.VideoGames", new[] { "SessionID" });
            AddColumn("dbo.Sessions", "VideoGameID", c => c.Int(nullable: false));
            CreateIndex("dbo.Sessions", "VideoGameID");
            AddForeignKey("dbo.Sessions", "VideoGameID", "dbo.VideoGames", "VideoGameID", cascadeDelete: true);
            DropColumn("dbo.VideoGames", "SessionID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VideoGames", "SessionID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Sessions", "VideoGameID", "dbo.VideoGames");
            DropIndex("dbo.Sessions", new[] { "VideoGameID" });
            DropColumn("dbo.Sessions", "VideoGameID");
            CreateIndex("dbo.VideoGames", "SessionID");
            AddForeignKey("dbo.VideoGames", "SessionID", "dbo.Sessions", "SessionID", cascadeDelete: true);
        }
    }
}
