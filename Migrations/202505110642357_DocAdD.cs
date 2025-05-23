namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocAdD : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Vet_Consultations", "DoctorId");
            AddForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Vet_Consultations", new[] { "DoctorId" });
        }
    }
}
