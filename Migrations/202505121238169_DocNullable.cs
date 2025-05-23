namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DocNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Vet_Consultations", new[] { "DoctorId" });
            AlterColumn("dbo.Vet_Consultations", "DoctorId", c => c.Int());
            CreateIndex("dbo.Vet_Consultations", "DoctorId");
            AddForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors");
            DropIndex("dbo.Vet_Consultations", new[] { "DoctorId" });
            AlterColumn("dbo.Vet_Consultations", "DoctorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Vet_Consultations", "DoctorId");
            AddForeignKey("dbo.Vet_Consultations", "DoctorId", "dbo.Doctors", "Id", cascadeDelete: true);
        }
    }
}
