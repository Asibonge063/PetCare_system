namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vet_Consultations", "InjectionAndVaccination", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vet_Consultations", "Neutering", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vet_Consultations", "GeneralCheckUp", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vet_Consultations", "Deworming", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vet_Consultations", "PregnancyOrUltraSound", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vet_Consultations", "AllergyTest", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vet_Consultations", "AllergyTest");
            DropColumn("dbo.Vet_Consultations", "PregnancyOrUltraSound");
            DropColumn("dbo.Vet_Consultations", "Deworming");
            DropColumn("dbo.Vet_Consultations", "GeneralCheckUp");
            DropColumn("dbo.Vet_Consultations", "Neutering");
            DropColumn("dbo.Vet_Consultations", "InjectionAndVaccination");
        }
    }
}
