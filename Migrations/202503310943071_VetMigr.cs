namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VetMigr : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vet_Consultations",
                c => new
                    {
                        Consult_Id = c.Int(nullable: false, identity: true),
                        Consult_Description = c.String(nullable: false, maxLength: 250),
                        Consult_Date = c.DateTime(nullable: false),
                        Consult_Time = c.DateTime(nullable: false),
                        HasSignsOfIllness = c.Boolean(nullable: false),
                        IsOnMedication = c.Boolean(nullable: false),
                        IsVaccinated = c.Boolean(nullable: false),
                        HasChangedEatingHabits = c.Boolean(nullable: false),
                        HasUnusualBehaviors = c.Boolean(nullable: false),
                        IsDewormed = c.Boolean(nullable: false),
                        PicturePath = c.String(),
                        PetId = c.Int(nullable: false),
                        PetName = c.String(),
                        PetType = c.String(),
                        PetBreed = c.String(),
                        PetDateOfBirth = c.DateTime(nullable: false),
                        PetPicturePath = c.String(),
                        ConsultationType = c.String(nullable: false),
                        Feedback = c.String(),
                    })
                .PrimaryKey(t => t.Consult_Id)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .Index(t => t.PetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vet_Consultations", "PetId", "dbo.Pets");
            DropIndex("dbo.Vet_Consultations", new[] { "PetId" });
            DropTable("dbo.Vet_Consultations");
        }
    }
}
