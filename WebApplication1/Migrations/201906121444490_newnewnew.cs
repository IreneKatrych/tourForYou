namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnewnew : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tours", "CountryId", "dbo.Countries");
            DropForeignKey("dbo.Tours", "HotelStarId", "dbo.HotelStars");
            DropForeignKey("dbo.Tours", "MealTypeId", "dbo.MealTypes");
            DropForeignKey("dbo.Tours", "TransportTypeId", "dbo.TransportTypes");
            DropIndex("dbo.Tours", new[] { "CountryId" });
            DropIndex("dbo.Tours", new[] { "HotelStarId" });
            DropIndex("dbo.Tours", new[] { "MealTypeId" });
            DropIndex("dbo.Tours", new[] { "TransportTypeId" });
            AddColumn("dbo.Tours", "Country", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "HotelStar", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "MealType", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "TransportType", c => c.Int(nullable: false));
            DropColumn("dbo.Tours", "CountryId");
            DropColumn("dbo.Tours", "HotelStarId");
            DropColumn("dbo.Tours", "MealTypeId");
            DropColumn("dbo.Tours", "TransportTypeId");
            DropTable("dbo.Countries");
            DropTable("dbo.HotelStars");
            DropTable("dbo.MealTypes");
            DropTable("dbo.TransportTypes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TransportTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
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
                "dbo.HotelStars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeHotel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tours", "TransportTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "MealTypeId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "HotelStarId", c => c.Int(nullable: false));
            AddColumn("dbo.Tours", "CountryId", c => c.Int(nullable: false));
            DropColumn("dbo.Tours", "TransportType");
            DropColumn("dbo.Tours", "MealType");
            DropColumn("dbo.Tours", "HotelStar");
            DropColumn("dbo.Tours", "Country");
            CreateIndex("dbo.Tours", "TransportTypeId");
            CreateIndex("dbo.Tours", "MealTypeId");
            CreateIndex("dbo.Tours", "HotelStarId");
            CreateIndex("dbo.Tours", "CountryId");
            AddForeignKey("dbo.Tours", "TransportTypeId", "dbo.TransportTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "MealTypeId", "dbo.MealTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "HotelStarId", "dbo.HotelStars", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tours", "CountryId", "dbo.Countries", "Id", cascadeDelete: true);
        }
    }
}
