namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migrPayment : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Payments", "ExpiryDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Payments", "ExpiryDate", c => c.DateTime(nullable: false));
        }
    }
}
