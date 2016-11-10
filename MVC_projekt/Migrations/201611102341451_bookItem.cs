namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuthorGroups", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.LabelGroups", "BookItem_BookItemID", "dbo.BookItems");
            AddColumn("dbo.BookItems", "Amount", c => c.Int(nullable: false));
            AddColumn("dbo.AuthorGroups", "BookItem_BookItemID1", c => c.Int());
            AddColumn("dbo.LabelGroups", "BookItem_BookItemID1", c => c.Int());
            CreateIndex("dbo.AuthorGroups", "BookItem_BookItemID1");
            CreateIndex("dbo.LabelGroups", "BookItem_BookItemID1");
            AddForeignKey("dbo.AuthorGroups", "BookItem_BookItemID1", "dbo.BookItems", "BookItemID");
            AddForeignKey("dbo.LabelGroups", "BookItem_BookItemID1", "dbo.BookItems", "BookItemID");
            AddForeignKey("dbo.AuthorGroups", "BookItem_BookItemID", "dbo.BookItems", "BookItemID", cascadeDelete: true);
            AddForeignKey("dbo.LabelGroups", "BookItem_BookItemID", "dbo.BookItems", "BookItemID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabelGroups", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.AuthorGroups", "BookItem_BookItemID", "dbo.BookItems");
            DropForeignKey("dbo.LabelGroups", "BookItem_BookItemID1", "dbo.BookItems");
            DropForeignKey("dbo.AuthorGroups", "BookItem_BookItemID1", "dbo.BookItems");
            DropIndex("dbo.LabelGroups", new[] { "BookItem_BookItemID1" });
            DropIndex("dbo.AuthorGroups", new[] { "BookItem_BookItemID1" });
            DropColumn("dbo.LabelGroups", "BookItem_BookItemID1");
            DropColumn("dbo.AuthorGroups", "BookItem_BookItemID1");
            DropColumn("dbo.BookItems", "Amount");
            AddForeignKey("dbo.LabelGroups", "BookItem_BookItemID", "dbo.BookItems", "BookItemID");
            AddForeignKey("dbo.AuthorGroups", "BookItem_BookItemID", "dbo.BookItems", "BookItemID");
        }
    }
}
