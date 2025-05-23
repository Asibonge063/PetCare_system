namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocFkTestRm : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Vet_Consultations", new[] { "DoctorId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Vet_Consultations", "DoctorId");
            AddForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
        }
    }
}
