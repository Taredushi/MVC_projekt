namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pierwsza : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "Book_BookID", c => c.Int());
            AddColumn("dbo.Orders", "Book_BookID", c => c.Int());
            CreateIndex("dbo.Bookings", "Book_BookID");
            CreateIndex("dbo.Orders", "Book_BookID");
            AddForeignKey("dbo.Bookings", "Book_BookID", "dbo.Books", "BookID");
            AddForeignKey("dbo.Orders", "Book_BookID", "dbo.Books", "BookID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.Bookings", "Book_BookID", "dbo.Books");
            DropIndex("dbo.Orders", new[] { "Book_BookID" });
            DropIndex("dbo.Bookings", new[] { "Book_BookID" });
            DropColumn("dbo.Orders", "Book_BookID");
            DropColumn("dbo.Bookings", "Book_BookID");
        }
    }
}
