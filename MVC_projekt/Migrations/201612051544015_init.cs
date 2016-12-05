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
                        BookItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttachmentID)
                .ForeignKey("dbo.BookItems", t => t.BookItemID, cascadeDelete: true)
                .Index(t => t.BookItemID);
            
            CreateTable(
                "dbo.BookItems",
                c => new
                    {
                        BookItemID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ISBN = c.Long(nullable: false),
                        Number = c.Int(nullable: false),
                        Descryption = c.String(),
                        Publisher = c.String(),
                        ReleaseDate = c.Int(nullable: false),
                        AddDate = c.DateTime(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookItemID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.ISBN, unique: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.AuthorGroups",
                c => new
                    {
                        AuthorGroupID = c.Int(nullable: false, identity: true),
                        AuthorID = c.Int(nullable: false),
                        BookItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AuthorGroupID)
                .ForeignKey("dbo.Authors", t => t.AuthorID, cascadeDelete: true)
                .ForeignKey("dbo.BookItems", t => t.BookItemID, cascadeDelete: true)
                .Index(t => t.AuthorID)
                .Index(t => t.BookItemID);
            
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
                        ApplicationUserID = c.String(maxLength: 128),
                        BookItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.BookItems", t => t.BookItemID, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.BookItemID);
            
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
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(nullable: false),
                        AvailableOn = c.DateTime(nullable: false),
                        Returned = c.Boolean(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        BookItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.BookItems", t => t.BookItemID, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.BookItemID);
            
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
                "dbo.SearchResults",
                c => new
                    {
                        SearchResultID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SearchResultID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Parent_CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.Categories", t => t.Parent_CategoryID)
                .Index(t => t.Parent_CategoryID);
            
            CreateTable(
                "dbo.LabelGroups",
                c => new
                    {
                        LabelGroupID = c.Int(nullable: false, identity: true),
                        BookItemID = c.Int(nullable: false),
                        LabelID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LabelGroupID)
                .ForeignKey("dbo.BookItems", t => t.BookItemID, cascadeDelete: true)
                .ForeignKey("dbo.Labels", t => t.LabelID, cascadeDelete: true)
                .Index(t => t.BookItemID)
                .Index(t => t.LabelID);
            
            CreateTable(
                "dbo.Labels",
                c => new
                    {
                        LabelID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.LabelID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LabelGroups", "LabelID", "dbo.Labels");
            DropForeignKey("dbo.LabelGroups", "BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.Categories", "Parent_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.BookItems", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Bookings", "BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.SearchResults", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.Orders", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Fees", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bookings", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AuthorGroups", "BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.AuthorGroups", "AuthorID", "dbo.Authors");
            DropForeignKey("dbo.Attachments", "BookItemID", "dbo.BookItems");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LabelGroups", new[] { "LabelID" });
            DropIndex("dbo.LabelGroups", new[] { "BookItemID" });
            DropIndex("dbo.Categories", new[] { "Parent_CategoryID" });
            DropIndex("dbo.SearchResults", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "BookItemID" });
            DropIndex("dbo.Orders", new[] { "ApplicationUserID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Fees", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bookings", new[] { "BookItemID" });
            DropIndex("dbo.Bookings", new[] { "ApplicationUserID" });
            DropIndex("dbo.AuthorGroups", new[] { "BookItemID" });
            DropIndex("dbo.AuthorGroups", new[] { "AuthorID" });
            DropIndex("dbo.BookItems", new[] { "CategoryID" });
            DropIndex("dbo.BookItems", new[] { "ISBN" });
            DropIndex("dbo.Attachments", new[] { "BookItemID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Labels");
            DropTable("dbo.LabelGroups");
            DropTable("dbo.Categories");
            DropTable("dbo.SearchResults");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Orders");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Fees");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bookings");
            DropTable("dbo.Authors");
            DropTable("dbo.AuthorGroups");
            DropTable("dbo.BookItems");
            DropTable("dbo.Attachments");
        }
    }
}
