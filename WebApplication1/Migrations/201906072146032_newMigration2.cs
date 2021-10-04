namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tours", "TOperator", c => c.String(nullable: false));
            AddColumn("dbo.Tours", "Link", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tours", "Link");
            DropColumn("dbo.Tours", "TOperator");
        }
    }
}
