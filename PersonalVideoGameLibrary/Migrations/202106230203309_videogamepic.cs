namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videogamepic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoGames", "VideoGameHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.VideoGames", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.VideoGames", "PicExtension");
            DropColumn("dbo.VideoGames", "VideoGameHasPic");
        }
    }
}
