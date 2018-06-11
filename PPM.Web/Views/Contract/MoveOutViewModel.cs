using System.Collections.Generic;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Contract
{
    public class MoveOutViewModel
    {
        private readonly UrlHelper _urlHelper;
        public MoveOutViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public int ContractId => MoveOutCommand.ContractId;
        public MoveOutCommand MoveOutCommand { get; set; }
        public IEnumerable<CustomerBill> Bills { get; set; }
        public Entities.Customer Customer { get; set; }
        public Entities.Contract Contract { get; set; }
        public decimal TotalBillCost { get; set; }
        public CustomerAccount CustomerAccount { get; set; }
        public string MoveOutReason { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Contact { get; set; }
        public WebCommand DefrayCustomerMoneyProxy(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DefrayCustomerMoneyProxy", "Contract"),
                Command = new DefrayCustomerMoneyProxyCommand { ContractId = id }
            };
        }
    }
}