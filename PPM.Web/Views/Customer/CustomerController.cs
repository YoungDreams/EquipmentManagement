using System.Linq;
using System.Web.Mvc;
using PensionInsurance.Query;
using PensionInsurance.Web.Views.Account;
using PensionInsurance.Commands;
using Foundation.Messaging;
using System;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Workflows;
using System.IO;
using CreateViewModel = PensionInsurance.Web.Views.CustomerFamily.CreateViewModel;
using System.Text;

namespace PensionInsurance.Web.Views.Customer
{
    public class CustomerController : AuthorizedController
    {
        private readonly ICommandService _commandService;
        private readonly IFetcher _fetcher;
        private readonly ICustomerLeaveQueryService _leaveQueryService;
        private readonly IServiceRecordQueryService _serviceRecordQueryService;
        private readonly ICustomerQueryService _customerQueryService;
        private readonly ICustomerFamilyQueryService _customerFamilyQueryService;
        private readonly ICustomerLeaveQueryService _customerLeaveQueryService;
        private readonly IContractQueryService _contractQueryService;
        private readonly ICustomerBillQueryService _customerBillQueryService;
        private readonly ICustomerPaymentQueryService _customerPaymentQueryService;
        private readonly ICustomerPointQueryService _customerPointQueryService;
        private readonly IContractCostChangeQueryService _contractCostChangeQueryService;
        private readonly IContractRoomChangeQueryService _contractRoomChangeQueryService;
        private readonly IContractServicePackChangeQueryService _contractServicePackChangeQueryService;
        private readonly IProjectQueryService _projectQueryService;
        private readonly CustomerAccountCheckOutRefundWorkflow _contractRefundWorkflow;
        private readonly ICustomerLivingHistoryQueryService _customerLivingHistoryQueryService;
        private readonly ICustomerExpenseHistoryQueryService _customerExpenseHistoryQueryService;
        private readonly IConsultingOrderFoodService _cashPaidOrderFoodService;
        private readonly IAreaQueryService _areaQuerySerivce;
        public CustomerController(ICustomerLeaveQueryService leaveQueryService,
            ICommandService commandService, IServiceRecordQueryService serviceRecordQueryService,
            ICustomerQueryService customerQueryService, ICustomerFamilyQueryService customerFamilyQueryService,
            IFetcher fetcher,
            ICustomerLeaveQueryService customerLeaveQueryService, IContractQueryService contractQueryService1,
            ICustomerPaymentQueryService customerPaymentQueryService,
            ICustomerPointQueryService customerPointQueryService, ICustomerBillQueryService customerBillQueryService,
            IContractCostChangeQueryService contractCostChangeQueryService,
            IContractRoomChangeQueryService contractRoomChangeQueryService,
            IContractServicePackChangeQueryService contractServicePackChangeQueryService, 
            IProjectQueryService projectQueryService, 
            CustomerAccountCheckOutRefundWorkflow contractRefundWorkflow, 
            ICustomerLivingHistoryQueryService customerLivingHistoryQueryService,
            ICustomerExpenseHistoryQueryService customerExpenseHistoryQueryService, IConsultingOrderFoodService cashPaidOrderFoodService,
            IAreaQueryService areaQuerySerivce)
        {
            _commandService = commandService;
            _serviceRecordQueryService = serviceRecordQueryService;
            _leaveQueryService = leaveQueryService;
            _customerQueryService = customerQueryService;
            _customerFamilyQueryService = customerFamilyQueryService;
            _fetcher = fetcher;
            _customerLeaveQueryService = customerLeaveQueryService;
            _contractQueryService = contractQueryService1;
            _customerPaymentQueryService = customerPaymentQueryService;
            _customerPointQueryService = customerPointQueryService;
            _customerBillQueryService = customerBillQueryService;
            _contractCostChangeQueryService = contractCostChangeQueryService;
            _contractRoomChangeQueryService = contractRoomChangeQueryService;
            _contractServicePackChangeQueryService = contractServicePackChangeQueryService;
            _projectQueryService = projectQueryService;
            _contractRefundWorkflow = contractRefundWorkflow;
            _customerLivingHistoryQueryService = customerLivingHistoryQueryService;
            _customerExpenseHistoryQueryService = customerExpenseHistoryQueryService;
            _cashPaidOrderFoodService = cashPaidOrderFoodService;
            _areaQuerySerivce = areaQuerySerivce;
        }

        public ActionResult IndexAll(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.客户信息查看全部客户))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                Query = query
            };

            viewModel.Customers = _customerQueryService.GetCustomers(page, pageSize, query);
            return View("~/Views/Customer/Index.All.cshtml", viewModel);
        }

        public ActionResult IndexCheckIn(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.客户信息查看入住客户))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                Query = query
            };
            viewModel.CustomerCheckInDetails = _customerQueryService.GetCheckInCustomers(page, pageSize, query);

            return View("~/Views/Customer/Index.CheckIn.cshtml", viewModel);
        }

        public ActionResult IndexCheckout(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.客户信息查看退住客户))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new IndexViewModel
            {
                Query = query
            };

            viewModel.CustomerCheckOutDetails = _customerQueryService.GetCheckOutCustomers(page, pageSize, query);

            return View("~/Views/Customer/Index.CheckOut.cshtml", viewModel);
        }

        [HttpGet]
        public PartialViewResult EditCustomer(int customerId)
        {
            var customer = _fetcher.Get<Entities.Customer>(customerId);
            if (customer == null)
            {
                throw new ApplicationException("客户不存在！");
            }
            var viewModel = new EditViewModel()
            {
                Id = customer.Id,
                Name = customer.Name,
                IDCard = customer.IDCard,
                Sex = customer.Sex,
                Address = customer.Address,
                CareDemand = customer.CareDemand,
                DietaryRestrictions = customer.DietaryRestrictions,
                Birthday = customer.Birthday,
                Bath = customer.Bath,
                Character = customer.Character,
                Decision = customer.Decision,
                DesignatedHospital = customer.DesignatedHospital,
                Diagnosis=customer.Diagnosis,
                Direction=customer.Direction,
                Dress = customer.Dress,
                DrinkAlcohol = customer.DrinkAlcohol,
                Eating = customer.Eating,
                Education = customer.Education,
                Faith = customer.Faith,
                FavouriteFood = customer.FavouriteFood,
                FavouritePerson = customer.FavouritePerson,
                FavouritePlace = customer.FavouritePlace,
                FavouriteThing = customer.FavouriteThing,
                Hearing = customer.Hearing,
                Hobby = customer.Hobby,
                Hygiene = customer.Hygiene,
                Language = customer.Language,
                Laundry = customer.Laundry,
                MainlyPharmacy = customer.MainlyPharmacy,
                MedicineControl = customer.MedicineControl,
                Memory = customer.Memory,
                Movement = customer.Movement,
                Nationality = customer.Nationality,
                PotentialRisk = customer.PotentialRisk,
                SiestaTime = customer.SiestaTime,
                SleepTime = customer.SleepTime,
                Smoke = customer.Smoke,
                TabooSubject = customer.TabooSubject,
                Taste = customer.Taste,
                Transfermation = customer.Taste,
                UseToilet = customer.UseToilet,
                Vision = customer.Vision,
                WakeUpTime = customer.WakeUpTime,
                Work = customer.Work,
                AllergyHistory = customer.AllergyHistory
            };
            return PartialView("_Customer.Edit", viewModel);
        }

        [HttpPost]
        public void EditCustomer(EditCustomerCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public PartialViewResult EditAvatar(int customerId)
        {
            var viewModel = new EditAvatarViewModel()
            {
                CustomerId = customerId
            };
            return PartialView("_Customer.Avatar", viewModel);
        }

        [HttpPost]
        public void EditAvatar(EditCustomerAvatarCommand command)
        {
            if (Request.Files.Count > 0)
            {
               command.FilePath = Request.Files[0].ReadBytes();
               command.FileName = Request.Files[0].FileName;
            }
            _commandService.Execute(command);
        }

        [HttpGet]
        public ActionResult CustomerDetail(int customerId)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理,Permission.客户信息查看全部客户))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CustomerDetailViewModel(Url);
            viewModel.CustomerInfo = _customerQueryService.Get(customerId);
            viewModel.CustomerContracts = _contractQueryService.GetContractByCustomerId(customerId).ToList();

            var contractIds = viewModel.CustomerContracts.Select(x => x.ContractId).ToList();
            viewModel.ContractCostChanges =
                _contractCostChangeQueryService.QueryAllByContractIds(contractIds);
            viewModel.ContractServicePackChanges = _contractServicePackChangeQueryService.QueryAllByContractIds(contractIds);
            viewModel.ContractRoomChanges = _contractRoomChangeQueryService.QueryAllByContractIds(contractIds);
            viewModel.CustomerLivingHistories = _customerLivingHistoryQueryService.Query(customerId);
            viewModel.CustomerExpenseHistories = _customerExpenseHistoryQueryService.Query(customerId);
            return View("CustomerDetail", viewModel);
        }

        [HttpGet]
        public ActionResult Detail(int customerAccountId)
        {
            var customerAccount = _fetcher.Get<CustomerAccount>(customerAccountId);
            if (customerAccount == null)
            {
                throw new ApplicationException("客户账户不存在！");
            }
            var viewModel = new DetailViewModel(Url);
            viewModel.CustomerId = customerAccount.Customer.Id;
            viewModel.CustomerAccountId = customerAccountId;
            
            viewModel.CustomerFamilyList = customerAccount.Customer.CustomerFamilies;
            viewModel.CustomerLeaveList = _customerLeaveQueryService.Query(customerAccountId);
            viewModel.CustomerServiceRecords = _serviceRecordQueryService.GetCustomerServiceRecordByCustomerAccountId(customerAccount.Id).Where(m=>m.ServiceRecordType == ServiceRecordType.记账);
            viewModel.CustomerContracts = _contractQueryService.GetContracts(customerAccountId).ToList();
            viewModel.CustomerPaymentList = _customerPaymentQueryService.GetCustomerPayments(customerAccountId);
            viewModel.CustomerPointList = _customerPointQueryService.GetCustomerPoints(customerAccountId);
            viewModel.CustomerRefundList = _fetcher.Query<Entities.CustomerAccountCheckOutRefund>().Where(x => x.CustomerAccount.Id == customerAccount.Id).ToList();
            viewModel.CustomerAccountTransferList = _fetcher.Query<Entities.CustomerAccountTransfer>().Where(x => x.TransferFrom.Id == customerAccount.Id).ToList();
            viewModel.CustomerType = _customerLeaveQueryService.GetCustomerStatus(customerAccountId);
            viewModel.CustomerBillDetails = _customerBillQueryService.GetCustomerBills(customerAccountId);
            viewModel.CashPaidOrderFoods = _cashPaidOrderFoodService.Query(customerAccountId);
            viewModel.CustomerFamilyCreateViewModel = new CreateViewModel();
            viewModel.ActivatedContract = viewModel.CustomerContracts.FirstOrDefault(x => x.ContractStatus == ContractStatus.生效);
            viewModel.CustomerLivingHistories = _customerLivingHistoryQueryService.Query(customerAccount.Customer.Id);
            viewModel.CustomerExpenseHistories = _customerExpenseHistoryQueryService.Query(customerAccount.Customer.Id);
            viewModel.CustomerMedicalAdvices =
                _fetcher.Query<Entities.CustomerMedicalAdvice>()
                    .Where(x => x.CustomerAccount.Id == customerAccount.Id && x.Status !=MedicalAdviceStatus.作废)
                    .ToList();
            viewModel.CustomerPrescriptions =
                _fetcher.Query<Entities.CustomerPrescription>()
                    .Where(x => x.CustomerMedicalAdvice.CustomerAccount.Id == customerAccount.Id)
                    .ToList();
            var contractIds = viewModel.CustomerContracts.Select(x => x.ContractId).ToList();
            viewModel.ContractCostChanges =
                _contractCostChangeQueryService.QueryAllByContractIds(contractIds);
            viewModel.ContractServicePackChanges =_contractServicePackChangeQueryService.QueryAllByContractIds(contractIds);
            viewModel.ContractRoomChanges = _contractRoomChangeQueryService.QueryAllByContractIds(contractIds);

            if (viewModel.CustomerType == CustomerType.入住 || viewModel.CustomerType == CustomerType.请假)
            {
                if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.客户信息查看入住客户))
                {
                    return RedirectToAction("NoPermission", "Home");
                }

                if (viewModel.CustomerType == CustomerType.请假)
                {
                    viewModel.ActivatedCustomerLeave =
                        viewModel.CustomerLeaveList.FirstOrDefault(x => x.ResumptionTime == null);
                }
                viewModel.CustomerInfo = _customerQueryService.GetCheckInCustomer(customerAccountId);
                var contractServicePackChange =
                    _fetcher.Query<Entities.ContractServicePackChange>()
                        .FirstOrDefault(
                            x =>
                                x.Status == ContractAddtionalStatus.生效 && x.ChangeDate <= DateTime.Now &&
                                x.ChangeEndDate >= DateTime.Now && x.Contract.Id == viewModel.ActivatedContract.ContractId);
                if (contractServicePackChange != null && viewModel.CustomerInfo != null)
                {
                    viewModel.CustomerInfo.ConcernType = contractServicePackChange.ConcernType;
                }
                viewModel.CustomerInfo.CheckedInDate =
                    _contractQueryService.QueryReNewContract(_contractQueryService.Get(viewModel.ActivatedContract.ContractId)).StartTime;
            }
            if (viewModel.CustomerType == CustomerType.退住)
            {
                if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.客户信息查看退住客户))
                {
                    return RedirectToAction("NoPermission", "Home");
                }
                viewModel.CustomerInfo = _customerQueryService.GetCheckOutCustomer(customerAccount.Customer.Id, customerAccountId);
            }
            
            if (viewModel.CustomerType == CustomerType.未知)
            {
               return RedirectToAction("CustomerDetail", new {CustomerId = viewModel.CustomerId});
            }

            return View("~/Views/Customer/Detail.cshtml", viewModel);
        }
        
        /// <summary>
        /// 删除亲属信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public void DeleteCustomerFamily(DeleteEntityCommand command)
        {
            Entities.CustomerFamily customerFamily = _customerFamilyQueryService.Get(command.EntityId);
            _commandService.Execute(DeleteEntityCommand.Of(customerFamily));
        }
        
        [HttpPost]
        public void CreateCustomerLeave(CreateCustomerLeaveCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public PartialViewResult CreateCustomerLeave(int customerAccountId)
        {
            var createCustomerLeaveViewModel = new CreateCustomerLeaveViewModel
            {
                CustomerAccountId = customerAccountId
            };
            return PartialView("_CustomerLeave.CreateForm", createCustomerLeaveViewModel);
        }
        
        [HttpGet]
        public PartialViewResult BackCustomerLeave(int customerLeaveId)
        {
            var viewModel = new EditCustomerLeaveViewModel
            {
               CustomerLeaveId = customerLeaveId,
               ResumptionTime = _customerLeaveQueryService.Get(customerLeaveId).ResumptionTime
            };
            return PartialView("_CustomerLeave.EditForm", viewModel);
        }
        
        [HttpPost]
        public void BackCustomerLeave(CreateCustomerSickLeaveCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpPost]
        public void DeleteCustomerLeave(DeleteEntityCommand command)
        {
            Entities.CustomerLeave leave = _leaveQueryService.Get(command.EntityId);
            _commandService.Execute(DeleteEntityCommand.Of(leave));
        }

        [HttpGet]
        public ActionResult CustomerLeaves(CustomerLeaveQuery query, int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize)
        {
            var viewModel = new CustomerLeavesViewModel(Url)
            {
                Query = query,
                CustomerWithoutList = _customerLeaveQueryService.Query(page, pageSize, query),
            };
            return View("~/Views/Customer/CustomerLeaves.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult ValidateCheckOutRefund(int customerAccountId)
        {
            var customerAccount = _fetcher.Get<CustomerAccount>(customerAccountId);

            var hasCustomerBills =
                _fetcher.Query<CustomerBill>()
                    .Any(x => x.Contract.CustomerAccount.Id == customerAccount.Id && x.Status != BillStatus.已结清);

            var hasPaddingPayment =
                _fetcher.Query<CustomerPayment>()
                    .Any(x => x.CustomerAccount.Id == customerAccountId && x.Status == CustomerPaymentStatus.待确认);

            if (hasCustomerBills || hasPaddingPayment)
            {
                return Json(new { success = false, message = "客户有未结清的账单或有未确认的缴费记录" }, JsonRequestBehavior.AllowGet);
            }

            if (customerAccount.Balance == 0)
            {
                return Json(new { success = false, message = "客户现金余额为0，不能发起退款" }, JsonRequestBehavior.AllowGet);
            }

            var checkOutRefund = _fetcher.Query<Entities.CustomerAccountCheckOutRefund>()
                .FirstOrDefault(
                    x =>
                        x.CustomerAccount.Id == customerAccountId && x.RefundStatus != RefundStatus.已完成);

            if (checkOutRefund != null && checkOutRefund.RefundStatus != RefundStatus.新建)
            {
                return Json(new
                {
                    success = false,
                    message = "退款流程已经发起，请勿重复操作",
                    redirect = Url.Action("Detail", "CustomerAccountCheckOutRefund", new { id = checkOutRefund.Id })
                },
                        JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, redirect = Url.Action("CreateAndSubmit", "CustomerAccountCheckOutRefund", new { id = customerAccount.Id }) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerLivingHistory(int page = 1, int pageSize = Web.Common.PaginationSetttings.PageSize, CustomerLivingHistoryQuery query = null)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.查看))
            {
                return RedirectToAction("NoPermission", "Home");
            }

            var viewModel = new CustomerLivingHistoryViewModel(Url)
            {
                Query = query,
                CustomerLivingHistories = _customerLivingHistoryQueryService.Query(page, pageSize, query)
            };

            return View("~/Views/Customer/CustomerLivingHistory.cshtml", viewModel);
        }
        [HttpPost]
        public void CreateCustomerLivingHistory(CreateCustomerLivingHistoryCommand command)
        {
            if (!WebAppContext.Current.User.HasPermission(ModuleType.客户信息管理, Permission.查看))
            {
                RedirectToAction("NoPermission", "Home");
            }
            _commandService.Execute(command);
        }

        /// <summary>
        /// 打印客户信息
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PrintCustomerInfo(PrintCustomerInfoWordToPdfCommand command)
        {
            var result = _commandService.ExecuteFoResult(command);

            return Json(new
            {
                success = true,
                redirect = $"{Url.Content("~/Attachments/Print/")}{result.FileName}"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void BindBeacon(BindBeaconCommand command)
        {
            _commandService.Execute(command);
        }

        [HttpGet]
        public void GetAreas()
        {
            var result = _areaQuerySerivce.GetAreaJson();

            var json = result.ToJavascript();
        }
    }

    public class FileManager
    {
        /// 写字节数组到文件  
        /// </summary>  
        /// <param name="buff"></param>  
        /// <param name="filePath"></param>  
        public static void WriteBuffToFile(byte[] buff, string filePath)
        {
            WriteBuffToFile(buff, 0, buff.Length, filePath);
        }
        /// <summary>  
        /// 写字节数组到文件  
        /// </summary>  
        /// <param name="buff"></param>  
        /// <param name="offset">开始位置</param>  
        /// <param name="len"></param>  
        /// <param name="filePath"></param>  
        public static void WriteBuffToFile(byte[] buff, int offset, int len, string filePath)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream output = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(output);
            writer.Write(buff, offset, len);
            writer.Flush();
            writer.Close();
            output.Close();
        }
    }
}