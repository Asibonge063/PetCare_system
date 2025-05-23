namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StaffFK : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GroomingStaffs",
                c => new
                    {
                        GroomStaffId = c.Int(nullable: false, identity: true),
                        Groom_Name = c.String(nullable: false, maxLength: 50),
                        Groom_Surname = c.String(nullable: false, maxLength: 50),
                        Groom_Email = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.GroomStaffId);
            
            AddColumn("dbo.Spar_Grooming", "GroomId", c => c.Int());
            CreateIndex("dbo.Spar_Grooming", "GroomId");
            AddForeignKey("dbo.Spar_Grooming", "GroomId", "dbo.GroomingStaffs", "GroomStaffId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Spar_Grooming", "GroomId", "dbo.GroomingStaffs");
            DropIndex("dbo.Spar_Grooming", new[] { "GroomId" });
            DropColumn("dbo.Spar_Grooming", "GroomId");
            DropTable("dbo.GroomingStaffs");
        }
    }
}
