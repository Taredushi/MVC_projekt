namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class language : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookItems", "Number", c => c.Int(nullable: false));
            DropColumn("dbo.BookItems", "Amount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BookItems", "Amount", c => c.Int(nullable: false));
            DropColumn("dbo.BookItems", "Number");
        }
    }
}
