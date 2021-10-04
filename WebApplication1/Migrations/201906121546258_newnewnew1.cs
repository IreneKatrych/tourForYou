namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newnewnew1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tours", "HotelStar", c => c.Int(nullable: false));
            AlterColumn("dbo.Tours", "MealType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tours", "MealType", c => c.String(nullable: false));
            AlterColumn("dbo.Tours", "HotelStar", c => c.String(nullable: false));
        }
    }
}
