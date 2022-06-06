namespace Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "RoleId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RoleId", c => c.String());
        }
    }
}
