namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sam : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Spar_Grooming", "GroomId", "dbo.GroomingStaffs");
            DropIndex("dbo.Spar_Grooming", new[] { "GroomId" });
            DropColumn("dbo.Spar_Grooming", "GroomId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Spar_Grooming", "GroomId", c => c.Int());
            CreateIndex("dbo.Spar_Grooming", "GroomId");
            AddForeignKey("dbo.Spar_Grooming", "GroomId", "dbo.GroomingStaffs", "GroomStaffId");
        }
    }
}
