namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class consolesvideogames : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoGamesConsoles",
                c => new
                    {
                        VideoGames_VideoGameID = c.Int(nullable: false),
                        Consoles_ConsoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VideoGames_VideoGameID, t.Consoles_ConsoleID })
                .ForeignKey("dbo.VideoGames", t => t.VideoGames_VideoGameID, cascadeDelete: true)
                .ForeignKey("dbo.Consoles", t => t.Consoles_ConsoleID, cascadeDelete: true)
                .Index(t => t.VideoGames_VideoGameID)
                .Index(t => t.Consoles_ConsoleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VideoGamesConsoles", "Consoles_ConsoleID", "dbo.Consoles");
            DropForeignKey("dbo.VideoGamesConsoles", "VideoGames_VideoGameID", "dbo.VideoGames");
            DropIndex("dbo.VideoGamesConsoles", new[] { "Consoles_ConsoleID" });
            DropIndex("dbo.VideoGamesConsoles", new[] { "VideoGames_VideoGameID" });
            DropTable("dbo.VideoGamesConsoles");
        }
    }
}
