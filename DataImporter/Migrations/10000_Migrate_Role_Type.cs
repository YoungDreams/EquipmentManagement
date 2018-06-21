using FluentMigrator;

namespace DataImporter.Migrations
{
    [Migration(180613)]
    public class Migrate_Role_Type : Migration
    {
        public override void Up()
        {
            //Execute.Sql("Update [PPMDB].[dbo].[User] Set RoleType = 0 Where RoleType = '超级管理员'");
            //Execute.Sql("Update [PPMDB].[dbo].[User] Set RoleType = 1 Where RoleType = '内部员工'");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
