namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatecolumnname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VideoGames", "VideoGameName", c => c.String());
            AddColumn("dbo.VideoGames", "VideoGamePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.VideoGames", "VideoGameHours", c => c.Int(nullable: false));
            DropColumn("dbo.VideoGames", "VgName");
            DropColumn("dbo.VideoGames", "VgPrice");
            DropColumn("dbo.VideoGames", "VgHours");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VideoGames", "VgHours", c => c.Int(nullable: false));
            AddColumn("dbo.VideoGames", "VgPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.VideoGames", "VgName", c => c.String());
            DropColumn("dbo.VideoGames", "VideoGameHours");
            DropColumn("dbo.VideoGames", "VideoGamePrice");
            DropColumn("dbo.VideoGames", "VideoGameName");
        }
    }
}
