namespace HTTP5204_PassionProject_N01437602.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductPicExtension", c => c.String());
            DropColumn("dbo.Products", "ProductPicExtentsion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ProductPicExtentsion", c => c.String());
            DropColumn("dbo.Products", "ProductPicExtension");
        }
    }
}
