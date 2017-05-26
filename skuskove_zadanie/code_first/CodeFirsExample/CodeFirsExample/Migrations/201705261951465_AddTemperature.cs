namespace CodeFirsExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTemperature : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Temperatures",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.Double(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Temperatures", "UserID", "dbo.Users");
            DropIndex("dbo.Temperatures", new[] { "UserID" });
            DropTable("dbo.Temperatures");
        }
    }
}
