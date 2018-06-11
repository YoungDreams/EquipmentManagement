using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundation.Core;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.CustomerFamily;

namespace PensionInsurance.Web.Views.Customer
{
    public class DetailViewModel
    {
        private readonly UrlHelper _urlHelper;
        public DetailViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public CreateViewModel CustomerFamilyCreateViewModel { get; set; }
        public int CustomerAccountId { get; set; }
        public int CustomerId { get; set; }
        public CustomerCheckInDetail CustomerInfo { get; set; }
        public IEnumerable<Entities.CustomerFamily> CustomerFamilyList { get; set; }
        public IEnumerable<CustomerLeave> CustomerLeaveList { get; set; }
        public IEnumerable<ServiceRecordDetail> CustomerServiceRecords { get; set; }
        public IEnumerable<ContractDetail> CustomerContracts { get; set; }
        public IEnumerable<CustomerPayment> CustomerPaymentList { get; set; }
        public IEnumerable<CustomerPoint> CustomerPointList { get; set; }
        public IEnumerable<Entities.CustomerAccountCheckOutRefund> CustomerRefundList { get; set; }
        public IEnumerable<Entities.CustomerAccountTransfer> CustomerAccountTransferList { get; set; }
        public IEnumerable<CustomerBillDetail> CustomerBillDetails { get; set; }
        public IEnumerable<Entities.ConsultingOrderFood> CashPaidOrderFoods { get; set; }
        public IEnumerable<Entities.ContractCostChange> ContractCostChanges { get; set; }
        public IEnumerable<Entities.CustomerMedicalAdvice> CustomerMedicalAdvices { get; set; }
        public IEnumerable<Entities.CustomerPrescription> CustomerPrescriptions { get; set; }
        public IEnumerable<Entities.ContractServicePackChange> ContractServicePackChanges { get; set; }
        public IEnumerable<ContractRoomChangeDetail> ContractRoomChanges { get; set; }
        public IEnumerable<CustomerLivingHistory> CustomerLivingHistories { get; set; }
        public IEnumerable<CustomerExpenseHistory> CustomerExpenseHistories { get; set; }
        public IEnumerable<CustomerExpenseHistory> Test_CustomerExpenseHistories { get; set; }
        public ContractDetail ActivatedContract { get; set; }
        public CustomerLeave ActivatedCustomerLeave { get; set; }
        public CustomerType CustomerType { get; set; }
        public RefundType RefundType { get; set; }
        public object DeleteCustomerServiceRecordCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "CustomerServiceRecord"),
                Command = new DeleteCustomerServiceRecordCommand { ServiceRecordId = id }
            };
        }

        public object DeleteCashPaidOrderFoodCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "CustomerOrderFood"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public object NurseConfirmedMedicalAdviceCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("NurseConfirmedMedicalAdvice", "CustomerMedicalAdvice"),
                Command = new NurseConfirmedMedicalAdviceCommand { CustomerMedicalAdviceId = id }
            };
        }

        public object NurseConfirmedDiscardMedicalAdviceCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DiscardConfirm", "CustomerMedicalAdvice"),
                Command = new NurseConfirmedDiscardMedicalAdviceCommand { CustomerMedicalAdviceId = id }
            };
        }

        public object GenerateDrugsOrderMedicalAdviceCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("GenerateDrugsOrderMedicalAdvice", "CustomerMedicalAdvice"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public object DeleteMedicalAdviceCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "CustomerMedicalAdvice"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }
        /// <summary>
        /// 删除用药单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DeletePrescriptionCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "CustomerPrescription"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        /// <summary>
        /// 发药确认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object ConfirmDispensingCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("ConfirmDispensing", "CustomerPrescription"),
                Command = new ConfirmDispensingCommand { CustomerPrescriptionId = id }
            };
        }
        /// <summary>
        /// 摆药确认
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object ConfirmPrescriptionOfferCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("ConfirmPrescriptionOffer", "CustomerPrescription"),
                Command = new ConfirmPrescriptionOfferCommand { CustomerPrescriptionId = id }
            };
        }

        public object DiscardMedicalAdviceCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Discard", "CustomerMedicalAdvice"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }



        public object DeleteCustomerLeaveCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCustomerLeave", "Customer"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public object DeleteCustomerFamilyCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCustomerFamily", "Customer"),
                Command = new DeleteEntityCommand { EntityId = id }
            };
        }

        public object DeletePayment(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DeleteCustomerPayment", "CustomerAccount"),
                Command = new DeleteCustomerPaymentCommand { CustomerPaymentId = id }
            };
        }

        public WebCommand PrintContractCommand(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("PrintContract", "Contract"),
                Command = new PrintContractCommand { ContractId = id }
            };
        }

        public object PrintCostChange(int contractCostChangeId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "ContractCostChange"),
                Command = new PrintContractCostChangeWordToPdfCommand { ContractCostChangeId = contractCostChangeId }
            };
        }

        public object PrintCustomerInfo(int customerAccountId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("PrintCustomerInfo", "Customer"),
                Command = new PrintCustomerInfoWordToPdfCommand { CustomerAccountId = customerAccountId }
            };
        }

        public object PrintRoomChange(int contractRoomChangeId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "ContractRoomChange"),
                Command = new PrintContractRoomChangeWordToPdfCommand { ContractRoomChangeId = contractRoomChangeId }
            };
        }

        public object PrintServicePackChange(int contractServicePackChangeId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Print", "ContractServicePackChange"),
                Command = new PrintContractServicePackChangeWordToPdfCommand { ContractServicePackChangeId = contractServicePackChangeId }
            };
        }

        public bool EnableDeleteCustomerRecord(ServiceRecordDetail serviceRecord)
        {
            if (PensionInsurance.Shared.WebAppContext.Current.User.HasPermission(ModuleType.客户点单, Permission.删除))
            {
                if (serviceRecord.ServiceDate.IsOverFisrtDayOfTwelve())
                {
                    return serviceRecord.ServiceDate.Year == DateTime.Now.Year &&
                           serviceRecord.ServiceDate.Month == DateTime.Now.Month;
                }

                var previousMonth = DateTime.Now.AddMonths(-1);
                return serviceRecord.ServiceDate.Year == previousMonth.Year && serviceRecord.ServiceDate.Month == previousMonth.Month;
            }
            return false;
        }

        public bool EnableEditCustomerRecord(ServiceRecordDetail serviceRecord)
        {
            if (PensionInsurance.Shared.WebAppContext.Current.User.HasPermission(ModuleType.客户点单, Permission.编辑))
            {
                if (serviceRecord.ServiceDate.IsOverFisrtDayOfTwelve())
                {
                    return serviceRecord.ServiceDate.Year == DateTime.Now.Year && serviceRecord.ServiceDate.Month == DateTime.Now.Month;
                }

                var previousMonth = DateTime.Now.AddMonths(-1);
                return serviceRecord.ServiceDate.Year == previousMonth.Year && serviceRecord.ServiceDate.Month == previousMonth.Month;
            }
            return false;
        }

        public object BindBeacon(int customerId)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("BindBeacon", "Customer"),
                Command = new BindBeaconCommand() { CustomerId = customerId }
            };
        }
    }

    public enum ActionType
    {
        Create,
        Back
    }

    public class CreateCustomerLeaveViewModel
    {
        public int CustomerAccountId { get; set; }
        public string Reason { get; set; }
    }

    public class EditCustomerLeaveViewModel
    {
        public int CustomerLeaveId { get; set; }
        public DateTime? ResumptionTime { get; set; }
    }


    public class CustomerPayViewModel
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public int CustomerAccountId { get; set; }
        public string CustomerName { get; set; }
        public decimal Money { get; set; }
        public decimal TotalPayableCost { get; set; }
        public decimal TotalPrePayableCost { get; set; }
        public decimal TotalRemainingCost { get; set; }
        public PaymentType PaymentType { get; set; }
        public IList<Bill> Bills { get; set; }

        public List<ServiceRecordDetail> CustomerOrderFoods { get; set; }

    }
}