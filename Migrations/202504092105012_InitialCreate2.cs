namespace PetCare_system.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingSpars",
                c => new
                    {
                        BookingSparId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false),
                        SpaServiceId = c.Int(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        PetName = c.String(nullable: false),
                        PetType = c.String(nullable: false),
                        SpecialInstructions = c.String(),
                        IsPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BookingSparId)
                .ForeignKey("dbo.SpaServices", t => t.SpaServiceId, cascadeDelete: true)
                .Index(t => t.SpaServiceId);
            
            CreateTable(
                "dbo.SpaServices",
                c => new
                    {
                        SpaServiceId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DurationMinutes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SpaServiceId);
            
            CreateTable(
                "dbo.PaymentForAdoptions",
                c => new
                    {
                        Payment_Id = c.Int(nullable: false, identity: true),
                        AdoptionId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PetName = c.String(),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        BankType = c.String(nullable: false, maxLength: 50),
                        CardHolderName = c.String(maxLength: 100),
                        AccountNumber = c.String(maxLength: 16),
                        CVV = c.String(maxLength: 3),
                        ExpiryDate = c.DateTime(),
                        Status = c.String(nullable: false, maxLength: 20),
                        TransactionId = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Payment_Id)
                .ForeignKey("dbo.Pet_Adoption", t => t.AdoptionId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.AdoptionId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Pet_Adoption",
                c => new
                    {
                        Adoption_Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 100),
                        PhoneNumber = c.String(nullable: false, maxLength: 20),
                        Address = c.String(maxLength: 200),
                        PetType = c.String(nullable: false),
                        PetPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpecificBreed = c.String(maxLength: 50),
                        ExperienceLevel = c.String(nullable: false),
                        HomeDescription = c.String(nullable: false, maxLength: 500),
                        AdoptionReason = c.String(nullable: false, maxLength: 1000),
                        Status = c.String(),
                        HasAgreedToTerms = c.Boolean(nullable: false),
                        ApplicationDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Adoption_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.PaymentForBoards",
                c => new
                    {
                        Payment_boardId = c.Int(nullable: false, identity: true),
                        BoardingId = c.Int(nullable: false),
                        AmountPaid = c.Decimal(nullable: false, storeType: "money"),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(nullable: false, maxLength: 50),
                        BankType = c.String(nullable: false, maxLength: 50),
                        CardHolderName = c.String(maxLength: 100),
                        AccountNumber = c.String(maxLength: 16),
                        CVV = c.String(maxLength: 3),
                        ExpiryDate = c.DateTime(),
                        Status = c.String(nullable: false, maxLength: 20),
                        TransactionId = c.String(maxLength: 100),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Payment_boardId)
                .ForeignKey("dbo.Pet_Boarding", t => t.BoardingId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.BoardingId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Pet_Boarding",
                c => new
                    {
                        board_Id = c.Int(nullable: false, identity: true),
                        OwnerName = c.String(nullable: false),
                        PetName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        PetType = c.String(nullable: false),
                        PetBreed = c.String(),
                        CheckInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        BookingDate = c.DateTime(nullable: false),
                        Package = c.String(nullable: false),
                        SpecialNeeds = c.String(),
                        Agreement = c.Boolean(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.board_Id);
            
            CreateTable(
                "dbo.PaymentForSpars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookingId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentDate = c.DateTime(nullable: false),
                        PaymentMethod = c.String(nullable: false),
                        TransactionId = c.String(),
                        Booking_BookingSparId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BookingSpars", t => t.Booking_BookingSparId)
                .Index(t => t.Booking_BookingSparId);
            
            CreateTable(
                "dbo.Spar_Grooming",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        OwnerName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        PetName = c.String(nullable: false, maxLength: 50),
                        PetType = c.Int(nullable: false),
                        Breed = c.String(nullable: false, maxLength: 50),
                        ServiceType = c.Int(nullable: false),
                        AddOnServices = c.Int(nullable: false),
                        DurationHours = c.Double(nullable: false),
                        PreferredDate = c.DateTime(nullable: false),
                        PreferredTime = c.String(nullable: false),
                        SpecialInstructions = c.String(maxLength: 500),
                        BookingDate = c.DateTime(nullable: false),
                        PaymentStatus = c.Int(nullable: false),
                        PaymentDate = c.DateTime(),
                        PaymentMethod = c.Int(),
                        TransactionId = c.String(maxLength: 50),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId);
            
            CreateTable(
                "dbo.Spar_GroomPayment",
                c => new
                    {
                        BookingId = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CardNumber = c.String(nullable: false),
                        ExpiryDate = c.String(nullable: false),
                        CVV = c.String(nullable: false),
                        CardName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BookingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentForSpars", "Booking_BookingSparId", "dbo.BookingSpars");
            DropForeignKey("dbo.PaymentForBoards", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentForBoards", "BoardingId", "dbo.Pet_Boarding");
            DropForeignKey("dbo.PaymentForAdoptions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentForAdoptions", "AdoptionId", "dbo.Pet_Adoption");
            DropForeignKey("dbo.Pet_Adoption", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BookingSpars", "SpaServiceId", "dbo.SpaServices");
            DropIndex("dbo.PaymentForSpars", new[] { "Booking_BookingSparId" });
            DropIndex("dbo.PaymentForBoards", new[] { "UserId" });
            DropIndex("dbo.PaymentForBoards", new[] { "BoardingId" });
            DropIndex("dbo.Pet_Adoption", new[] { "UserId" });
            DropIndex("dbo.PaymentForAdoptions", new[] { "UserId" });
            DropIndex("dbo.PaymentForAdoptions", new[] { "AdoptionId" });
            DropIndex("dbo.BookingSpars", new[] { "SpaServiceId" });
            DropTable("dbo.Spar_GroomPayment");
            DropTable("dbo.Spar_Grooming");
            DropTable("dbo.PaymentForSpars");
            DropTable("dbo.Pet_Boarding");
            DropTable("dbo.PaymentForBoards");
            DropTable("dbo.Pet_Adoption");
            DropTable("dbo.PaymentForAdoptions");
            DropTable("dbo.SpaServices");
            DropTable("dbo.BookingSpars");
        }
    }
}
