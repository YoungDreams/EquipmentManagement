using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class IndexViewModel
    {
        public PurchaseOrderQuery Query { get; set; }
        public PagedData<Entities.PurchaseOrder> Items { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
    }
}