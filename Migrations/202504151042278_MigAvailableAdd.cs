namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigAvailableAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Doctors", "AvailabilityStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Doctors", "AvailabilityStatus");
        }
    }
}
