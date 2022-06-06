namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roles3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "RoleId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "RoleId", c => c.Int());
        }
    }
}
