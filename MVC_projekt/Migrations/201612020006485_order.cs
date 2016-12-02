namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class order : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.Bookings", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.Books", "BookItem_BookItemID", "dbo.BookItems");
            DropIndex("dbo.Books", new[] { "BookItem_BookItemID" });
            DropIndex("dbo.Bookings", new[] { "Book_BookID" });
            DropIndex("dbo.Orders", new[] { "Book_BookID" });
            RenameColumn(table: "dbo.Bookings", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "ApplicationUserID");
            RenameIndex(table: "dbo.Bookings", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserID");
            RenameIndex(table: "dbo.Orders", name: "IX_ApplicationUser_Id", newName: "IX_ApplicationUserID");
            AddColumn("dbo.Bookings", "BookItemID", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Returned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "BookItemID", c => c.Int(nullable: false));
            CreateIndex("dbo.Bookings", "BookItemID");
            CreateIndex("dbo.Orders", "BookItemID");
            AddForeignKey("dbo.Orders", "BookItemID", "dbo.BookItems", "BookItemID", cascadeDelete: true);
            AddForeignKey("dbo.Bookings", "BookItemID", "dbo.BookItems", "BookItemID", cascadeDelete: true);
            DropColumn("dbo.Bookings", "Book_BookID");
            DropColumn("dbo.Orders", "Book_BookID");
            DropTable("dbo.Books");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Returned = c.Boolean(nullable: false),
                        BookItem_BookItemID = c.Int(),
                    })
                .PrimaryKey(t => t.BookID);
            
            AddColumn("dbo.Orders", "Book_BookID", c => c.Int());
            AddColumn("dbo.Bookings", "Book_BookID", c => c.Int());
            DropForeignKey("dbo.Bookings", "BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.Orders", "BookItemID", "dbo.BookItems");
            DropIndex("dbo.Orders", new[] { "BookItemID" });
            DropIndex("dbo.Bookings", new[] { "BookItemID" });
            DropColumn("dbo.Orders", "BookItemID");
            DropColumn("dbo.Orders", "Returned");
            DropColumn("dbo.Bookings", "BookItemID");
            RenameIndex(table: "dbo.Orders", name: "IX_ApplicationUserID", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.Bookings", name: "IX_ApplicationUserID", newName: "IX_ApplicationUser_Id");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Bookings", name: "ApplicationUserID", newName: "ApplicationUser_Id");
            CreateIndex("dbo.Orders", "Book_BookID");
            CreateIndex("dbo.Bookings", "Book_BookID");
            CreateIndex("dbo.Books", "BookItem_BookItemID");
            AddForeignKey("dbo.Books", "BookItem_BookItemID", "dbo.BookItems", "BookItemID");
            AddForeignKey("dbo.Bookings", "Book_BookID", "dbo.Books", "BookID");
            AddForeignKey("dbo.Orders", "Book_BookID", "dbo.Books", "BookID");
        }
    }
}
