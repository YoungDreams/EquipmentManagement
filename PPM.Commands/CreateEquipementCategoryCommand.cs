using System.Collections.Generic;
using Foundation.Messaging;
using PPM.Entities;

namespace PPM.Commands
{
    public class CreateEquipmentCategoryCommand : Command
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public bool Published { get; set; }
    }

    public class EditEquipmentCategoryCommand : Command
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public bool Published { get; set; }
        public IList<EquipmentCategoryColumn> Columns { get; set; }
    }

    public class EditEquipmentCategoryColumnCommand : Command
    {
        public int Id { get; set; }
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
    }

    public class SetEquipmentCategoryColumnsCommand : Command
    {
        public int Id { get; set; }
        public List<string> Columns { get; set; }
        public List<EquipmentCategoryColumnType> ColumnsTypes { get; set; }
    }
    public class DeleteEquipmentCategoryColumnCommand : Command
    {
        public int Id { get; set; }

    }

    public class ExportEquipmentCategoryColumnTemplateCommand : Command,
        ICommand<ExportEquipmentCategoryColumnTemplateCommand.Result>
    {
        public int CategoryId { get; set; }
        public class Result
        {
            public bool IsSucceed { get; set; }
            public string UrlPath { get; set; }
        }
    }

    public enum EquipmentCategoryColumnType
    {
        字符串,
        整数,
        浮点数,
        文件,
        日期
    }
}