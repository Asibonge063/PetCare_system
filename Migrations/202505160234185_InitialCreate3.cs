namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PaymentForAdoptions", "AccountNumber", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.PaymentForBoards", "AccountNumber", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Payments", "AccountNumber", c => c.String(nullable: false, maxLength: 16));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "AccountNumber", c => c.String(maxLength: 16));
            AlterColumn("dbo.PaymentForBoards", "AccountNumber", c => c.String(maxLength: 16));
            AlterColumn("dbo.PaymentForAdoptions", "AccountNumber", c => c.String(maxLength: 16));
        }
    }
}
