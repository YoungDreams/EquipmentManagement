using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Foundation.Data;
using Foundation.Messaging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class ValidUsersCommandHandler : ICommandHandler<ValidUsersCommand>
    {
        private readonly IRepository _repository;

        public ValidUsersCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(ValidUsersCommand command)
        {
            foreach (var userId in command.UserIds)
            {
                var user = _repository.Get<User>(userId);
                user.IsEnabled = true;
                _repository.Update(user);
            }
        }
    }

    public class
        ExportEquipmentCategoryColumnTemplateCommandHandler : ICommandHandler<ExportEquipmentCategoryColumnTemplateCommand,
            ExportEquipmentCategoryColumnTemplateCommand.Result>
    {
        private readonly IRepository _repository;

        public ExportEquipmentCategoryColumnTemplateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public ExportEquipmentCategoryColumnTemplateCommand.Result Handle(ExportEquipmentCategoryColumnTemplateCommand command)
        {
            var category = _repository.Get<EquipmentCategory>(command.CategoryId);
            var columns = category.Columns;
            string fileName = $"{category.Name}设备分类模板-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // 文件名称
            string urlPath = "Attachments/CategoryTemplate/" + fileName; // 文件下载的URL地址，供给前台下载
            string filePath = HttpContext.Current.Server.MapPath("\\" + urlPath); // 文件路径
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                if (directoryName != null) Directory.CreateDirectory(directoryName);
            }
            //创建Excel文件的对象  
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet  
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题  
            NPOI.SS.UserModel.IRow rowHeader = sheet1.CreateRow(0);

            rowHeader.CreateCell(0).SetCellValue("ID"); // 表头列名
            rowHeader.CreateCell(1).SetCellValue("设备分类"); // 表头列名
            rowHeader.CreateCell(2).SetCellValue("生产厂商");
            rowHeader.CreateCell(3).SetCellValue("批次");
            rowHeader.CreateCell(4).SetCellValue("产品小类");
            rowHeader.CreateCell(5).SetCellValue("产品名称");
            rowHeader.CreateCell(6).SetCellValue("产品编码");
            rowHeader.CreateCell(7).SetCellValue("规格型号");
            rowHeader.CreateCell(8).SetCellValue("材质"); 
            rowHeader.CreateCell(9).SetCellValue("技术人员");
            rowHeader.CreateCell(10).SetCellValue("物资人员");
            rowHeader.CreateCell(11).SetCellValue("领料人");
            rowHeader.CreateCell(12).SetCellValue("出厂日期");
            rowHeader.CreateCell(13).SetCellValue("检测人员");
            rowHeader.CreateCell(14).SetCellValue("检测结果"); 
            rowHeader.CreateCell(15).SetCellValue("产品执行标准");
            rowHeader.CreateCell(16).SetCellValue("安装位置");
            var dateIndexes = new List<int>();
            for (int i = 0; i < columns.Count; i++)
            {
                var cellStartIndex = i + 17;
                rowHeader.CreateCell(cellStartIndex).SetCellValue(columns[i].ColumnName); // 表头列名
                if (columns[i].ColumnType == EquipmentCategoryColumnType.日期.ToString())
                {
                    dateIndexes.Add(cellStartIndex);
                }
            }
            IRow row1 = sheet1.CreateRow(1);
            row1.CreateCell(0).SetCellValue(category.Id);
            row1.CreateCell(1).SetCellValue(category.Name);
            foreach (var dateIndex in dateIndexes)
            {
                var cell = row1.CreateCell(dateIndex);
                //设置单元格格式
                HSSFCellStyle style = (HSSFCellStyle)book.CreateCellStyle();
                HSSFDataFormat format = (HSSFDataFormat)book.CreateDataFormat();
                style.DataFormat = format.GetFormat("yyyy年mm月dd日");
                cell.CellStyle = style;
            }
            // 4.生成文件
            FileStream file = new FileStream(filePath, FileMode.Create);
            book.Write(file);
            file.Close();
            return new ExportEquipmentCategoryColumnTemplateCommand.Result
            {
                IsSucceed = true,
                UrlPath = urlPath,
            };
        }
    }
}