using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        /// 产品大类
        /// </summary>
        //[Required]
        //[DisplayName("产品大类")]
        public int CategoryId { get; set; }
        public List<string> Values { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        public FileInfo File { get; set; }
        public List<FileInfo> Files { get; set; }
        //public string QrCodeImage { get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        [Required]
        [DisplayName("生产厂商")]
        public string Manufacturer { get; set; }
        /// <summary>
        /// 批次，可查询
        /// </summary>
        [Required]
        [DisplayName("批次")]
        public int BatchNum { get; set; }
        /// <summary>
        /// 产品小类，可查询
        /// </summary>
        [Required]
        [DisplayName("产品小类")]
        public int CategoryId1 { get; set; }
        /// <summary>
        /// 产品名称，可查询
        /// </summary>
        [Required]
        [DisplayName("产品名称")]
        public string Name { get; set; }
        /// <summary>
        /// 产品图片路径
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 产品编码，可查询
        /// </summary>
        [Required]
        [DisplayName("产品编码")]
        public string IdentifierNo { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [Required]
        [DisplayName("规格型号")]
        public string Specification { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        [Required]
        [DisplayName("材质")]
        public string Meterial { get; set; }
        /// <summary>
        /// 技术人员
        /// </summary>
        [Required]
        [DisplayName("技术人员")]
        public string Technician { get; set; }
        /// <summary>
        /// 物资人员
        /// </summary>
        [Required]
        [DisplayName("物资人员")]
        public string Supplier { get; set; }
        /// <summary>
        /// 领料人
        /// </summary>
        [Required]
        [DisplayName("领料人")]
        public string Picker { get; set; }
        /// <summary>
        /// 出厂日期，可查询
        /// </summary>
        [Required]
        [DisplayName("出厂日期")]
        public DateTime? OutDateTime { get; set; }
        /// <summary>
        /// 检测人员
        /// </summary>
        [Required]
        [DisplayName("检测人员")]
        public string Checker { get; set; }
        /// <summary>
        /// 检测结果
        /// </summary>
        [Required]
        [DisplayName("检测结果")]
        public string CheckResult { get; set; }
        /// <summary>
        /// 产品执行标准
        /// </summary>
        [Required]
        [DisplayName("产品执行标准")]
        public string ExecuteStandard { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        [Required]
        [DisplayName("安装位置")]
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
        /// 产品图片
        /// </summary>
        public FileInfo File { get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        [Required]
        public string Manufacturer { get; set; }
        /// <summary>
        /// 批次，可查询
        /// </summary>
        [Required]
        public int BatchNum { get; set; }
        /// <summary>
        /// 产品小类，可查询
        /// </summary>
        [Required]
        public int CategoryId1 { get; set; }
        /// <summary>
        /// 产品名称，可查询
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 产品图片路径
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 产品编码，可查询
        /// </summary>
        [Required]
        public string IdentifierNo { get; set; }
        /// <summary>
        /// 规格型号
        /// </summary>
        [Required]
        public string Specification { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        [Required]
        public string Meterial { get; set; }
        /// <summary>
        /// 技术人员
        /// </summary>
        [Required]
        public string Technician { get; set; }
        /// <summary>
        /// 物资人员
        /// </summary>
        [Required]
        public string Supplier { get; set; }
        /// <summary>
        /// 领料人
        /// </summary>
        [Required]
        public string Picker { get; set; }
        /// <summary>
        /// 出厂日期，可查询
        /// </summary>
        [Required]
        public DateTime? OutDateTime { get; set; }
        /// <summary>
        /// 检测人员
        /// </summary>
        [Required]
        public string Checker { get; set; }
        /// <summary>
        /// 检测结果
        /// </summary>
        [Required]
        public string CheckResult { get; set; }
        /// <summary>
        /// 产品执行标准
        /// </summary>
        [Required]
        public string ExecuteStandard { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        [Required]
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