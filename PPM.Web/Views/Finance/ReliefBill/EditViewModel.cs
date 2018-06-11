using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Web.Common;

namespace PensionInsurance.Web.Views.Finance.ReliefBill
{
    public class EditViewModel
    {
        private readonly UrlHelper _urlHelper;
        public EditViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public int Id { get; set; }
        public int CustomerAccountId { get; set; }
        public Entities.CustomerAccount CustomerAccount { get; set; }
        public IList<CustomerBill> CustomerBills { get; set; }
        public bool IsIncluded { get; set; }
        public int BillId { get; set; }
        public decimal RoomCost { get; set; }
        public decimal MealsCost { get; set; }
        public decimal PackageServiceCost { get; set; }
        public decimal IncrementCost { get; set; }
        public decimal ActualPaymentCost { get; set; }
        public decimal CustomerCurrentYearDiscount { get; set; }
        public decimal ProjectYearDiscount { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public decimal MaxReliefCost { get; set; }

        public WebCommand DraftAndDelete(int id)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("DraftAndDelete", "ReliefBill"),
                Command = new DraftAndDeleteReliefBillCommand { ReliefBillId = id}
            };
        }
    }
    
}