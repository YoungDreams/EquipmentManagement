using System;
using System.Collections.Generic;
using Foundation.Messaging;
using PPM.Entities;

namespace PPM.Commands
{
    public class CreateEquipmentInfoCommand : Command
    {
        public CreateEquipmentInfoCommand()
        {
            Files = new List<FileInfo>();
        }
        /// <summary>
        /// ��Ʒ����
        /// </summary>
        public int CategoryId { get; set; }
        public List<string> Values { get; set; }
        /// <summary>
        /// ��ƷͼƬ
        /// </summary>
        public FileInfo File { get; set; }
        public List<FileInfo> Files { get; set; }
        public string QrCodeImage { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// ���Σ��ɲ�ѯ
        /// </summary>
        public int BatchNum { get; set; }
        /// <summary>
        /// ��ƷС�࣬�ɲ�ѯ
        /// </summary>
        public int CategoryId1 { get; set; }
        /// <summary>
        /// ��Ʒ���ƣ��ɲ�ѯ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��ƷͼƬ·��
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// ��Ʒ���룬�ɲ�ѯ
        /// </summary>
        public string IdentifierNo { get; set; }
        /// <summary>
        /// ����ͺ�
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Meterial { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public string Technician { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string Picker { get; set; }
        /// <summary>
        /// �������ڣ��ɲ�ѯ
        /// </summary>
        public DateTime? OutDateTime { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public string CheckResult { get; set; }
        /// <summary>
        /// ��Ʒִ�б�׼
        /// </summary>
        public string ExecuteStandard { get; set; }
        /// <summary>
        /// ��װλ��
        /// </summary>
        public string SetupLocation { get; set; }
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
        /// <summary>
        /// ��������
        /// </summary>
        public string Manufacturer { get; set; }
        /// <summary>
        /// ���Σ��ɲ�ѯ
        /// </summary>
        public int BatchNum { get; set; }
        /// <summary>
        /// ��ƷС�࣬�ɲ�ѯ
        /// </summary>
        public EquipmentCategory EquipmentCategory1 { get; set; }
        /// <summary>
        /// ��Ʒ���ƣ��ɲ�ѯ
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��ƷͼƬ·��
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// ��Ʒ���룬�ɲ�ѯ
        /// </summary>
        public string IdentifierNo { get; set; }
        /// <summary>
        /// ����ͺ�
        /// </summary>
        public string Specification { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Meterial { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public string Technician { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public string Supplier { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public string Picker { get; set; }
        /// <summary>
        /// �������ڣ��ɲ�ѯ
        /// </summary>
        public DateTime? OutDateTime { get; set; }
        /// <summary>
        /// �����Ա
        /// </summary>
        public string Checker { get; set; }
        /// <summary>
        /// �����
        /// </summary>
        public string CheckResult { get; set; }
        /// <summary>
        /// ��Ʒִ�б�׼
        /// </summary>
        public string ExcuteStandard { get; set; }
        /// <summary>
        /// ��װλ��
        /// </summary>
        public string SetupLocation { get; set; }
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