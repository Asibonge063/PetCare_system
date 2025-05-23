namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdoptable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adoptions",
                c => new
                    {
                        AdoptionId = c.Int(nullable: false, identity: true),
                        ExperienceLevel = c.String(nullable: false),
                        HomeDescription = c.String(nullable: false, maxLength: 500),
                        AdoptionReason = c.String(nullable: false, maxLength: 1000),
                        Status = c.String(),
                        SubmittedDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        AdopterFullName = c.String(),
                        AdopterEmail = c.String(),
                        AdopterPhone = c.String(),
                        Id = c.Int(nullable: false),
                        PetName = c.String(),
                        PetType = c.String(),
                        PetBreed = c.String(),
                        PetDOB = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.AdoptionId)
                .ForeignKey("dbo.Pets", t => t.Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.Id);
            
            AddColumn("dbo.Pet_Adoption", "Id", c => c.Int(nullable: false));
            AddColumn("dbo.Pet_Adoption", "PetName", c => c.String());
            AddColumn("dbo.Pet_Adoption", "PetBreed", c => c.String());
            AddColumn("dbo.Pet_Adoption", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pet_Adoption", "AdopterFullName", c => c.String());
            AddColumn("dbo.Pet_Adoption", "AdopterEmail", c => c.String());
            AddColumn("dbo.Pet_Adoption", "AdopterPhone", c => c.String());
            AlterColumn("dbo.Pet_Adoption", "PetType", c => c.String());
            CreateIndex("dbo.Pet_Adoption", "Id");
            AddForeignKey("dbo.Pet_Adoption", "Id", "dbo.Pets", "Id", cascadeDelete: true);
            DropColumn("dbo.Pet_Adoption", "FullName");
            DropColumn("dbo.Pet_Adoption", "Email");
            DropColumn("dbo.Pet_Adoption", "PhoneNumber");
            DropColumn("dbo.Pet_Adoption", "Address");
            DropColumn("dbo.Pet_Adoption", "PetPrice");
            DropColumn("dbo.Pet_Adoption", "SpecificBreed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pet_Adoption", "SpecificBreed", c => c.String(maxLength: 50));
            AddColumn("dbo.Pet_Adoption", "PetPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Pet_Adoption", "Address", c => c.String(maxLength: 200));
            AddColumn("dbo.Pet_Adoption", "PhoneNumber", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Pet_Adoption", "Email", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Pet_Adoption", "FullName", c => c.String(nullable: false, maxLength: 100));
            DropForeignKey("dbo.Pet_Adoption", "Id", "dbo.Pets");
            DropForeignKey("dbo.Adoptions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Adoptions", "Id", "dbo.Pets");
            DropIndex("dbo.Pet_Adoption", new[] { "Id" });
            DropIndex("dbo.Adoptions", new[] { "Id" });
            DropIndex("dbo.Adoptions", new[] { "UserId" });
            AlterColumn("dbo.Pet_Adoption", "PetType", c => c.String(nullable: false));
            DropColumn("dbo.Pet_Adoption", "AdopterPhone");
            DropColumn("dbo.Pet_Adoption", "AdopterEmail");
            DropColumn("dbo.Pet_Adoption", "AdopterFullName");
            DropColumn("dbo.Pet_Adoption", "DateOfBirth");
            DropColumn("dbo.Pet_Adoption", "PetBreed");
            DropColumn("dbo.Pet_Adoption", "PetName");
            DropColumn("dbo.Pet_Adoption", "Id");
            DropTable("dbo.Adoptions");
        }
    }
}
