using System;
using System.IO;
using System.Web.Hosting;
using Foundation.Data;
using Foundation.Messaging;
using PPM.Commands;
using PPM.Entities;

namespace PPM.CommandHandlers
{
    public class UploadEquipmentInfoAttachmentCommandHandler : ICommandHandler<UploadEquipmentInfoAttachmentCommand, UploadEquipmentInfoAttachmentCommand.Result>
    {
        private readonly IRepository _repository;
        private string VirtualPath => "~/Attachments/EquipementInfo/";
        private string StorageRoot
            => HostingEnvironment.MapPath(VirtualPath);
        public UploadEquipmentInfoAttachmentCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public UploadEquipmentInfoAttachmentCommand.Result Handle(UploadEquipmentInfoAttachmentCommand command)
        {
            var column = _repository.Get<EquipmentInfo>(command.Id);
            if (command.FileBytes == null || command.FileBytes.Length == 0)
            {
                throw new ApplicationException("ÇëÑ¡Ôñ¸½¼þ");
            }
            var fileName = command.FileName;
            var saveAsFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = VirtualPath + saveAsFileName;
            var saveAsPath = Path.Combine(StorageRoot, saveAsFileName);
            File.WriteAllBytes(saveAsPath, command.FileBytes);
            column.ImageUrl = filePath;
            _repository.Update(column);
            return new UploadEquipmentInfoAttachmentCommand.Result { FileName = saveAsFileName, FilePath = filePath };
        }
    }
}