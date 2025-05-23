namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        AmountPaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        AccountNumber = c.String(maxLength: 16),
                        CVV = c.String(maxLength: 3),
                        ExpiryDate = c.DateTime(nullable: false),
                        AccountHolder = c.String(maxLength: 100),
                        BankType = c.String(nullable: false, maxLength: 50),
                        ConsultId = c.Int(nullable: false),
                        PaymentStatus = c.Boolean(nullable: false),
                        TransactionReference = c.String(),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Vet_Consultations", t => t.ConsultId, cascadeDelete: true)
                .Index(t => t.ConsultId);
            
            AddColumn("dbo.Vet_Consultations", "PaymentStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "ConsultId", "dbo.Vet_Consultations");
            DropIndex("dbo.Payments", new[] { "ConsultId" });
            DropColumn("dbo.Vet_Consultations", "PaymentStatus");
            DropTable("dbo.Payments");
        }
    }
}
