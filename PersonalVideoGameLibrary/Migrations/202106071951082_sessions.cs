namespace PersonalVideoGameLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sessions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionID = c.Int(nullable: false, identity: true),
                        SessionMsg = c.String(),
                    })
                .PrimaryKey(t => t.SessionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Sessions");
        }
    }
}
