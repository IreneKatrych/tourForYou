namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "HowManyPeople", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "HowManyPeople", c => c.Int(nullable: false));
        }
    }
}
