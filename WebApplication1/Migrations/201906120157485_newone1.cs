namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newone1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HotelStars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeHotel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MealTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tours", "CountryId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "HotelStarId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "MealTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "TransportTypeId", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "GetTransport_Id", c => c.Int());
            AlterColumn("dbo.Orders", "PaymentStage", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "DocumentStage", c => c.Int(nullable: false));
            CreateIndex("dbo.Tours", "CountryId");
            CreateIndex("dbo.Tours", "HotelStarId");
            CreateIndex("dbo.Tours", "MealTypeId");
            CreateIndex("dbo.Tours", "GetTransport_Id");
            AddForeignKey("dbo.Tours", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "HotelStarId", "dbo.HotelStars", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "MealTypeId", "dbo.MealTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "GetTransport_Id", "dbo.TransportTypes", "Id");
            DropColumn("dbo.Tours", "Country");
            DropColumn("dbo.Tours", "HotelClass");
            DropColumn("dbo.Tours", "MealClass");
            DropColumn("dbo.Tours", "TransportType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tours", "TransportType", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "MealClass", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "HotelClass", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "Country", c => c.String(nullable: false));
            DropForeignKey("dbo.Tours", "GetTransport_Id", "dbo.TransportTypes");
            DropForeignKey("dbo.Tours", "MealTypeId", "dbo.MealTypes");
            DropForeignKey("dbo.Tours", "HotelStarId", "dbo.HotelStars");
            DropForeignKey("dbo.Tours", "CountryId", "dbo.Countries");
            DropIndex("dbo.Tours", new[] { "GetTransport_Id" });
            DropIndex("dbo.Tours", new[] { "MealTypeId" });
            DropIndex("dbo.Tours", new[] { "HotelStarId" });
            DropIndex("dbo.Tours", new[] { "CountryId" });
            AlterColumn("dbo.Orders", "DocumentStage", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "PaymentStage", c => c.String(nullable: false));
            DropColumn("dbo.Tours", "GetTransport_Id");
            DropColumn("dbo.Tours", "TransportTypeId");
            DropColumn("dbo.Tours", "MealTypeId");
            DropColumn("dbo.Tours", "HotelStarId");
            DropColumn("dbo.Tours", "CountryId");
            DropTable("dbo.TransportTypes");
            DropTable("dbo.MealTypes");
            DropTable("dbo.HotelStars");
            DropTable("dbo.Countries");
        }
    }
}
