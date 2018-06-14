using FluentMigrator;

namespace DataImporter.Migrations
{
    [Migration(180614)]
    public class Migrate_EquipmentCategoryColumnType : Migration
    {
        public override void Up()
        {
            Execute.Sql("Update [PPMDB].[dbo].[EquipmentCategoryColumn] Set ColumnType = '字符' Where ColumnType = '字符串'");
            Execute.Sql("Update [PPMDB].[dbo].[EquipmentCategoryColumn] Set ColumnType = '小数' Where ColumnType = '浮点数'");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}