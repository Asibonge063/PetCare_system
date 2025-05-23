namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        InventoryId = c.Int(nullable: false, identity: true),
                        MedicationName = c.String(),
                        InventoryType = c.Int(nullable: false),
                        InventoryQuantity = c.Int(nullable: false),
                        MedDiscription = c.String(),
                    })
                .PrimaryKey(t => t.InventoryId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Inventories");
        }
    }
}
