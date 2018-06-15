using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;
using PPM.Entities.Exceptions;

namespace PPM.CommandHandlers
{
    public class EditEquipmentInfoCommandHandler : ICommandHandler<EditEquipmentInfoCommand>
    {
        private readonly IRepository _repository;
        private string FileVirtualPath => "~/Attachments/EquipementInfo/";
        private string FileStorageRoot
            => HostingEnvironment.MapPath(FileVirtualPath);
        public EditEquipmentInfoCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(EditEquipmentInfoCommand command)
        {
            var equipmentInfo = _repository.Get<EquipmentInfo>(command.EquipmentInfoId);
            equipmentInfo.Manufacturer = command.Manufacturer;
            equipmentInfo.BatchNum = command.BatchNum;
            equipmentInfo.CheckResult = command.CheckResult;
            equipmentInfo.Checker = command.Checker;
            equipmentInfo.ExecuteStandard = command.ExcuteStandard;
            equipmentInfo.IdentifierNo = command.IdentifierNo;
            equipmentInfo.Meterial = command.Meterial;
            equipmentInfo.Name = command.Name;
            equipmentInfo.OutDateTime = command.OutDateTime;
            equipmentInfo.Picker = command.Picker;
            equipmentInfo.SetupLocation = command.SetupLocation;
            equipmentInfo.Specification = command.Specification;
            equipmentInfo.Supplier = command.Supplier;
            equipmentInfo.Technician = command.Technician;
            equipmentInfo.ImageUrl =
                SaveFile(command.File.FileBytes, command.File.FileName);
            equipmentInfo.EquipmentCategory1 = new EquipmentCategory {Id = command.CategoryId1};
            _repository.Update(equipmentInfo);
            var category = equipmentInfo.EquipmentCategory;
            var index = 0;
            foreach (var categoryColun in category.Columns)
            {
                var equipmentInfoColumnValue = equipmentInfo.EquipmentInfoColumnValues[index];
                if (categoryColun.ColumnType == EquipmentCategoryColumnType.文件.ToString())
                {
                    var firstOrDefault = command.Files.FirstOrDefault();
                    if (firstOrDefault != null)
                    {
                        if (firstOrDefault.FileBytes != null && !string.IsNullOrEmpty(firstOrDefault.FileName))
                        {
                            equipmentInfoColumnValue.Value =
                                SaveFile(firstOrDefault.FileBytes, firstOrDefault.FileName);
                            _repository.Update(equipmentInfoColumnValue);
                        }
                        command.Files.Remove(firstOrDefault);
                    }
                }
                else
                {
                    var type = category.Columns[index].ColumnType;
                    IsValidTypeValue(type, command.Values[index]);
                    equipmentInfoColumnValue.Value = command.Values[index];
                    _repository.Update(equipmentInfoColumnValue);
                }
                index++;
            }
        }

        public string SaveFile(byte[] fileBytes, string fileName)
        {
            if (fileBytes == null || string.IsNullOrEmpty(fileName))
            {
                throw new ApplicationException("请选择附件");
            }
            var saveAsFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = FileVirtualPath + saveAsFileName;
            var saveAsPath = Path.Combine(FileStorageRoot, saveAsFileName);
            File.WriteAllBytes(saveAsPath, fileBytes);
            return filePath.Replace("~", "");
        }

        private void IsValidTypeValue(string type, string value)
        {
            var toType = Enum.Parse(typeof(EquipmentCategoryColumnType), type);
            switch (toType)
            {
                case EquipmentCategoryColumnType.整数:
                    int result = 0;
                    if (!int.TryParse(value, out result))
                    {
                        throw new DomainValidationException($"{value}不是整数类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.小数:
                    double result1 = 0.00;
                    if (!double.TryParse(value, out result1))
                    {
                        throw new DomainValidationException($"{value}不是小数类型。");
                    }
                    break;
                case EquipmentCategoryColumnType.日期:
                    DateTime date;
                    if (!DateTime.TryParse(value, out date))
                    {
                        throw new DomainValidationException($"{value}不是浮点数类型。");
                    }
                    break;
            }
        }
    }

    public class DeleteEquipmentInfoCommandHandler : ICommandHandler<DeleteEquipmentInfoCommand>
    {
        private readonly IRepository _repository;
        public DeleteEquipmentInfoCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(DeleteEquipmentInfoCommand command)
        {
            _repository.Execute($"DELETE FROM EquipmentInfoColumnValue WHERE EquipmentInfoId = {command.Id}");
            _repository.Execute($"DELETE FROM EquipmentInfo WHERE Id = {command.Id}");
        }
    }
}