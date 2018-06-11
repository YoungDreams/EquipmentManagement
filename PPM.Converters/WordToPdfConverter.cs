//using System;
//using System.IO;
//using System.Linq;
//using System.Web.Hosting;
//using Foundation.Data;
//using Foundation.Utils;
//using Microsoft.Office.Interop.Word;
//using PPM.Entities;
//using PPM.Entities.Exceptions;
//using TemplateEngine.Docx;
//using Foundation.Core;
//using PPM.Query;

//namespace PPM.Converters
//{
//    public class WordToPdfConverter
//    {
//        private readonly IRepository _repository;
//        private readonly ICustomerQueryService _customerQueryService;
//        private readonly IContractQueryService _contractQueryService;
//        public WordToPdfConverter(IRepository repository, ICustomerQueryService customerQueryService, IContractQueryService contractQueryService)
//        {
//            _repository = repository;
//            _customerQueryService = customerQueryService;
//            _contractQueryService = contractQueryService;
//        }

//        public string PrintVirtualPath => "~/Attachments/Print/";

//        public string PrintStorageRoot
//            => HostingEnvironment.MapPath(PrintVirtualPath);

//        public string TemplateVirtualPath => "~/ContractTemplates/";

//        public string TemplateStorageRoot
//            => HostingEnvironment.MapPath(TemplateVirtualPath);

//        public const string WordExtension = ".docx";
//        public const string PdfExtension = ".pdf";

//        public const string ContractRoomChangeTemplateName = "房间变更补充协议";
//        public const string ContractRoomChangeTemplateName20170301 = "房间变更补充协议20170301";
//        public const string ContractRoomChangeTemplateName20170801 = "房间变更补充协议20170801";
//        public const string ContractCostChangeTemplateName = "月费减免增加月费补充协议";
//        public const string ContractCostChangeTemplateName20170301 = "月费减免增加月费补充协议20170301";
//        public const string ContractCostChangeTemplateName20170801 = "月费减免增加月费补充协议20170801";
//        public const string ContractCostChangeTypeTemplateName20170301 = "开业优惠补充协议折扣20170301";
//        public const string ContractCostChangeTypeTemplateName20170801 = "开业优惠补充协议折扣20170801";
//        public const string ContractServicePackChangeTemplateName = "护理级别变更补充协议";
//        public const string ContractServicePackChangeTemplateName20170301 = "护理级别变更补充协议20170301";
//        public const string ContractServicePackChangeTemplateName20170801 = "护理级别变更补充协议20170801";
//        public const string CustomerAccountCheckOutRefundTemplateName = "正式合同退费申请";
//        public const string CustomerAccountCheckOutAgreementTemplateName = "椿萱茂养老服务合同终止协议";
//        public const string CustomerAccountTransferTemplateName = "转款委托书";
//        public const string CustomerBasicInfo = "客户基本信息";

//        public string FromContract(int contractId)
//        {
//            var contract = _repository.Get<Contract>(contractId);

//            string destFileName = PrintStorageRoot + $"{contract.ContractNo}{contract.BName}{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            File.Copy(TemplateStorageRoot + $"{contract.ContractTemplate}{WordExtension}", destFileName);
//            var valuesToFill = new Content();
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20160101)
//            {
//                valuesToFill = new Content(
//                    new FieldContent("ContractNo", contract.ContractNo),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("SignedYear", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("SignedMonth", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("SignedDay", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("StartYear", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("StartMonth", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("StartDay", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("EndYear", contract.EndTime.Value.Year.ToString()),
//                    new FieldContent("EndMonth", contract.EndTime.Value.Month.ToString()),
//                    new FieldContent("EndDay", contract.EndTime.Value.Day.ToString()),

//                    new FieldContent("CompanyName", contract.Project.CompanyName ?? string.Empty),
//                    new FieldContent("CompanyCorporation", contract.Project.CompanyCorporation ?? string.Empty),
//                    new FieldContent("Address", contract.Project.Address ?? string.Empty),
//                    new FieldContent("CompanyTel", contract.Project.CompanyTel ?? string.Empty),

//                    new FieldContent("BName", contract.BName ?? string.Empty),
//                    new FieldContent("BIDcard", contract.BIDcard ?? string.Empty),
//                    new FieldContent("BAddress", contract.BAddress ?? string.Empty),

//                    new FieldContent("CName", contract.CName ?? string.Empty),
//                    new FieldContent("CSex", contract.CSex ?? string.Empty),
//                    new FieldContent("CRelationship", contract.CRelationship ?? string.Empty),
//                    new FieldContent("CIDcard", contract.CIDcard ?? string.Empty),
//                    new FieldContent("CPhone", contract.CPhone ?? string.Empty),
//                    new FieldContent("CTel", contract.CTel ?? string.Empty),
//                    new FieldContent("CCompany", contract.CCompany ?? string.Empty),
//                    new FieldContent("CAddress", contract.CAddress ?? string.Empty),
//                    new FieldContent("CEmail", contract.CEmail ?? string.Empty),
//                    new FieldContent("CLegalPersonCompany", contract.CLegalPersonCompany ?? string.Empty),
//                    new FieldContent("CLegalPersonName", contract.CLegalPersonName ?? string.Empty),
//                    new FieldContent("CLegalPersonAddress", contract.CLegalPersonAddress ?? string.Empty),
//                    new FieldContent("CLegalPersonEmail", contract.CLegalPersonEmail ?? string.Empty),
//                    new FieldContent("CLegalPersonContactName", contract.CLegalPersonContactName ?? string.Empty),
//                    new FieldContent("CLegalPersonPhone", contract.CLegalPersonPhone ?? string.Empty),
//                    new FieldContent("CLegalPersonTel", contract.CLegalPersonTel ?? string.Empty),


//                    new FieldContent("DName", contract.DName ?? string.Empty),
//                    new FieldContent("DSex", contract.DSex ?? string.Empty),
//                    new FieldContent("DRelationship", contract.DRelationship ?? string.Empty),
//                    new FieldContent("DIDcard", contract.DIDcard ?? string.Empty),
//                    new FieldContent("DPhone", contract.DPhone ?? string.Empty),
//                    new FieldContent("DTel", contract.DTel ?? string.Empty),
//                    new FieldContent("DCompany", contract.DCompany ?? string.Empty),
//                    new FieldContent("DAddress", contract.DAddress ?? string.Empty),
//                    new FieldContent("DEmail", contract.DEmail ?? string.Empty),
//                    new FieldContent("DLegalPersonCompany", contract.DLegalPersonCompany ?? string.Empty),
//                    new FieldContent("DLegalPersonName", contract.DLegalPersonName ?? string.Empty),
//                    new FieldContent("DLegalPersonAddress", contract.DLegalPersonAddress ?? string.Empty),
//                    new FieldContent("DLegalPersonEmail", contract.DLegalPersonEmail ?? string.Empty),
//                    new FieldContent("DLegalPersonContactName", contract.DLegalPersonContactName ?? string.Empty),
//                    new FieldContent("DLegalPersonPhone", contract.DLegalPersonPhone ?? string.Empty),
//                    new FieldContent("DLegalPersonTel", contract.DLegalPersonTel ?? string.Empty),

//                    new FieldContent("PensionAddress", contract.Project.PensionAddress ?? string.Empty),
//                    new FieldContent("ProjectFullName", contract.Project.ProjectFullName ?? string.Empty),
//                    new FieldContent("NursingType", contract.NursingType ?? string.Empty),
//                    new FieldContent("BuildingName", contract.Room.Floor.Unit.Building.Name ?? string.Empty),
//                    new FieldContent("RoomType", contract.RoomType ?? string.Empty),
//                    new FieldContent("RoomName", contract.Room.Name ?? string.Empty),
//                    new FieldContent("BedName", contract.Bed.Name ?? string.Empty),
//                    new FieldContent("CheckinType", contract.CheckinType ?? string.Empty),

//                    new FieldContent("IsCompartment", contract.IsCompartment.Value ? "包房" : "单床"),
//                    new FieldContent("DepositCost", contract.DepositCost.ToString("F2")),
//                    new FieldContent("ChDepositCost", RMB.Parse(contract.DepositCost)), //转换成大写人民币

//                    new FieldContent("ChShortTotalMonthly",
//                        RMB.Parse((contract.ShortTermMealsCost + contract.ShortTermRoomCost))),
//                    //转换成大写人民币
//                    new FieldContent("ShortTotalMonthly",
//                        (contract.ShortTermMealsCost + contract.ShortTermRoomCost).ToString(
//                            "F2")),

//                    new FieldContent("ChShortTermServiceMonthlyCost", RMB.Parse(contract.ShortTermServiceMonthlyCost)),

//                    new FieldContent("ShortTermServiceMonthlyCost", contract.ShortTermServiceMonthlyCost.ToString("F2")),

//                    new FieldContent("ChShortTermNursingCost", RMB.Parse(contract.ShortTermNursingCost)),
//                    new FieldContent("ShortTermNursingCost", contract.ShortTermNursingCost.ToString("F2")),

//                    new FieldContent("ChRelocationCost", RMB.Parse(contract.RelocationCost)),
//                    new FieldContent("RelocationCost", contract.RelocationCost.ToString("F2")),

//                    new FieldContent("ChShortTermServiceCost", RMB.Parse(contract.ShortTermServiceCost)),
//                    new FieldContent("ShortTermServiceCost", contract.ShortTermServiceCost.ToString("F2")),

//                    new FieldContent("ChLongTermServiceCost", RMB.Parse(contract.LongTermServiceCost)),
//                    new FieldContent("LongTermServiceCost", contract.LongTermServiceCost.ToString("F2")),

//                    new FieldContent("ChLongTotalMonthly",
//                        RMB.Parse(contract.LongTermMealsCost + contract.LongTermRoomCost)),
//                    //转换成大写人民币
//                    new FieldContent("LongTotalMonthly",
//                        (contract.LongTermMealsCost + contract.LongTermRoomCost).ToString(
//                            "F2")),

//                    new FieldContent("ChLongTermServiceMonthlyCost", RMB.Parse(contract.LongTermServiceMonthlyCost)),
//                    //转换成大写人民币
//                    new FieldContent("LongTermServiceMonthlyCost", contract.LongTermServiceMonthlyCost.ToString("F2")),

//                    new FieldContent("ChLongTermNursingCost", RMB.Parse(contract.LongTermNursingCost)), //转换成大写人民币
//                    new FieldContent("LongTermNursingCost", contract.LongTermNursingCost.ToString("F2")),

//                    new FieldContent("RefundCost", contract.RefundCost.ToString("F2")),
//                    new FieldContent("CompanyAccountName", contract.Project.CompanyAccountName ?? string.Empty),
//                    new FieldContent("CompanyAccount", contract.Project.CompanyAccount ?? string.Empty),
//                    new FieldContent("CompanyAccountBank", contract.Project.CompanyAccountBank ?? string.Empty));
//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170301)
//            {

//                var cmxs = contract.CAsTypes.SplitToList<int>(',').Select(x => (CAsType)x).ToList();
//                var strCAsTypes = string.Empty;
//                if (cmxs.Count > 0)
//                {
//                    if (cmxs.Contains(CAsType.付款义务人))
//                    {
//                        strCAsTypes += "☑ 付款义务人 ";
//                    }
//                    else
//                    {
//                        strCAsTypes += "□ 付款义务人 ";
//                    }
//                    if (cmxs.Contains(CAsType.连带责任保证人))
//                    {
//                        strCAsTypes += "☑ 连带责任保证人 ";
//                    }
//                    else
//                    {
//                        strCAsTypes += "□连带责任保证人 ";
//                    }
//                    if (cmxs.Contains(CAsType.联系人))
//                    {
//                        strCAsTypes += "☑ 联系人 ";
//                    }
//                    else
//                    {
//                        strCAsTypes += "□联系人 ";
//                    }
//                    if (cmxs.Contains(CAsType.代理人))
//                    {
//                        strCAsTypes += "☑ 代理人 ";
//                    }
//                    else
//                    {
//                        strCAsTypes += "□代理人 ";
//                    }
//                    if (cmxs.Contains(CAsType.其他))
//                    {
//                        strCAsTypes += "☑ 其他 ";
//                    }
//                    else
//                    {
//                        strCAsTypes += "□其他 ";
//                    }

//                }
//                else
//                {
//                    strCAsTypes = "□付款义务人 □连带责任保证人 □联系人 □代理人 □其他";
//                }

//                valuesToFill = new Content(
//                new FieldContent("ContractNo", contract.ContractNo),
//                new FieldContent("StartYear", contract.StartTime.Value.Year.ToString()),
//                new FieldContent("StartMonth", contract.StartTime.Value.Month.ToString()),
//                new FieldContent("StartDay", contract.StartTime.Value.Day.ToString()),
//                new FieldContent("EndYear", contract.EndTime.Value.Year.ToString()),
//                new FieldContent("EndMonth", contract.EndTime.Value.Month.ToString()),
//                new FieldContent("EndDay", contract.EndTime.Value.Day.ToString()),

//                new FieldContent("CompanyCorporation", contract.Project.CompanyCorporation ?? string.Empty),
//                new FieldContent("Address", contract.Project.Address ?? string.Empty),
//                new FieldContent("CompanyTel", contract.Project.CompanyTel ?? string.Empty),
//                new FieldContent("Email", contract.Project.Email ?? string.Empty),
//                new FieldContent("PostCode", contract.Project.PostCode ?? string.Empty),

//                new FieldContent("BName", contract.BName ?? string.Empty),
//                new FieldContent("BIDcard", contract.BIDcard ?? string.Empty),
//                new FieldContent("BAddress", contract.BAddress ?? string.Empty),
//                new FieldContent("BPhone", contract.BPhone ?? string.Empty),
//                new FieldContent("BEmail", contract.BEmail ?? string.Empty),
//                new FieldContent("BAge", contract.BAge.ToString() ?? string.Empty),
//                new FieldContent("BSex", contract.BSex.ToString() ?? string.Empty),
//                new FieldContent("BCredentialType", contract.BCredentialType.ToString() ?? string.Empty),
//                new FieldContent("CCredentialType", contract.CCredentialType.ToString() ?? string.Empty),
//                new FieldContent("DCredentialType", contract.DCredentialType.ToString() ?? string.Empty),
//                new FieldContent("CName", contract.CName ?? string.Empty),
//                new FieldContent("CRelationship", contract.CRelationship ?? string.Empty),
//                new FieldContent("CIDcard", contract.CIDcard ?? string.Empty),
//                new FieldContent("CPhone", contract.CPhone ?? string.Empty),
//                new FieldContent("CCompany", contract.CCompany ?? string.Empty),
//                new FieldContent("CAddress", contract.CAddress ?? string.Empty),
//                new FieldContent("CEmail", contract.CEmail ?? string.Empty),
//                new FieldContent("CLegalPersonCompany", contract.CLegalPersonCompany ?? string.Empty),
//                new FieldContent("CLegalPersonName", contract.CLegalPersonName ?? string.Empty),
//                new FieldContent("CLegalPersonAddress", contract.CLegalPersonAddress ?? string.Empty),
//                new FieldContent("CLegalPersonEmail", contract.CLegalPersonEmail ?? string.Empty),
//                new FieldContent("CLegalPersonContactName", contract.CLegalPersonContactName ?? string.Empty),
//                new FieldContent("CLegalPersonPhone", contract.CLegalPersonPhone ?? string.Empty),
//                new FieldContent("CLegalPersonTel", contract.CLegalPersonTel ?? string.Empty),
//                new FieldContent("CliveAddress", contract.CliveAddress ?? string.Empty),
//                new FieldContent("CPostcode", contract.CPostcode ?? string.Empty),
//                new FieldContent("CLegalPersonPostcode", contract.CLegalPersonPostcode ?? string.Empty),

//                new FieldContent("CAsTypes", strCAsTypes ?? string.Empty),
//                new FieldContent("DName", contract.DName ?? string.Empty),
//                new FieldContent("DRelationship", contract.DRelationship ?? string.Empty),
//                new FieldContent("DIDcard", contract.DIDcard ?? string.Empty),
//                new FieldContent("DPhone", contract.DPhone ?? string.Empty),
//                new FieldContent("DCompany", contract.DCompany ?? string.Empty),
//                new FieldContent("DAddress", contract.DAddress ?? string.Empty),
//                new FieldContent("DEmail", contract.DEmail ?? string.Empty),
//                new FieldContent("DLegalPersonCompany", contract.DLegalPersonCompany ?? string.Empty),
//                new FieldContent("DLegalPersonName", contract.DLegalPersonName ?? string.Empty),
//                new FieldContent("DLegalPersonAddress", contract.DLegalPersonAddress ?? string.Empty),
//                new FieldContent("DLegalPersonEmail", contract.DLegalPersonEmail ?? string.Empty),
//                new FieldContent("DLegalPersonContactName", contract.DLegalPersonContactName ?? string.Empty),
//                new FieldContent("DLegalPersonPhone", contract.DLegalPersonPhone ?? string.Empty),
//                new FieldContent("DLegalPersonTel", contract.DLegalPersonTel ?? string.Empty),
//                new FieldContent("DliveAddress", contract.DliveAddress ?? string.Empty),
//                new FieldContent("DPostcode", contract.DPostcode ?? string.Empty),
//                new FieldContent("DLegalPersonPostcode", contract.DLegalPersonPostcode ?? string.Empty),

//                new FieldContent("PensionAddress", contract.Project.PensionAddress ?? string.Empty),
//                new FieldContent("ProjectFullName", contract.Project.ProjectFullName ?? string.Empty),
//                new FieldContent("NursingType", contract.NursingType ?? string.Empty),
//                new FieldContent("RoomType", contract.RoomType ?? string.Empty),
//                new FieldContent("RoomName", contract.Room.Name ?? string.Empty),
//                new FieldContent("BedName", contract.Bed.Name ?? string.Empty),
//                new FieldContent("IsCompartment", contract.IsCompartment.Value ? "包房" : "单床"),
//                new FieldContent("DepositCost", $"{RMB.Parse(contract.DepositCost)}（RMB {contract.DepositCost.ToString("F2")}）"),
//                new FieldContent("RelocationCost", $"{RMB.Parse(contract.RelocationCost)}（RMB {contract.RelocationCost.ToString("F2")}）"),

//                new FieldContent("TotalCost", $"{RMB.Parse(contract.LongTermServiceCost + contract.LongTermMealsCost + contract.LongTermRoomCost + contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost + contract.LongTermAttachCost)}（RMB {(contract.LongTermServiceCost + contract.LongTermMealsCost + contract.LongTermRoomCost + contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost + contract.LongTermAttachCost).ToString("F2")}）"),
//                new FieldContent("LongTermServiceCost", $"{RMB.Parse(contract.LongTermServiceCost)}（RMB {contract.LongTermServiceCost.ToString("F2")}）"),
//                new FieldContent("LongTotalMonthly", $"{RMB.Parse(contract.LongTermMealsCost + contract.LongTermRoomCost)}（RMB {(contract.LongTermMealsCost + contract.LongTermRoomCost).ToString("F2")}）"),
//                new FieldContent("LongTermServiceMonthlyCost", $"{RMB.Parse(contract.LongTermServiceMonthlyCost)}（RMB {contract.LongTermServiceMonthlyCost.ToString("F2")}）"),
//                new FieldContent("LongTermNursingCost", $"{RMB.Parse(contract.LongTermNursingCost)}（RMB {contract.LongTermNursingCost.ToString("F2")}）"),
//                new FieldContent("LongTermAttachCost", $"{RMB.Parse(contract.LongTermAttachCost)}（RMB {contract.LongTermAttachCost.ToString("F2")}）"),
//                new FieldContent("LiquidatedDamages", $"{RMB.Parse(contract.LiquidatedDamages)}（RMB {contract.LiquidatedDamages.ToString("F2")}"),
//                new FieldContent("CompanyAccountName", contract.Project.CompanyAccountName ?? string.Empty),
//                new FieldContent("CompanyAccount", contract.Project.CompanyAccount ?? string.Empty),
//                new FieldContent("CompanyAccountBank", contract.Project.CompanyAccountBank ?? string.Empty),
//                new FieldContent("LiquidatedDamagesRatio", $"{(int)(contract.Project.LiquidatedDamagesRatio * 12 * 100)}%" ?? string.Empty));

//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801换签)
//            {
//                string strLongTermAttachCost = string.Empty;

//                if (contract.LongTermAttachCost > 0)
//                {
//                    strLongTermAttachCost = $",附加服务费为人民币  {RMB.Parse(contract.LongTermAttachCost)}（RMB {contract.LongTermAttachCost.ToString("F2")}）";
//                }

//                valuesToFill = new Content(
//                // new FieldContent("ContractNo", contract.ContractNo),
//                new FieldContent("StartYear", contract.StartTime.Value.Year.ToString()),
//                new FieldContent("StartMonth", contract.StartTime.Value.Month.ToString()),
//                new FieldContent("StartDay", contract.StartTime.Value.Day.ToString()),
//                new FieldContent("EndYear", contract.EndTime.Value.Year.ToString()),
//                new FieldContent("EndMonth", contract.EndTime.Value.Month.ToString()),
//                new FieldContent("EndDay", contract.EndTime.Value.Day.ToString()),

//                new FieldContent("CompanyCorporation", contract.Project.CompanyCorporation ?? string.Empty),
//                new FieldContent("Address", contract.Project.Address ?? string.Empty),
//                new FieldContent("CompanyTel", contract.Project.CompanyTel ?? string.Empty),

//                new FieldContent("BName", contract.BName ?? string.Empty),
//                new FieldContent("BIDcard", contract.BIDcard ?? string.Empty),
//                new FieldContent("BAddress", contract.BAddress ?? string.Empty),
//                new FieldContent("BPhone", contract.BPhone ?? string.Empty),
//                new FieldContent("BEmail", contract.BEmail ?? string.Empty),
//                new FieldContent("BAge", contract.BAge.ToString() ?? string.Empty),
//                new FieldContent("BSex", contract.BSex.ToString() ?? string.Empty),
//                new FieldContent("BCredentialType", contract.BCredentialType.ToString() ?? string.Empty),


//                new FieldContent("CName", contract.CName ?? string.Empty),
//                new FieldContent("CSex", contract.CSex.IsNullOrWhiteSpace() ? string.Empty : contract.CSex),
//                 new FieldContent("CRelationship", contract.CRelationship ?? string.Empty),
//                new FieldContent("CCredentialType", contract.CCredentialType.ToString() ?? string.Empty),
//                new FieldContent("CIDcard", contract.CIDcard ?? string.Empty),
//                 new FieldContent("CAddress", contract.CAddress ?? string.Empty),
//                new FieldContent("CliveAddress", contract.CliveAddress ?? string.Empty),
//                new FieldContent("CPhone", contract.CPhone ?? string.Empty),
//                new FieldContent("CEmail", contract.CEmail ?? string.Empty),


//                new FieldContent("DName", contract.DName ?? string.Empty),
//                new FieldContent("DCredentialType", contract.DCredentialType.ToString() ?? string.Empty),
//                new FieldContent("DRelationship", contract.DRelationship ?? string.Empty),
//                new FieldContent("DIDcard", contract.DIDcard ?? string.Empty),
//                new FieldContent("DPhone", contract.DPhone ?? string.Empty),
//                new FieldContent("DCompany", contract.DCompany ?? string.Empty),
//                new FieldContent("DAddress", contract.DAddress ?? string.Empty),
//                new FieldContent("DEmail", contract.DEmail ?? string.Empty),
//                new FieldContent("DLegalPersonCompany", contract.DLegalPersonCompany ?? string.Empty),
//                new FieldContent("DLegalPersonName", contract.DLegalPersonName ?? string.Empty),
//                new FieldContent("DLegalPersonAddress", contract.DLegalPersonAddress ?? string.Empty),
//                new FieldContent("DLegalPersonEmail", contract.DLegalPersonEmail ?? string.Empty),
//                new FieldContent("DLegalPersonContactName", contract.DLegalPersonContactName ?? string.Empty),
//                new FieldContent("DLegalPersonPhone", contract.DLegalPersonPhone ?? string.Empty),
//                new FieldContent("DliveAddress", contract.DliveAddress ?? string.Empty),

//                new FieldContent("PensionAddress", contract.Project.PensionAddress ?? string.Empty),
//                new FieldContent("ProjectFullName", contract.Project.ProjectFullName ?? string.Empty),
//                new FieldContent("NursingType", contract.NursingType ?? string.Empty),
//                new FieldContent("RoomType", contract.RoomType ?? string.Empty),
//                new FieldContent("BuildingName", contract.Room.Floor.Unit.Building.Name ?? string.Empty),
//                new FieldContent("RoomName", contract.Room.Name ?? string.Empty),
//                new FieldContent("BedName", contract.Bed.Name ?? string.Empty),
//                new FieldContent("IsCompartment", contract.IsCompartment.Value ? "包房" : "单床"),

//                 new FieldContent("DepositCost", $"{RMB.Parse(contract.DepositCost)}（RMB {contract.DepositCost.ToString("F2")}）"),
//                 new FieldContent("TotalCost", $"{RMB.Parse(contract.LongTermServiceCost + contract.LongTermMealsCost + contract.LongTermRoomCost + contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost + contract.LongTermAttachCost)}（RMB {(contract.LongTermServiceCost + contract.LongTermMealsCost + contract.LongTermRoomCost + contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost + contract.LongTermAttachCost).ToString("F2")}）"),
//                 new FieldContent("LongTermRoomCost", $"{RMB.Parse(contract.LongTermRoomCost)}（RMB {contract.LongTermRoomCost.ToString("F2")}）"),
//                 new FieldContent("LongTermMealsCost", $"{RMB.Parse(contract.LongTermMealsCost)}（RMB {contract.LongTermMealsCost.ToString("F2")}）"),
//                 new FieldContent("LongTermServiceCost", $"{RMB.Parse(contract.LongTermServiceCost)}（RMB {(contract.LongTermServiceCost).ToString("F2")}）"),
//                //GradeCost
//                new FieldContent("LiquidatedDamages", $"{RMB.Parse(contract.LiquidatedDamages)} RMB {contract.LiquidatedDamages.ToString("F2")}"),
//                new FieldContent("GradeCost", $"{RMB.Parse(contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost)}（RMB {(contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost).ToString("F2")}）"),
//                new FieldContent("LiquidatedDamagesRatio", $"{(int)(contract.Project.LiquidatedDamagesRatio * 12 * 100)}%" ?? string.Empty),

//                new FieldContent("RelocationCost", $"{RMB.Parse(contract.RelocationCost)}（RMB {contract.RelocationCost.ToString("F2")}）"),
//                new FieldContent("RefundCost", contract.RefundCost.ToString("F2")),

//                new FieldContent("LongTermAttachCost", strLongTermAttachCost),
//                new FieldContent("CompanyAccountName", contract.Project.CompanyAccountName ?? string.Empty),
//                new FieldContent("CompanyAccount", contract.Project.CompanyAccount ?? string.Empty),
//                new FieldContent("CompanyAccountBank", contract.Project.CompanyAccountBank ?? string.Empty)
//                );
//            }

//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801新签)
//            {
//                string strLongTermAttachCost = string.Empty;

//                if (contract.LongTermAttachCost > 0)
//                {
//                    strLongTermAttachCost = $",附加服务费为人民币  {RMB.Parse(contract.LongTermAttachCost)}（RMB {contract.LongTermAttachCost.ToString("F2")}）";
//                }
//                string reNewStr= string.Empty;
//                if (contract.SignedType != SignedType.续签)
//                {
//                    reNewStr = "乙方首次入住 30 日内（不含30日）不适应居住环境或管理方式的；乙方有权解除合同且不承担违约责任。";
//                }

//                valuesToFill = new Content(
//                // new FieldContent("ContractNo", contract.ContractNo),
//                new FieldContent("StartYear", contract.StartTime.Value.Year.ToString()),
//                new FieldContent("StartMonth", contract.StartTime.Value.Month.ToString()),
//                new FieldContent("StartDay", contract.StartTime.Value.Day.ToString()),
//                new FieldContent("EndYear", contract.EndTime.Value.Year.ToString()),
//                new FieldContent("EndMonth", contract.EndTime.Value.Month.ToString()),
//                new FieldContent("EndDay", contract.EndTime.Value.Day.ToString()),

//                new FieldContent("CompanyCorporation", contract.Project.CompanyCorporation ?? string.Empty),
//                new FieldContent("Address", contract.Project.Address ?? string.Empty),
//                new FieldContent("CompanyTel", contract.Project.CompanyTel ?? string.Empty),

//                new FieldContent("BName", contract.BName ?? string.Empty),
//                new FieldContent("BIDcard", contract.BIDcard ?? string.Empty),
//                new FieldContent("BAddress", contract.BAddress ?? string.Empty),
//                new FieldContent("BPhone", contract.BPhone ?? string.Empty),
//                new FieldContent("BEmail", contract.BEmail ?? string.Empty),
//                new FieldContent("BAge", contract.BAge.ToString() ?? string.Empty),
//                new FieldContent("BSex", contract.BSex.ToString() ?? string.Empty),
//                new FieldContent("BCredentialType", contract.BCredentialType.ToString() ?? string.Empty),


//                new FieldContent("CName", contract.CName ?? string.Empty),
//                new FieldContent("CSex", contract.CSex.IsNullOrWhiteSpace() ? string.Empty : contract.CSex),
//                 new FieldContent("CRelationship", contract.CRelationship ?? string.Empty),
//                new FieldContent("CCredentialType", contract.CCredentialType.ToString() ?? string.Empty),
//                new FieldContent("CIDcard", contract.CIDcard ?? string.Empty),
//                 new FieldContent("CAddress", contract.CAddress ?? string.Empty),
//                new FieldContent("CliveAddress", contract.CliveAddress ?? string.Empty),
//                new FieldContent("CPhone", contract.CPhone ?? string.Empty),
//                new FieldContent("CEmail", contract.CEmail ?? string.Empty),


//                new FieldContent("DName", contract.DName ?? string.Empty),
//                new FieldContent("DCredentialType", contract.DCredentialType.ToString() ?? string.Empty),
//                new FieldContent("DRelationship", contract.DRelationship ?? string.Empty),
//                new FieldContent("DIDcard", contract.DIDcard ?? string.Empty),
//                new FieldContent("DPhone", contract.DPhone ?? string.Empty),
//                new FieldContent("DCompany", contract.DCompany ?? string.Empty),
//                new FieldContent("DAddress", contract.DAddress ?? string.Empty),
//                new FieldContent("DEmail", contract.DEmail ?? string.Empty),
//                new FieldContent("DLegalPersonCompany", contract.DLegalPersonCompany ?? string.Empty),
//                new FieldContent("DLegalPersonName", contract.DLegalPersonName ?? string.Empty),
//                new FieldContent("DLegalPersonAddress", contract.DLegalPersonAddress ?? string.Empty),
//                new FieldContent("DLegalPersonEmail", contract.DLegalPersonEmail ?? string.Empty),
//                new FieldContent("DLegalPersonContactName", contract.DLegalPersonContactName ?? string.Empty),
//                new FieldContent("DLegalPersonPhone", contract.DLegalPersonPhone ?? string.Empty),
//                new FieldContent("DliveAddress", contract.DliveAddress ?? string.Empty),

//                new FieldContent("PensionAddress", contract.Project.PensionAddress ?? string.Empty),
//                new FieldContent("ProjectFullName", contract.Project.ProjectFullName ?? string.Empty),
//                new FieldContent("NursingType", contract.NursingType ?? string.Empty),
//                new FieldContent("RoomType", contract.RoomType ?? string.Empty),
//                new FieldContent("BuildingName", contract.Room.Floor.Unit.Building.Name ?? string.Empty),
//                new FieldContent("RoomName", contract.Room.Name ?? string.Empty),
//                new FieldContent("BedName", contract.Bed.Name ?? string.Empty),
//                new FieldContent("IsCompartment", contract.IsCompartment.Value ? "包房" : "单床"),

//                 new FieldContent("DepositCost", $"{RMB.Parse(contract.DepositCost)}（RMB {contract.DepositCost.ToString("F2")}）"),
//                 new FieldContent("TotalCost", $"{RMB.Parse(contract.LongTermServiceCost + contract.LongTermMealsCost + contract.LongTermRoomCost + contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost + contract.LongTermAttachCost)}（RMB {(contract.LongTermServiceCost + contract.LongTermMealsCost + contract.LongTermRoomCost + contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost + contract.LongTermAttachCost).ToString("F2")}）"),
//                 new FieldContent("LongTermRoomCost", $"{RMB.Parse(contract.LongTermRoomCost)}（RMB {contract.LongTermRoomCost.ToString("F2")}）"),
//                 new FieldContent("LongTermMealsCost", $"{RMB.Parse(contract.LongTermMealsCost)}（RMB {contract.LongTermMealsCost.ToString("F2")}）"),
//                 new FieldContent("LongTermServiceCost", $"{RMB.Parse(contract.LongTermServiceCost)}（RMB {(contract.LongTermServiceCost).ToString("F2")}）"),
//                //GradeCost
//                new FieldContent("GradeCost", $"{RMB.Parse(contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost)}（RMB {(contract.LongTermServiceMonthlyCost + contract.LongTermNursingCost).ToString("F2")}）"),
//                new FieldContent("LiquidatedDamages", $"{RMB.Parse(contract.LiquidatedDamages)} RMB {contract.LiquidatedDamages.ToString("F2")}"),
//                new FieldContent("LiquidatedDamagesRatio", $"{(int)(contract.Project.LiquidatedDamagesRatio * 12 * 100)}%" ?? string.Empty),

//                new FieldContent("RelocationCost", $"{RMB.Parse(contract.RelocationCost)}（RMB {contract.RelocationCost.ToString("F2")}）"),
//                new FieldContent("RefundCost", contract.RefundCost.ToString("F2")),
//                new FieldContent("ReNewStr",reNewStr),
//                new FieldContent("LongTermAttachCost", strLongTermAttachCost),
//                new FieldContent("CompanyAccountName", contract.Project.CompanyAccountName ?? string.Empty),
//                new FieldContent("CompanyAccount", contract.Project.CompanyAccount ?? string.Empty),
//                new FieldContent("CompanyAccountBank", contract.Project.CompanyAccountBank ?? string.Empty)
//                );
//            }

//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }
//            var targetPath = $"{contract.ContractNo}{contract.BName}{PdfExtension}";
//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        public string FromContractRoomChange(int contractRoomChangeId)
//        {
//            var contractRoomChange = _repository.Get<ContractRoomChange>(contractRoomChangeId);
//            var contract = contractRoomChange.Contract;

//            string destFileName = PrintStorageRoot + $"{contractRoomChange.ContractRoomChangeNo}{contract.BName}{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            var valuesToFill = new Content();
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20160101)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractRoomChangeTemplateName}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//                new FieldContent("ContractRoomChangeNo", contractRoomChange.ContractRoomChangeNo),
//                new FieldContent("BName", contract.BName ?? string.Empty),
//                new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                new FieldContent("ContractNo", contract.ContractNo),

//                new FieldContent("ChangeDate", contractRoomChange.ChangeDate.ToString("yyyy年MM月dd日")),
//                new FieldContent("CheckInType", contractRoomChange.NewIsCompartment ? "包房" : "单床"),
//                new FieldContent("NewRoomType", contractRoomChange.NewRoomType ?? string.Empty),
//                new FieldContent("RoomName", contractRoomChange.NewRoom.Name ?? string.Empty),
//                new FieldContent("BedName", contractRoomChange.NewBed?.Name ?? string.Empty),
//                new FieldContent("ShortTotalMonthlyCost",
//                    $"{RMB.Parse(contractRoomChange.ShortRoomRate + contractRoomChange.ShortMeals)}（RMB {(contractRoomChange.ShortRoomRate + contractRoomChange.ShortMeals).ToString("F2")}）"),
//                new FieldContent("LongTotalMonthlyCost",
//                    $"{RMB.Parse(contractRoomChange.LongRoomRate + contractRoomChange.LongMeals)}（RMB {(contractRoomChange.LongRoomRate + contractRoomChange.LongMeals).ToString("F2")}）"));
//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170301)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractRoomChangeTemplateName20170301}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//               new FieldContent("ContractRoomChangeNo", contractRoomChange.ContractRoomChangeNo),
//               new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//               new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//               new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//               new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//               new FieldContent("ContractNo", contract.ContractNo),

//               new FieldContent("ChangeDate", contractRoomChange.ChangeDate.ToString("yyyy年MM月dd日")),
//               new FieldContent("CheckInType", contractRoomChange.NewIsCompartment ? "包房" : "单床"),
//               new FieldContent("NewRoomType", contractRoomChange.NewRoomType ?? string.Empty),
//               new FieldContent("RoomName", contractRoomChange.NewRoom.Name ?? string.Empty),
//               new FieldContent("BedName", contractRoomChange.NewBed?.Name ?? string.Empty),
//               new FieldContent("LongTotalMonthlyCost",
//                   $"{RMB.Parse(contractRoomChange.LongRoomRate + contractRoomChange.LongMeals)}（RMB {(contractRoomChange.LongRoomRate + contractRoomChange.LongMeals).ToString("F2")}）"));
//            }

//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801新签 || contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801换签)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractRoomChangeTemplateName20170801}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//               new FieldContent("ContractRoomChangeNo", contractRoomChange.ContractRoomChangeNo),
//               new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//               new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//               new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//               new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//               new FieldContent("ContractNo", contract.ContractNo),

//               new FieldContent("ChangeDate", contractRoomChange.ChangeDate.ToString("yyyy年MM月dd日")),
//               new FieldContent("CheckInType", contractRoomChange.NewIsCompartment ? "包房" : "单床"),
//               new FieldContent("NewRoomType", contractRoomChange.NewRoomType ?? string.Empty),
//               new FieldContent("RoomName", contractRoomChange.NewRoom.Name ?? string.Empty),
//               new FieldContent("BedName", contractRoomChange.NewBed?.Name ?? string.Empty),
//               new FieldContent("LongRoomRate",
//                   $"{RMB.Parse(contractRoomChange.LongRoomRate)}（RMB {contractRoomChange.LongRoomRate.ToString("F2")}）"));
//            }
//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath = $"{contractRoomChange.ContractRoomChangeNo}{contract.BName}{PdfExtension}";
//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        public string FromContractServicePackChange(int contractServicePackChangeId)
//        {
//            var contractServicePackChange = _repository.Get<ContractServicePackChange>(contractServicePackChangeId);
//            var contract = contractServicePackChange.Contract;

//            string destFileName = PrintStorageRoot + $"{contractServicePackChange.ContractServicePackChangeNo}{contract.BName}{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            var valuesToFill = new Content();
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20160101)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractServicePackChangeTemplateName}{WordExtension}", destFileName);

//                valuesToFill = new Content(
//                    new FieldContent("ContractServicePackChangeNo",
//                        contractServicePackChange.ContractServicePackChangeNo),
//                    new FieldContent("BName", contract.BName ?? string.Empty),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),

//                    new FieldContent("ChangeDate", contractServicePackChange.ChangeDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("ServiceLevel", contractServicePackChange.ConcernType ?? string.Empty),
//                    new FieldContent("ShortServiceMonthlyAmount",
//                        $"{RMB.Parse(contractServicePackChange.ShortServiceMonthlyAmount)}（RMB {contractServicePackChange.ShortServiceMonthlyAmount.ToString("F2")}）"),
//                    new FieldContent("ShortConcernAmount",
//                        $"{RMB.Parse(contractServicePackChange.ShortConcernAmount)}（RMB {contractServicePackChange.ShortConcernAmount.ToString("F2")}）"),
//                    new FieldContent("LongServiceMonthlyAmount",
//                        $"{RMB.Parse(contractServicePackChange.LongServiceMonthlyAmount)}（RMB {contractServicePackChange.LongServiceMonthlyAmount.ToString("F2")}）"),
//                    new FieldContent("LongConcernAmount",
//                        $"{RMB.Parse(contractServicePackChange.LongConcernAmount)}（RMB {contractServicePackChange.LongConcernAmount.ToString("F2")}）"));
//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170301)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractServicePackChangeTemplateName20170301}{WordExtension}", destFileName);

//                valuesToFill = new Content(
//                    new FieldContent("ContractServicePackChangeNo",
//                        contractServicePackChange.ContractServicePackChangeNo),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),

//                    new FieldContent("ChangeDate", contractServicePackChange.ChangeDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("ServiceLevel", contractServicePackChange.ConcernType ?? string.Empty),
//                    new FieldContent("LongServiceMonthlyAmount",
//                        $"{RMB.Parse(contractServicePackChange.LongServiceMonthlyAmount)}（RMB {contractServicePackChange.LongServiceMonthlyAmount.ToString("F2")}）"),
//                    new FieldContent("LongConcernAmount",
//                        $"{RMB.Parse(contractServicePackChange.LongConcernAmount)}（RMB {contractServicePackChange.LongConcernAmount.ToString("F2")}）"),
//                    new FieldContent("LongAttachAmount",
//                       $"{RMB.Parse(contractServicePackChange.LongAttachAmount)}（RMB {contractServicePackChange.LongAttachAmount.ToString("F2")}）"));

//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801新签 || contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801换签)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractServicePackChangeTemplateName20170801}{WordExtension}", destFileName);

//                valuesToFill = new Content(
//                    new FieldContent("ContractServicePackChangeNo",
//                        contractServicePackChange.ContractServicePackChangeNo),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),

//                    new FieldContent("ChangeDate", contractServicePackChange.ChangeDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("ServiceLevel", contractServicePackChange.ConcernType ?? string.Empty),
//                    new FieldContent("LongServiceMonthlyAmount",
//                        $"{RMB.Parse(contractServicePackChange.LongServiceMonthlyAmount+contractServicePackChange.LongConcernAmount)}（RMB {(contractServicePackChange.LongServiceMonthlyAmount+contractServicePackChange.LongConcernAmount).ToString("F2")}）"),
//                    new FieldContent("LongAttachAmount",
//                       $"{RMB.Parse(contractServicePackChange.LongAttachAmount)}（RMB {contractServicePackChange.LongAttachAmount.ToString("F2")}）"));

//            }

//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath =
//                $"{contractServicePackChange.ContractServicePackChangeNo}{contract.BName}{PdfExtension}";
//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        public string FromContractCostChange(int contractCostChangeId)
//        {
//            var contractCostChange = _repository.Get<ContractCostChange>(contractCostChangeId);
//            var contract = contractCostChange.Contract;

//            string destFileName = PrintStorageRoot + $"{contractCostChange.ContractCostChangeNo}{contract.BName}{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            var valuesToFill = new Content();
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20160101)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractCostChangeTemplateName}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//                    new FieldContent("ContractCostChangeNo", contractCostChange.ContractCostChangeNo),
//                    new FieldContent("BName", contract.BName ?? string.Empty),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),

//                    new FieldContent("ChangeDate", contractCostChange.ChangeDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("ChangeEndDate", contractCostChange.ChangeEndDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("Description", contractCostChange.Description ?? string.Empty),
//                    new FieldContent("Type", contractCostChange.ChangeLimit > 0 ? "增加" : "减免"),
//                    new FieldContent("ChangeLimit",
//                        $"{RMB.Parse(contractCostChange.ChangeLimit)}（RMB {Math.Abs(contractCostChange.ChangeLimit).ToString("F2")}）"));
//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170301 && contractCostChange.ChangeType == ChangeType.定额变更)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractCostChangeTemplateName20170301}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//                    new FieldContent("ContractCostChangeNo", contractCostChange.ContractCostChangeNo),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),

//                    new FieldContent("ChangeDate", contractCostChange.ChangeDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("Type", contractCostChange.ChangeLimit > 0 ? "增加" : "减免"),
//                    new FieldContent("ChangeLimit",
//                        $"{RMB.Parse(contractCostChange.ChangeLimit)}（RMB {Math.Abs(contractCostChange.ChangeLimit).ToString("F2")}）"));
//            }
//            if (contract.ContractTemplate == ContractTemplate.养老机构服务合同20170301 && contractCostChange.ChangeType == ChangeType.比例变更)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractCostChangeTypeTemplateName20170301}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//                    new FieldContent("ContractCostChangeNo", contractCostChange.ContractCostChangeNo),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),
//                    new FieldContent("ChangeDate", contractCostChange.ChangeDate.ToString("yyyy年MM月dd日")));
//            }
//            if ((contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801新签 || contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801换签) && contractCostChange.ChangeType==ChangeType.定额变更)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractCostChangeTemplateName20170801}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//                    new FieldContent("ContractCostChangeNo", contractCostChange.ContractCostChangeNo),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),
//                    new FieldContent("Type", contractCostChange.ChangeLimit > 0 ? "增加" : "减免"),
//                    new FieldContent("ChangeRoomCost",
//                        $"{RMB.Parse(contractCostChange.ChangeRoomCost)}（RMB {Math.Abs(contractCostChange.ChangeRoomCost).ToString("F2")}）"),
//                    new FieldContent("ChangeDate", contractCostChange.ChangeDate.ToString("yyyy年MM月dd日")));
                
//            }
//            if ((contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801新签 || contract.ContractTemplate == ContractTemplate.养老机构服务合同20170801换签) && contractCostChange.ChangeType == ChangeType.比例变更)
//            {
//                File.Copy(TemplateStorageRoot + $"{ContractCostChangeTypeTemplateName20170801}{WordExtension}", destFileName);
//                valuesToFill = new Content(
//                    new FieldContent("ContractCostChangeNo", contractCostChange.ContractCostChangeNo),
//                    new FieldContent("Year", contract.StartTime.Value.Year.ToString()),
//                    new FieldContent("Month", contract.StartTime.Value.Month.ToString()),
//                    new FieldContent("Day", contract.StartTime.Value.Day.ToString()),
//                    new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                    new FieldContent("ContractNo", contract.ContractNo),
//                    new FieldContent("ChangeRatio", $"{(1+contractCostChange.ChangeRatio)*10}"),
//                    new FieldContent("ChangeEndDate", contractCostChange.ChangeEndDate.ToString("yyyy年MM月dd日")),
//                    new FieldContent("ChangeDate", contractCostChange.ChangeDate.ToString("yyyy年MM月dd日")));

//            }
//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath = $"{contractCostChange.ContractCostChangeNo}{contract.BName}{PdfExtension}";

//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        public string FromCheckOutRefund(int checkOutRefundId)
//        {
//            var customerAccountCheckoutRefund = _repository.Get<CustomerAccountCheckOutRefund>(checkOutRefundId);

//            var contract = _repository.Query<Contract>()
//                .Where(x => x.Status == ContractStatus.失效 && x.CustomerAccount.Id == customerAccountCheckoutRefund.CustomerAccount.Id)
//                .OrderByDescending(x => x.ActualEndTime)
//                .FirstOrDefault();

//            if (contract == null)
//            {
//                throw new DomainValidationException("打印失败，未找到客户合同信息");
//            }

//            string destFileName = PrintStorageRoot + $"{customerAccountCheckoutRefund.CustomerAccount.Customer.CustomerNo}{customerAccountCheckoutRefund.CustomerAccount.Customer.Name}-退费申请单{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            File.Copy(TemplateStorageRoot + $"{CustomerAccountCheckOutRefundTemplateName}{WordExtension}", destFileName);

//            var valuesToFill = new Content(
//                new FieldContent("CustomerName", customerAccountCheckoutRefund.CustomerAccount.Customer.Name),
//                new FieldContent("ProjectName", customerAccountCheckoutRefund.CustomerAccount.Project.Name),
//                new FieldContent("RoomNo", contract.ActualRoom.Name),
//                new FieldContent("ContractNo", contract.ContractNo),
//                new FieldContent("Balance",
//                    $"{RMB.Parse(customerAccountCheckoutRefund.CustomerAccount.Balance)}（大写） （RMB {Math.Abs(customerAccountCheckoutRefund.CustomerAccount.Balance).ToString("F2")}）元"),

//                new FieldContent("RefundName", customerAccountCheckoutRefund.RefundName ?? string.Empty),
//                new FieldContent("RefundAccountNo", customerAccountCheckoutRefund.RefundAccountNo ?? string.Empty),
//                new FieldContent("RefundBank", customerAccountCheckoutRefund.RefundBank ?? string.Empty),

//                new FieldContent("RefundName2", customerAccountCheckoutRefund.RefundName2 ?? string.Empty),
//                new FieldContent("RefundAccountNo2", customerAccountCheckoutRefund.RefundAccountNo2 ?? string.Empty),
//                new FieldContent("RefundBank2", customerAccountCheckoutRefund.RefundBank2 ?? string.Empty));

//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath = $"{customerAccountCheckoutRefund.CustomerAccount.Customer.CustomerNo}{customerAccountCheckoutRefund.CustomerAccount.Customer.Name}-退费申请单{PdfExtension}";

//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }
//        /// <summary>
//        /// 打印委托书-转账
//        /// </summary>
//        /// <param name="customerAccountTransferId"></param>
//        /// <returns></returns>
//        public string FromCustomerAccountTransfer(int customerAccountTransferId)
//        {
//            var customerAccountTransfer = _repository.Get<CustomerAccountTransfer>(customerAccountTransferId);

//            var contract = _repository.Query<Contract>()
//               .Where(x =>(x.Status == ContractStatus.生效 || x.Status == ContractStatus.失效) && x.CustomerAccount.Id == customerAccountTransfer.TransferFrom.Id)
//               .OrderByDescending(x => x.ActualEndTime)
//               .FirstOrDefault();

//            if (contract == null)
//            {
//                throw new DomainValidationException("打印失败，未找到客户合同信息");
//            }

//            var transferTocontract = _repository.Query<Contract>()
//               .Where(x => (x.Status == ContractStatus.生效 || x.Status == ContractStatus.失效) && x.CustomerAccount.Id == customerAccountTransfer.TransferTo.Id)
//               .OrderByDescending(x => x.ActualEndTime)
//               .FirstOrDefault();

//            if (transferTocontract == null)
//            {
//                throw new DomainValidationException("打印失败，未找到客户收款人合同信息");
//            }

//            string destFileName = PrintStorageRoot + $"{customerAccountTransfer.TransferFrom.Customer.CustomerNo}{customerAccountTransfer.TransferFrom.Customer.Name}-转款委托书{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            File.Copy(TemplateStorageRoot + $"{CustomerAccountTransferTemplateName}{WordExtension}", destFileName);

//            var valuesToFill = new Content(
//                new FieldContent("CustomerName", customerAccountTransfer.TransferFrom.Customer.Name),
//                new FieldContent("ProjectName", customerAccountTransfer.TransferFrom.Project.ProjectFullName),
//                new FieldContent("RoomNo", contract.ActualRoom.Name),
//                new FieldContent("ToCustomerName", customerAccountTransfer.TransferTo.Customer.Name),
//                new FieldContent("ToProjectName", customerAccountTransfer.TransferTo.Project.ProjectFullName),
//                new FieldContent("ToRoomNo", transferTocontract.ActualRoom.Name),
//                new FieldContent("Balance", $"{Math.Abs(customerAccountTransfer.TransferAmount).ToString("F2")}元（大写金额：人民币{RMB.Parse(customerAccountTransfer.TransferAmount)}）")
                    
//                    );
//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath = $"{customerAccountTransfer.TransferFrom.Customer.CustomerNo}{customerAccountTransfer.TransferFrom.Customer.Name}-转款委托书{PdfExtension}";

//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        /// <summary>
//        /// 打印退款转院协议
//        /// </summary>
//        /// <param name="checkOutRefundId"></param>
//        /// <returns></returns>
//        public string FromCheckOutAgreement(int checkOutRefundId)
//        {
//            var customerAccountCheckoutRefund = _repository.Get<CustomerAccountCheckOutRefund>(checkOutRefundId);

//            var contract = _repository.Query<Contract>()
//                .Where(x => x.Status == ContractStatus.失效 && x.CustomerAccount.Id == customerAccountCheckoutRefund.CustomerAccount.Id)
//                .OrderByDescending(x => x.ActualEndTime)
//                .FirstOrDefault();

//            if (contract == null)
//            {
//                throw new DomainValidationException("打印失败，未找到客户合同信息");
//            }

//            string destFileName = PrintStorageRoot + $"{customerAccountCheckoutRefund.CustomerAccount.Customer.CustomerNo}{customerAccountCheckoutRefund.CustomerAccount.Customer.Name}-客户退住转院协议{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            File.Copy(TemplateStorageRoot + $"{CustomerAccountCheckOutAgreementTemplateName}{WordExtension}", destFileName);

//            var valuesToFill = new Content(
//                new FieldContent("ContractNo", contract.ContractNo),
//                new FieldContent("StartYear", contract.SignedOn.Value.Year.ToString()),
//                new FieldContent("StartMonth", contract.SignedOn.Value.Month.ToString()),
//                new FieldContent("StartDay", contract.SignedOn.Value.Day.ToString()),
//                new FieldContent("EndYear", contract.ActualEndTime.Value.Year.ToString()),
//                new FieldContent("EndMonth", contract.ActualEndTime.Value.Month.ToString()),
//                new FieldContent("EndDay", contract.ActualEndTime.Value.Day.ToString()),
//                 new FieldContent("City", contract.Project.City?.ShortName ?? string.Empty),
//                 new FieldContent("CompanyName", customerAccountCheckoutRefund.RefundProject.CompanyName ?? string.Empty),
//                new FieldContent("Balance",
//                    $"{Math.Abs(customerAccountCheckoutRefund.CustomerAccount.Balance).ToString("F2")}元（大写：人民币{RMB.Parse(customerAccountCheckoutRefund.CustomerAccount.Balance)}）"),
//                new FieldContent("ProjectFullName",customerAccountCheckoutRefund.RefundProject.ProjectFullName??string.Empty),
//                new FieldContent("CompanyAccountName", customerAccountCheckoutRefund.RefundProject.CompanyAccountName ?? string.Empty),
//                new FieldContent("CompanyAccount", customerAccountCheckoutRefund.RefundProject.CompanyAccount ?? string.Empty),
//                new FieldContent("CompanyAccountBank", customerAccountCheckoutRefund.RefundProject.CompanyAccountBank ?? string.Empty));

//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath = $"{customerAccountCheckoutRefund.CustomerAccount.Customer.CustomerNo}{customerAccountCheckoutRefund.CustomerAccount.Customer.Name}-客户退住转院协议{PdfExtension}";

//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        public string FromCustomerInfo(int customerAccountId)
//        {
//            var customer = _customerQueryService.GetCheckInCustomer(customerAccountId);
//            var customerAccount = _repository.Get<CustomerAccount>(customerAccountId);

//            if (customer == null)
//            {
//                throw new DomainValidationException("打印失败，未找到客户信息");
//            }

//            string destFileName = PrintStorageRoot + $"{customer.CustomerName}-基本信息{WordExtension}";
//            if (File.Exists(destFileName))
//            {
//                File.Delete(destFileName);
//            }
//            File.Copy(TemplateStorageRoot + $"{CustomerBasicInfo}{WordExtension}", destFileName);
//            var checkedInDate = _contractQueryService
//                .QueryReNewContract(_contractQueryService.Get(customerAccount.Contracts.FirstOrDefault().Id))
//                .StartTime.Value.ToShortDateString();
//            var valuesToFill = new Content(
//                new FieldContent("RoomNo", customer.RoomName),
//                new FieldContent("CustomerName", customer.CustomerName),
//                new FieldContent("Age", customer.Birthday.ToAge().ToString()),
//                new FieldContent("Sex", customer.Sex),
//                new FieldContent("National", customer.Nationality),
//                new FieldContent("Faith", customer.Faith),
//                new FieldContent("CheckedIndate", checkedInDate),
//                new FieldContent("Diagnosis", customer.Diagnosis),
//                new FieldContent("DesignatedHospital", customer.DesignatedHospital),
//                new FieldContent("MainlyPharmacy", customer.MainlyPharmacy),
//                new FieldContent("PotentialRisk", customer.PotentialRisk),
//                new FieldContent("Education", customer.Education),
//                new FieldContent("Language", customer.Language),
//                new FieldContent("Work", customer.Work),
//                new FieldContent("Character", customer.Character),
//                new FieldContent("TabooSubject", customer.TabooSubject),
//                new FieldContent("FavouritePerson", customer.FavouritePerson),
//                new FieldContent("FavouriteFood", customer.FavouriteFood),
//                new FieldContent("FavouriteThing", customer.FavouriteThing),
//                new FieldContent("FavouritePlace", customer.FavouritePlace),
//                new FieldContent("Hearing", customer.Hearing),
//                new FieldContent("Vision", customer.Vision),
//                new FieldContent("Taste", customer.Taste),
//                new FieldContent("Memory", customer.Memory),
//                new FieldContent("Direction", customer.Direction),
//                new FieldContent("Decision", customer.Decision),
//                new FieldContent("Hygiene", customer.Hygiene),
//                new FieldContent("Movement", customer.Movement),
//                new FieldContent("MedicineControl", customer.MedicineControl),
//                new FieldContent("Bath", customer.Bath),
//                new FieldContent("Transfermation", customer.Transfermation),
//                new FieldContent("Eating", customer.Eating),
//                new FieldContent("Laundry", customer.Laundry),
//                new FieldContent("Dress", customer.Dress),
//                new FieldContent("UseToilet", customer.UseToilet),
//                new FieldContent("WakeUpTime", customer.WakeUpTime),
//                new FieldContent("SiestaTime", customer.SiestaTime),
//                new FieldContent("SleepTime", customer.SleepTime),
//                new FieldContent("DrinkAlcohol", customer.DrinkAlcohol),
//                new FieldContent("Smoke", customer.Smoke),
//                new FieldContent("AllergyHistory", customer.AllergyHistory),
//                new FieldContent("Hobby", customer.Hobby)
//                );

//            using (var outputDocument = new TemplateProcessor(destFileName)
//                .SetRemoveContentControls(true))
//            {
//                outputDocument.FillContent(valuesToFill);
//                outputDocument.SaveChanges();
//            }

//            var targetPath = $"{customer.CustomerName}-基本信息{PdfExtension}";

//            if (Parse(destFileName, PrintStorageRoot + targetPath))
//            {
//                File.Delete(destFileName);
//            }
//            else
//            {
//                throw new DomainValidationException("打印失败");
//            }
//            return targetPath;
//        }

//        public bool Parse(string sourcePath, string targetPath)
//        {
//            bool result = false;
//            WdExportFormat exportFormat = WdExportFormat.wdExportFormatPDF;
//            object paramMissing = Type.Missing;
//            ApplicationClass wordApplication = new ApplicationClass();
//            Document wordDocument = null;
//            try
//            {
//                object paramSourceDocPath = sourcePath;
//                string paramExportFilePath = targetPath;

//                WdExportFormat paramExportFormat = exportFormat;
//                bool paramOpenAfterExport = false;
//                WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForPrint;
//                WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
//                int paramStartPage = 0;
//                int paramEndPage = 0;
//                WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;
//                bool paramIncludeDocProps = true;
//                bool paramKeepIRM = true;
//                WdExportCreateBookmarks paramCreateBookmarks = WdExportCreateBookmarks.wdExportCreateWordBookmarks;
//                bool paramDocStructureTags = true;
//                bool paramBitmapMissingFonts = true;
//                bool paramUseISO19005_1 = false;

//                wordDocument = wordApplication.Documents.Open(
//                ref paramSourceDocPath, ref paramMissing, ref paramMissing,
//                ref paramMissing, ref paramMissing, ref paramMissing,
//                ref paramMissing, ref paramMissing, ref paramMissing,
//                ref paramMissing, ref paramMissing, ref paramMissing,
//                ref paramMissing, ref paramMissing, ref paramMissing,
//                ref paramMissing);

//                if (wordDocument != null)
//                    wordDocument.ExportAsFixedFormat(paramExportFilePath,
//                    paramExportFormat, paramOpenAfterExport,
//                    paramExportOptimizeFor, paramExportRange, paramStartPage,
//                    paramEndPage, paramExportItem, paramIncludeDocProps,
//                    paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
//                    paramBitmapMissingFonts, paramUseISO19005_1,
//                    ref paramMissing);
//                result = true;
//            }
//            catch (Exception ex)
//            {
//                result = false;
//            }
//            finally
//            {
//                if (wordDocument != null)
//                {
//                    wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
//                    wordDocument = null;
//                }
//                if (wordApplication != null)
//                {
//                    wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
//                    wordApplication = null;
//                }
//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//            }
//            return result;
//        }
//    }
//}
