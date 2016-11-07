namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        AttachmentID = c.Int(nullable: false, identity: true),
                        Source = c.String(),
                        Descryption = c.String(),
                        BookItem_BookItemID = c.Int(),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.BookItems", t => t.BookItem_BookItemID)
                .Index(t => t.BookItem_BookItemID);
            
            CreateTable(
                "dbo.BookItems",
                c => new
                    {
                        BookItemID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ISBN = c.Int(nullable: false),
                        Descryption = c.String(),
                        Publisher = c.String(),
                        ReleaseDate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookItemID)
                .Index(t => t.ISBN, unique: true);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false),
                        ParentID = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.BookItems", t => t.CategoryID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.AuthorGroups",
                c => new
                    {
                        AuthorGroupID = c.Int(nullable: false, identity: true),
                        Author_AuthorID = c.Int(),
                        BookItem_BookItemID = c.Int(),
                    })
                .PrimaryKey(t => t.AuthorGroupID)
                .ForeignKey("dbo.Authors", t => t.Author_AuthorID)
                .ForeignKey("dbo.BookItems", t => t.BookItem_BookItemID)
                .Index(t => t.Author_AuthorID)
                .Index(t => t.BookItem_BookItemID);
            
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        AuthorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                    })
                .PrimaryKey(t => t.AuthorID);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Book_BookID = c.Int(),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Books", t => t.Book_BookID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Book_BookID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        BookItem_BookItemID = c.Int(),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.BookItems", t => t.BookItem_BookItemID)
                .Index(t => t.BookItem_BookItemID);
            
            CreateTable(
                "dbo.Fees",
                c => new
                    {
                        FeeID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FeeID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.LabelGroups",
                c => new
                    {
                        LabelGroupID = c.Int(nullable: false, identity: true),
                        BookItem_BookItemID = c.Int(),
                        Label_LabelID = c.Int(),
                    })
                .PrimaryKey(t => t.LabelGroupID)
                .ForeignKey("dbo.BookItems", t => t.BookItem_BookItemID)
                .ForeignKey("dbo.Labels", t => t.Label_LabelID)
                .Index(t => t.BookItem_BookItemID)
                .Index(t => t.Label_LabelID);
            
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        LabelID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LabelID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Book_BookID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Books", t => t.Book_BookID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Book_BookID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SearchResults",
                c => new
                    {
                        SearchResultID = c.Int(nullable: false, identity: true),
                        Account_Id = c.String(maxLength: 128),
                        BookItem_BookItemID = c.Int(),
                    })
                .PrimaryKey(t => t.SearchResultID)
                .ForeignKey("dbo.AspNetUsers", t => t.Account_Id)
                .ForeignKey("dbo.BookItems", t => t.BookItem_BookItemID)
                .Index(t => t.Account_Id)
                .Index(t => t.BookItem_BookItemID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SearchResults", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.SearchResults", "Account_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Orders", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LabelGroups", "Label_LabelID", "dbo.Labels");
            DropForeignKey("dbo.LabelGroups", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.Fees", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "Book_BookID", "dbo.Books");
            DropForeignKey("dbo.Books", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.Bookings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuthorGroups", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.AuthorGroups", "Author_AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Attachments", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.Categories", "CategoryID", "dbo.BookItems");
            DropIndex("dbo.SearchResults", new[] { "BookItem_BookItemID" });
            DropIndex("dbo.SearchResults", new[] { "Account_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Orders", new[] { "Book_BookID" });
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.LabelGroups", new[] { "Label_LabelID" });
            DropIndex("dbo.LabelGroups", new[] { "BookItem_BookItemID" });
            DropIndex("dbo.Fees", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Books", new[] { "BookItem_BookItemID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bookings", new[] { "Book_BookID" });
            DropIndex("dbo.Bookings", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AuthorGroups", new[] { "BookItem_BookItemID" });
            DropIndex("dbo.AuthorGroups", new[] { "Author_AuthorID" });
            DropIndex("dbo.Categories", new[] { "CategoryID" });
            DropIndex("dbo.BookItems", new[] { "ISBN" });
            DropIndex("dbo.Attachments", new[] { "BookItem_BookItemID" });
            DropTable("dbo.SearchResults");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Orders");
            DropTable("dbo.Labels");
            DropTable("dbo.LabelGroups");
            DropTable("dbo.Fees");
            DropTable("dbo.Books");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bookings");
            DropTable("dbo.Authors");
            DropTable("dbo.AuthorGroups");
            DropTable("dbo.Categories");
            DropTable("dbo.BookItems");
            DropTable("dbo.Attachments");
        }
    }
}
