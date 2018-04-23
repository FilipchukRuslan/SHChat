namespace WebApplication5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyDbC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FriendRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        FromUser_Id = c.String(maxLength: 128),
                        ToUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FromUser_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ToUser_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.FromUser_Id)
                .Index(t => t.ToUser_Id);
            
            CreateTable(
                "dbo.FriendShips",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Friend_Id = c.String(maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Friend_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.Friend_Id)
                .Index(t => t.User_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FriendRequests", "ToUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendRequests", "FromUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendShips", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendShips", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendShips", "Friend_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.FriendRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.FriendShips", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.FriendShips", new[] { "User_Id" });
            DropIndex("dbo.FriendShips", new[] { "Friend_Id" });
            DropIndex("dbo.FriendRequests", new[] { "ToUser_Id" });
            DropIndex("dbo.FriendRequests", new[] { "FromUser_Id" });
            DropIndex("dbo.FriendRequests", new[] { "ApplicationUser_Id" });
            DropTable("dbo.FriendShips");
            DropTable("dbo.FriendRequests");
        }
    }
}
