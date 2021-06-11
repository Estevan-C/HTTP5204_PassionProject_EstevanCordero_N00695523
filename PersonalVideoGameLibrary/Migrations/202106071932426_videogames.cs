namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videogames : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VideoGames",
                c => new
                    {
                        VideoGameID = c.Int(nullable: false, identity: true),
                        VgName = c.String(),
                        VgPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VgHours = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VideoGameID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VideoGames");
        }
    }
}
