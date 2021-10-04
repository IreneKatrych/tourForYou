namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newone2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tours", "GetTransport_Id", "dbo.TransportTypes");
            DropIndex("dbo.Tours", new[] { "GetTransport_Id" });
            DropColumn("dbo.Tours", "TransportTypeId");
            RenameColumn(table: "dbo.Tours", name: "GetTransport_Id", newName: "TransportTypeId");
            AlterColumn("dbo.Tours", "TransportTypeId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tours", "TransportTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tours", "TransportTypeId");
            AddForeignKey("dbo.Tours", "TransportTypeId", "dbo.TransportTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tours", "TransportTypeId", "dbo.TransportTypes");
            DropIndex("dbo.Tours", new[] { "TransportTypeId" });
            AlterColumn("dbo.Tours", "TransportTypeId", c => c.Int());
            AlterColumn("dbo.Tours", "TransportTypeId", c => c.String(nullable: false));
            RenameColumn(table: "dbo.Tours", name: "TransportTypeId", newName: "GetTransport_Id");
            AddColumn("dbo.Tours", "TransportTypeId", c => c.String(nullable: false));
            CreateIndex("dbo.Tours", "GetTransport_Id");
            AddForeignKey("dbo.Tours", "GetTransport_Id", "dbo.TransportTypes", "Id");
        }
    }
}
