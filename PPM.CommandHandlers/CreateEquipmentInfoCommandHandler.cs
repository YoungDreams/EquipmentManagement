using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;
using PPM.Entities.Exceptions;
using QRCoder;

namespace PPM.CommandHandlers
{
    public class CreateEquipmentInfoCommandHandler : ICommandHandler<CreateEquipmentInfoCommand>
    {
        private readonly IRepository _repository;
        private readonly string _host = ConfigurationManager.AppSettings["Host"];
        private string QrCodeVirtualPath => "~/Attachments/QrCode/";
        private string FileVirtualPath => "~/Attachments/EquipementInfo/";
        private string QrCodeStorageRoot
            => HostingEnvironment.MapPath(QrCodeVirtualPath);
        private string FileStorageRoot
            => HostingEnvironment.MapPath(FileVirtualPath);
        public CreateEquipmentInfoCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateEquipmentInfoCommand command)
        {
            var equipmentInfo = command.MapToEntity<EquipmentInfo>();
            var category = _repository.Get<EquipmentCategory>(command.CategoryId);
            equipmentInfo.EquipmentCategory = category ?? throw new DomainValidationException("产品大类未选择！");
            if (command.CategoryId1 != 0)
            {
                equipmentInfo.EquipmentCategory1 = new EquipmentCategory
                {
                    Id = command.CategoryId1
                };
            }
            equipmentInfo.ImageUrl =
                SaveFile(command.File.FileBytes, command.File.FileName);
            _repository.Create(equipmentInfo);

            var index = 0;
            foreach (var categoryColun in category.Columns)
            {
                if (categoryColun.ColumnType == EquipmentCategoryColumnType.文件.ToString())
                {
                    var firstOrDefault = command.Files.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        var equipmentInfoColumnValue = new EquipmentInfoColumnValue
                        {
                            EquipmentInfo = equipmentInfo,
                            Value = SaveFile(firstOrDefault.FileBytes, firstOrDefault.FileName, false)
                        };
                        _repository.Create(equipmentInfoColumnValue);
                        command.Files.Remove(firstOrDefault);
                    }
                }
                else 
                {
                    var type = category.Columns[index].ColumnType;
                    IsValidTypeValue(type, command.Values[index]);
                    if (type == EquipmentCategoryColumnType.小数.ToString()&&string.IsNullOrEmpty(command.Values[index]))
                    {
                        command.Values[index] = "0";
                    }
                    var equipmentInfoColumnValue = new EquipmentInfoColumnValue
                    {
                        EquipmentInfo = equipmentInfo,
                        Value = command.Values[index]
                    };
                    _repository.Create(equipmentInfoColumnValue);
                    index++;
                }
            }
            
            var fileName = SaveQrCodeImage(equipmentInfo);
            equipmentInfo.QrCodeImage = fileName;
            _repository.Update(equipmentInfo);
        }

        public string SaveFile(byte[] fileBytes, string fileName, bool isRequired = true)
        {
            if (fileBytes == null && string.IsNullOrEmpty(fileName) && isRequired)
            {
                throw new ApplicationException("请选择附件");
            }
            var saveAsFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = FileVirtualPath + saveAsFileName;
            var saveAsPath = Path.Combine(FileStorageRoot, saveAsFileName);
            File.WriteAllBytes(saveAsPath, fileBytes);
            return filePath.Replace("~", "");
        }

        private string SaveQrCodeImage(EquipmentInfo equipmentInfo)
        {
            var qrcodeUrl = _host + "/EquipmentInfo/ViewInWeChat?id=" + equipmentInfo.Id;
            var qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrcodeUrl, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(100);
            var fileName = $"{equipmentInfo.Id}.png";
            var fullfileName = Path.Combine(QrCodeStorageRoot, fileName);
            qrCodeImage.Save(fullfileName, ImageFormat.Png);
            return QrCodeVirtualPath.Replace("~", "") + fileName;
        }

        private void IsValidTypeValue(string type,string value)
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
}