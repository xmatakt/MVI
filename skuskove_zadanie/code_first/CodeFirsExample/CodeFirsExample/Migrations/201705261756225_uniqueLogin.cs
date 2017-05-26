namespace CodeFirsExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniqueLogin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropPrimaryKey("dbo.Users");
            AddColumn("dbo.UserDetails", "User_Login", c => c.String(maxLength: 128));
            AlterColumn("dbo.Users", "UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Login");
            CreateIndex("dbo.UserDetails", "User_Login");
            AddForeignKey("dbo.UserDetails", "User_Login", "dbo.Users", "Login");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "User_Login", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "User_Login" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Login", c => c.String());
            AlterColumn("dbo.Users", "UserID", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.UserDetails", "User_Login");
            AddPrimaryKey("dbo.Users", "UserID");
            CreateIndex("dbo.UserDetails", "UserID");
            AddForeignKey("dbo.UserDetails", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
    }
}
