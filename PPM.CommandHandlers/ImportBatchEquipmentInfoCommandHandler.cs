using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Foundation.Core;
using Foundation.Data;
using Foundation.Messaging;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using PPM.Commands;
using PPM.Entities;
using PPM.Entities.Exceptions;
using QRCoder;

namespace PPM.CommandHandlers
{
    public class ImportBatchEquipmentInfoCommandHandler : ICommandHandler<ImportBatchEquipmentInfoCommand, ImportBatchEquipmentInfoCommand.Result>
    {
        private readonly IRepository _repository;
        private readonly string _host = ConfigurationManager.AppSettings["Host"];
        private string VirtualPath => "~/Attachments/QrCode/";
        private string StorageRoot
            => HostingEnvironment.MapPath(VirtualPath);
        public ImportBatchEquipmentInfoCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public ImportBatchEquipmentInfoCommand.Result Handle(ImportBatchEquipmentInfoCommand command)
        {
            if (command.FileBytes == null || command.FileBytes.Length == 0)
            {
                throw new ApplicationException("请选择附件");
            }
            var excelTypes = new[] { ".xlsx", ".xls" };

            var extension = Path.GetExtension(command.FileName);

            if (extension != null)
            {
                var extensionName = extension.ToLower();

                if (excelTypes.All(x => x != extensionName))
                {
                    throw new ApplicationException("请选择EXCEL文件进行导入！");
                }
            }

            var result = new ImportBatchEquipmentInfoCommand.Result();

            using (var stream = new MemoryStream(command.FileBytes))
            {
                stream.Seek(0, SeekOrigin.Begin);

                var workbook = new HSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);
                var row = sheet.GetRow(0);
                var cell = row.GetCell(0)?.StringCellValue;
                if (cell != "ID")
                {
                    throw new ApplicationException("此EXCEL文件不是所导出的模板文件！");
                }
                var categoryId = sheet.GetRow(1).GetCell(0).GetCellValue();
                var categories = _repository.Query<EquipmentCategory>();
                var category = categories.SingleOrDefault(x => x.Id == int.Parse(categoryId));
                if (category == null) throw new ApplicationException("设备分类匹配失败！");

                var equipmentInfo = new EquipmentInfo
                {
                    EquipmentCategory = category
                };
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    var manufacturer = sheet.GetRow(rowNum).GetCell(2).GetCellValue();
                    var batchNum = sheet.GetRow(rowNum).GetCell(3).GetCellValue();
                    
                    var categoryName1 = sheet.GetRow(rowNum).GetCell(4).GetCellValue();
                    var name = sheet.GetRow(rowNum).GetCell(5).GetCellValue();
                    var identifierNo = sheet.GetRow(rowNum).GetCell(6).GetCellValue();
                    var specification = sheet.GetRow(rowNum).GetCell(7).GetCellValue();
                    var meterial = sheet.GetRow(rowNum).GetCell(8).GetCellValue();
                    var technician = sheet.GetRow(rowNum).GetCell(9).GetCellValue();
                    var supplier = sheet.GetRow(rowNum).GetCell(10).GetCellValue();
                    var picker = sheet.GetRow(rowNum).GetCell(11).GetCellValue();
                    var outDateTime = sheet.GetRow(rowNum).GetCell(12)?.GetCellDateTime()?.ToString();
                    
                    var checker = sheet.GetRow(rowNum).GetCell(13).GetCellValue();
                    var checkResult = sheet.GetRow(rowNum).GetCell(14).GetCellValue();
                    var executeStandard = sheet.GetRow(rowNum).GetCell(15).GetCellValue();
                    var setupLocation = sheet.GetRow(rowNum).GetCell(16).GetCellValue();
                    equipmentInfo.Manufacturer = manufacturer;
                    int batchNumResult = 0;
                    if (string.IsNullOrEmpty(batchNum))
                    {
                        //result.IsSucceed = false;
                        //result.Errors.Add($"第{rowNum}行的批次未填写！");
                        //break;
                        throw new DomainValidationException($"第{rowNum}行的批次未填写！");
                    }
                    if (!int.TryParse(batchNum, out batchNumResult))
                    {
                        //result.IsSucceed = false;
                        //result.Errors.Add($"第{rowNum}行的批次填写错误！");
                        //break;
                        throw new DomainValidationException($"第{rowNum}行的批次填写错误！");
                    }
                    equipmentInfo.BatchNum = Convert.ToInt32(batchNum);    
                    equipmentInfo.CheckResult = checkResult;
                    equipmentInfo.Checker = checker;
                    equipmentInfo.ExecuteStandard = executeStandard;
                    equipmentInfo.IdentifierNo = identifierNo;
                    equipmentInfo.Meterial = meterial;
                    equipmentInfo.Name = name;
                    if (outDateTime == null)
                    {
                        //result.IsSucceed = false;
                        //result.Errors.Add($"第{rowNum}行的出厂日期未填写！");
                        //break;
                        throw new DomainValidationException($"第{rowNum}行的出厂日期未填写！");
                    }
                    equipmentInfo.OutDateTime = Convert.ToDateTime(outDateTime);
                    equipmentInfo.Picker = picker;
                    equipmentInfo.SetupLocation = setupLocation;
                    equipmentInfo.Specification = specification;
                    equipmentInfo.Supplier = supplier;
                    equipmentInfo.Technician = technician;
                    if (!string.IsNullOrEmpty(categoryName1))
                    {
                        var category1 = categories.FirstOrDefault(x => x.Name == categoryName1);
                        equipmentInfo.EquipmentCategory1 = category1;
                    }
                    _repository.Create(equipmentInfo);
                    var fileName = SaveQrCodeImage(equipmentInfo);
                    equipmentInfo.QrCodeImage = fileName;
                    _repository.Update(equipmentInfo);
                    
                    for (int i = 0; i < category.Columns.Count; i++)
                    {
                        var cellStartIndex = i + 17;
                        var type = category.Columns[i].ColumnType;
                        var currentRow = sheet.GetRow(rowNum);
                        var currentCell = currentRow.GetCell(cellStartIndex);
                        var value = currentCell?.GetCellValue();
                        if (type == EquipmentCategoryColumnType.日期.ToString())
                        {
                            value = currentCell?.GetCellDateTime().ToString();
                        }
                        
                        IsValidTypeValue(type, value, rowNum, cellStartIndex);
                        var columnValue = new EquipmentInfoColumnValue
                        {
                            Value = value,
                            EquipmentInfo = equipmentInfo
                        };
                        _repository.Create(columnValue);
                    }
                }
            }
            return result;
        }

        private string SaveQrCodeImage(EquipmentInfo equipmentInfo)
        {
            var qrcodeUrl = _host + "/EquipmentInfo/ViewInWeChat?id=" + equipmentInfo.Id;
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodeUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(100);
            var fileName = $"{equipmentInfo.Id}.png";
            var fullfileName = Path.Combine(StorageRoot, fileName);
            qrCodeImage.Save(fullfileName, ImageFormat.Png);
            return VirtualPath.Replace("~", "") + fileName;
        }

        private void IsValidTypeValue(string type, string value, int rowNum, int column)
        {
            var toType = Enum.Parse(typeof(EquipmentCategoryColumnType), type);
            switch (toType)
            {
                case EquipmentCategoryColumnType.整数:
                    int result = 0;
                    if (!int.TryParse(value, out result) && !string.IsNullOrEmpty(value))
                    {
                        throw new ApplicationException($"第{rowNum}行{column+1}列的{type}{value}不是{EquipmentCategoryColumnType.整数}类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.小数:
                    double result1 = 0.00;
                    if (!double.TryParse(value, out result1) && !string.IsNullOrEmpty(value))
                    {
                        throw new ApplicationException($"第{rowNum}行{column+1}列的{type}{value}不是{EquipmentCategoryColumnType.小数}类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.日期:
                    DateTime date;
                    if (!DateTime.TryParse(value, out date) && !string.IsNullOrEmpty(value))
                    {
                        throw new ApplicationException($"第{rowNum}行{column+1}列的{type}{value}不是{EquipmentCategoryColumnType.日期}类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.文件:
                    if (!string.IsNullOrEmpty(value))
                    {
                        throw new ApplicationException($"第{rowNum}行{column+1}列的{type}{value}不能填写，只能从后台上传。");
                    }
                    break;
            }
        }

        private void IsValidTypeValue(string type, string value)
        {
            var toType = Enum.Parse(typeof(EquipmentCategoryColumnType), type);
            switch (toType)
            {
                case EquipmentCategoryColumnType.整数:
                    int result = 0;
                    if (!int.TryParse(value, out result) && !string.IsNullOrEmpty(value))
                    {
                        throw new DomainValidationException($"{value}不是整数类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.小数:
                    double result1 = 0.00;
                    if (!double.TryParse(value, out result1) && !string.IsNullOrEmpty(value))
                    {
                        throw new DomainValidationException($"{value}不是小数类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.日期:
                    DateTime date;
                    if (!DateTime.TryParse(value, out date) && !string.IsNullOrEmpty(value))
                    {
                        throw new DomainValidationException($"{value}不是浮点数类型。");
                    }
                    break;
            }
        }
    }

    public static class CellExtensions
    {
        public static string GetCellValue(this ICell cell)
        {
            if (cell.CellType == CellType.Numeric)
                return cell.NumericCellValue.ToString();
            return cell.StringCellValue;
        }

        public static DateTime? GetCellDateTime(this ICell cell)
        {
            if (cell.CellType == CellType.Numeric)
            {
                var result = DateTime.FromOADate(cell.NumericCellValue);
                return result;
            }

            return cell.StringCellValue.ToDateTime();
        }
    }
}