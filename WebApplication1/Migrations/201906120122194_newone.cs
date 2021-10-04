namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Stage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Stage", c => c.String(nullable: false));
        }
    }
}
