namespace BugBully.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BugBullyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bugs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        DateReported = c.DateTime(nullable: false),
                        StatusId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Statuses", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.StatusId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Statuses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bugs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Bugs", "StatusId", "dbo.Statuses");
            DropIndex("dbo.Bugs", new[] { "UserId" });
            DropIndex("dbo.Bugs", new[] { "StatusId" });
            DropTable("dbo.Users");
            DropTable("dbo.Statuses");
            DropTable("dbo.Bugs");
        }
    }
}
