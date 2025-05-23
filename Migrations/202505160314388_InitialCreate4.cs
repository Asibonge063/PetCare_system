namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Spar_Grooming", name: "GroomId", newName: "GroomingStaff_GroomStaffId");
            RenameIndex(table: "dbo.Spar_Grooming", name: "IX_GroomId", newName: "IX_GroomingStaff_GroomStaffId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Spar_Grooming", name: "IX_GroomingStaff_GroomStaffId", newName: "IX_GroomId");
            RenameColumn(table: "dbo.Spar_Grooming", name: "GroomingStaff_GroomStaffId", newName: "GroomId");
        }
    }
}
