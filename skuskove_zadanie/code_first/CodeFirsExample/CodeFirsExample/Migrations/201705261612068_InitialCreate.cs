namespace CodeFirsExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        Email = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        pwdHash = c.String(),
                        UserType = c.Int(nullable: false),
                        IsActice = c.Boolean(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropTable("dbo.Users");
            DropTable("dbo.UserDetails");
        }
    }
}
