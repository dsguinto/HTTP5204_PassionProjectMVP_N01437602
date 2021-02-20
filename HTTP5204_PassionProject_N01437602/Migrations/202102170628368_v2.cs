namespace HTTP5204_PassionProject_N01437602.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "CartID", "dbo.Carts");
            DropIndex("dbo.Users", new[] { "CartID" });
            DropColumn("dbo.Users", "CartID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "CartID", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "CartID");
            AddForeignKey("dbo.Users", "CartID", "dbo.Carts", "CartID", cascadeDelete: true);
        }
    }
}
