namespace HTTP5204_PassionProject_N01437602.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "UserPicExtension", c => c.String());
            DropColumn("dbo.Users", "PlayerHasPic");
            DropColumn("dbo.Users", "PlayerPicExtentsion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PlayerPicExtentsion", c => c.String());
            AddColumn("dbo.Users", "PlayerHasPic", c => c.Boolean(nullable: false));
            DropColumn("dbo.Users", "UserPicExtension");
            DropColumn("dbo.Users", "UserHasPic");
        }
    }
}
