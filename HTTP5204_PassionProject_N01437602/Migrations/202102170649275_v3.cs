namespace HTTP5204_PassionProject_N01437602.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Carts", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Carts", "UserID");
            AddForeignKey("dbo.Carts", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "UserID", "dbo.Users");
            DropIndex("dbo.Carts", new[] { "UserID" });
            DropColumn("dbo.Carts", "UserID");
        }
    }
}
