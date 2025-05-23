namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrMembership : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Memberships",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        PetId = c.Int(nullable: false),
                        MembershipType = c.String(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Status = c.String(nullable: false),
                        PaymentAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LastRenewalDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pets", t => t.PetId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PetId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Memberships", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Memberships", "PetId", "dbo.Pets");
            DropIndex("dbo.Memberships", new[] { "PetId" });
            DropIndex("dbo.Memberships", new[] { "UserId" });
            DropTable("dbo.Memberships");
        }
    }
}
