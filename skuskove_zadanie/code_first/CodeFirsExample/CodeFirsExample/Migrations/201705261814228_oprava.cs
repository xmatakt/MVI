namespace CodeFirsExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class oprava : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDetails", "User_Login", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Login");
            AddForeignKey("dbo.UserDetails", "User_Login", "dbo.Users", "Login");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "User_Login", "dbo.Users");
            DropPrimaryKey("dbo.Users");
            AlterColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Users", "Login");
            AddForeignKey("dbo.UserDetails", "User_Login", "dbo.Users", "Login");
        }
    }
}
