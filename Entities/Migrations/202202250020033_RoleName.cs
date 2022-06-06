namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RoleName", c => c.String());
            DropColumn("dbo.AspNetUsers", "RoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RoleId", c => c.String());
            DropColumn("dbo.AspNetUsers", "RoleName");
        }
    }
}
