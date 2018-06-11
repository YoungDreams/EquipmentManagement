using System.Collections.Generic;
using Foundation.Messaging;

namespace PPM.Commands
{
    public class CreateEquipmentInfoCommand : Command
    {
        public CreateEquipmentInfoCommand()
        {
            Files = new List<FileInfo>();
        }
        public int CategoryId { get; set; }
        public List<string> Values { get; set; }
        public List<FileInfo> Files { get; set; }
    }

    public class FileInfo
    {
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
    }

    public class EditEquipmentInfoCommand : Command
    {
        public int EquipmentInfoId { get; set; }
        public List<string> Values { get; set; }
        public List<FileInfo> Files { get; set; }
    }
    public class DeleteEquipmentInfoCommand : Command
    {
        public int Id { get; set; }
    }
    public class UploadEquipmentInfoAttachmentCommand : Command, ICommand<UploadEquipmentInfoAttachmentCommand.Result>
    {
        public int ColumnId { get; set; }
        public byte[] FileBytes { get; set; }
        public string FileName { get; set; }
        public class Result
        {
            public string FileName { get; set; }
            public string FilePath { get; set; }
        }
    }
    public class ImportBatchEquipmentInfoCommand : Command, ICommand<ImportBatchEquipmentInfoCommand.Result>
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public class Result
        {
            public Result()
            {
                IsSucceed = true;
                Errors = new List<string>();
            }
            public bool IsSucceed { get; set; }
            public List<string> Errors { get; set; }
        }
    }
}