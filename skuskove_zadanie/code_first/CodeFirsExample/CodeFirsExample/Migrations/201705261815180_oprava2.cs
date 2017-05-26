namespace CodeFirsExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oprava2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDetails", "User_Login", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "User_Login" });
            DropColumn("dbo.UserDetails", "UserID");
            RenameColumn(table: "dbo.UserDetails", name: "User_Login", newName: "UserID");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.UserDetails", "UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Login", c => c.String());
            AlterColumn("dbo.Users", "UserID", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Users", "UserID");
            CreateIndex("dbo.UserDetails", "UserID");
            AddForeignKey("dbo.UserDetails", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "UserID", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.UserDetails", "UserID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.Users", "Login");
            RenameColumn(table: "dbo.UserDetails", name: "UserID", newName: "User_Login");
            AddColumn("dbo.UserDetails", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.UserDetails", "User_Login");
            AddForeignKey("dbo.UserDetails", "User_Login", "dbo.Users", "Login");
        }
    }
}
