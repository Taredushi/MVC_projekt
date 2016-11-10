namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookitem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookItems", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookItems", "Amount");
        }
    }
}
