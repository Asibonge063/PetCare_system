namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigAvailable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "AvailabilityStatus", c => c.String());
            DropColumn("dbo.Doctors", "IsAvailable");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "IsAvailable", c => c.Boolean(nullable: false));
            DropColumn("dbo.Doctors", "AvailabilityStatus");
        }
    }
}
