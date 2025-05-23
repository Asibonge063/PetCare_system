namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainings",
                c => new
                    {
                        TrainingId = c.Int(nullable: false, identity: true),
                        PetId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        TrainingTypeId = c.Int(nullable: false),
                        TrainingDate = c.DateTime(nullable: false),
                        TrainingDuration = c.Int(nullable: false),
                        TrainingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TrainingLocation = c.String(),
                        TrainingStatus = c.String(nullable: false),
                        ProgressNotes = c.String(),
                        NextSessionDate = c.DateTime(),
                        IsGroupSession = c.Boolean(nullable: false),
                        PetBehaviorBefore = c.String(),
                        PetBehaviorAfter = c.String(),
                    })
                .PrimaryKey(t => t.TrainingId)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .ForeignKey("dbo.TrainingTypes", t => t.TrainingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.PetId)
                .Index(t => t.UserId)
                .Index(t => t.TrainingTypeId);
            
            CreateTable(
                "dbo.TrainingTypes",
                c => new
                    {
                        TrainingTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        DefaultDuration = c.Int(nullable: false),
                        DefaultCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TrainingTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainings", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Trainings", "TrainingTypeId", "dbo.TrainingTypes");
            DropForeignKey("dbo.Trainings", "PetId", "dbo.Pets");
            DropIndex("dbo.Trainings", new[] { "TrainingTypeId" });
            DropIndex("dbo.Trainings", new[] { "UserId" });
            DropIndex("dbo.Trainings", new[] { "PetId" });
            DropTable("dbo.TrainingTypes");
            DropTable("dbo.Trainings");
        }
    }
}
