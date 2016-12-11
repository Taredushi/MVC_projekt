namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class attachment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachments", "FileType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachments", "FileType");
        }
    }
}
