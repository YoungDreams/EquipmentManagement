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
            string fileName = $"{category.Name}�豸����ģ��-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls"; // �ļ�����
            string urlPath = "Attachments/CategoryTemplate/" + fileName; // �ļ����ص�URL��ַ������ǰ̨����
            string filePath = HttpContext.Current.Server.MapPath("\\" + urlPath); // �ļ�·��
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                if (directoryName != null) Directory.CreateDirectory(directoryName);
            }
            //����Excel�ļ��Ķ���  
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //���һ��sheet  
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //��sheet1��ӵ�һ�е�ͷ������  
            NPOI.SS.UserModel.IRow rowHeader = sheet1.CreateRow(0);

            rowHeader.CreateCell(0).SetCellValue("ID"); // ��ͷ����
            rowHeader.CreateCell(1).SetCellValue("�豸����"); // ��ͷ����
            rowHeader.CreateCell(2).SetCellValue("��������");
            rowHeader.CreateCell(3).SetCellValue("����");
            rowHeader.CreateCell(4).SetCellValue("��ƷС��");
            rowHeader.CreateCell(5).SetCellValue("��Ʒ����");
            rowHeader.CreateCell(6).SetCellValue("��Ʒ����");
            rowHeader.CreateCell(7).SetCellValue("����ͺ�");
            rowHeader.CreateCell(8).SetCellValue("����"); 
            rowHeader.CreateCell(9).SetCellValue("������Ա");
            rowHeader.CreateCell(10).SetCellValue("������Ա");
            rowHeader.CreateCell(11).SetCellValue("������");
            rowHeader.CreateCell(12).SetCellValue("��������");
            rowHeader.CreateCell(13).SetCellValue("�����Ա");
            rowHeader.CreateCell(14).SetCellValue("�����"); 
            rowHeader.CreateCell(15).SetCellValue("��Ʒִ�б�׼");
            rowHeader.CreateCell(16).SetCellValue("��װλ��");
            var dateIndexes = new List<int>();
            for (int i = 0; i < columns.Count; i++)
            {
                var cellStartIndex = i + 17;
                rowHeader.CreateCell(cellStartIndex).SetCellValue(columns[i].ColumnName); // ��ͷ����
                if (columns[i].ColumnType == EquipmentCategoryColumnType.����.ToString())
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
                //���õ�Ԫ���ʽ
                HSSFCellStyle style = (HSSFCellStyle)book.CreateCellStyle();
                HSSFDataFormat format = (HSSFDataFormat)book.CreateDataFormat();
                style.DataFormat = format.GetFormat("yyyy��mm��dd��");
                cell.CellStyle = style;
            }
            // 4.�����ļ�
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