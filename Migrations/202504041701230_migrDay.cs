namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrDay : DbMigration
    {
      
           public override void Up()
        {
            CreateTable(
                "dbo.DayCares",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MembershipId = c.Int(nullable: false),
                    PetId = c.Int(nullable: false),
                    UserId = c.String(nullable: false, maxLength: 128),
                    CareType = c.String(nullable: false),
                    CheckInDate = c.DateTime(nullable: false),
                    CheckOutDate = c.DateTime(),
                    SpecialInstructions = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Memberships", t => t.MembershipId, cascadeDelete: true)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: false) // Changed to no cascade
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false) // Changed to no cascade
                .Index(t => t.MembershipId)
                .Index(t => t.PetId)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.DayCares", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DayCares", "PetId", "dbo.Pets");
           
            DropIndex("dbo.DayCares", new[] { "UserId" });
            DropIndex("dbo.DayCares", new[] { "PetId" });
            DropIndex("dbo.DayCares", new[] { "MembershipId" });
            DropTable("dbo.DayCares");
        }
    }
}
