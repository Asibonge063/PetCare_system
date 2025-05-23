namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pet_Boarding", "Check_Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pet_Boarding", "Check_Status");
        }
    }
}
