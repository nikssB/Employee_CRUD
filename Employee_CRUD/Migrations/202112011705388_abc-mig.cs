namespace Employee_CRUD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class abcmig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Gender", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "Gender");
        }
    }
}
