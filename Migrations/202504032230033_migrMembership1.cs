namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrMembership1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Memberships", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Memberships", new[] { "UserId" });
            AddColumn("dbo.Memberships", "PaymentMethod", c => c.String(nullable: false));
            AddColumn("dbo.Memberships", "PaymentStatus", c => c.String());
            AddColumn("dbo.Memberships", "PaymentDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Memberships", "CancellationDate", c => c.DateTime());
            AddColumn("dbo.Memberships", "AccountNumber", c => c.String());
            AddColumn("dbo.Memberships", "CVV", c => c.String());
            AddColumn("dbo.Memberships", "ExpiryDate", c => c.DateTime());
            AddColumn("dbo.Memberships", "AccountHolder", c => c.String());
            AddColumn("dbo.Memberships", "BankType", c => c.String());
            AddColumn("dbo.Memberships", "PreviousMembershipId", c => c.Int());
            AlterColumn("dbo.Memberships", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Memberships", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Memberships", "Status", c => c.String(nullable: false));
            AlterColumn("dbo.Memberships", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Memberships", "PreviousMembershipId");
            DropColumn("dbo.Memberships", "BankType");
            DropColumn("dbo.Memberships", "AccountHolder");
            DropColumn("dbo.Memberships", "ExpiryDate");
            DropColumn("dbo.Memberships", "CVV");
            DropColumn("dbo.Memberships", "AccountNumber");
            DropColumn("dbo.Memberships", "CancellationDate");
            DropColumn("dbo.Memberships", "PaymentDate");
            DropColumn("dbo.Memberships", "PaymentStatus");
            DropColumn("dbo.Memberships", "PaymentMethod");
            CreateIndex("dbo.Memberships", "UserId");
            AddForeignKey("dbo.Memberships", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
