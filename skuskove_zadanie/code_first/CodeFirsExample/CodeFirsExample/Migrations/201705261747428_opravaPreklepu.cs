namespace CodeFirsExample.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class opravaPreklepu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsActive", c => c.Boolean());
            DropColumn("dbo.Users", "IsActice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "IsActice", c => c.Boolean());
            DropColumn("dbo.Users", "IsActive");
        }
    }
}
