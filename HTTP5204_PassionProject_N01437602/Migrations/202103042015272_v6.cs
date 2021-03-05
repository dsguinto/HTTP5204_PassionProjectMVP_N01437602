namespace HTTP5204_PassionProject_N01437602.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ApplicationUser_UserName", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Products", new[] { "ApplicationUser_UserName" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropPrimaryKey("dbo.AspNetUserRoles");
            DropPrimaryKey("dbo.AspNetUsers");
            DropPrimaryKey("dbo.AspNetUserLogins");
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            AddPrimaryKey("dbo.AspNetUsers", "Id");
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.Products", "ApplicationUser_UserName");
            DropColumn("dbo.AspNetUsers", "UserFirstName");
            DropColumn("dbo.AspNetUsers", "UserLastName");
            DropColumn("dbo.AspNetUsers", "UserLocation");
            DropColumn("dbo.AspNetUsers", "UserBio");
            DropColumn("dbo.AspNetUsers", "UserHasPic");
            DropColumn("dbo.AspNetUsers", "UserPicExtension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "UserPicExtension", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.AspNetUsers", "UserBio", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserLocation", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserLastName", c => c.String());
            AddColumn("dbo.AspNetUsers", "UserFirstName", c => c.String());
            AddColumn("dbo.Products", "ApplicationUser_UserName", c => c.String(maxLength: 256));
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropPrimaryKey("dbo.AspNetUserLogins");
            DropPrimaryKey("dbo.AspNetUsers");
            DropPrimaryKey("dbo.AspNetUserRoles");
            AlterColumn("dbo.AspNetUserLogins", "UserId", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUserClaims", "UserId", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AspNetUsers", "Id", c => c.String());
            AlterColumn("dbo.AspNetUserRoles", "UserId", c => c.String(nullable: false, maxLength: 256));
            AddPrimaryKey("dbo.AspNetUserLogins", new[] { "LoginProvider", "ProviderKey", "UserId" });
            AddPrimaryKey("dbo.AspNetUsers", "UserName");
            AddPrimaryKey("dbo.AspNetUserRoles", new[] { "UserId", "RoleId" });
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.Products", "ApplicationUser_UserName");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "UserName", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "UserName", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "UserName", cascadeDelete: true);
            AddForeignKey("dbo.Products", "ApplicationUser_UserName", "dbo.AspNetUsers", "UserName");
        }
    }
}
