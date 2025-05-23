namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrVet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Vet_Consultations", "DoctorAvailability", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vet_Consultations", "DoctorAvailability");
        }
    }
}
