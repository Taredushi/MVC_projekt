namespace MVC_projekt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class search : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SearchResults", name: "Account_Id", newName: "ApplicationUserID");
            RenameIndex(table: "dbo.SearchResults", name: "IX_Account_Id", newName: "IX_ApplicationUserID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.SearchResults", name: "IX_ApplicationUserID", newName: "IX_Account_Id");
            RenameColumn(table: "dbo.SearchResults", name: "ApplicationUserID", newName: "Account_Id");
        }
    }
}
