namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigAvailableRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Doctors", "AvailabilityStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Doctors", "AvailabilityStatus", c => c.String());
        }
    }
}
