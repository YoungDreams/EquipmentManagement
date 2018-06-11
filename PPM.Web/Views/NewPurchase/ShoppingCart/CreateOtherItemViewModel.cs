using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PensionInsurance.Web.Views.NewPurchase.ShoppingCart
{
    public class CreateOtherItemViewModel
    {
        public int CartId { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public string ProductBrand { get; set; }
        public string ProductSpecification { get; set; }
        public string ProductUnit { get; set; }
        public int PurchaseQuantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public string ProductAssetClass { get; set; }
        public string Remarks { get; set; }
        public string ProductUrl { get; set; }
    }
}