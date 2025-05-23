namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PetMigr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(nullable: false),
                        Breed = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        PicturePath = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "FirstName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "LastName", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "CellphoneNumber", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "IdNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pets", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Pets", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "IdNumber");
            DropColumn("dbo.AspNetUsers", "CellphoneNumber");
            DropColumn("dbo.AspNetUsers", "LastName");
            DropColumn("dbo.AspNetUsers", "FirstName");
            DropTable("dbo.Pets");
        }
    }
}
