using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.Exceptions;
using PensionInsurance.Query;
using PensionInsurance.Shared;
using PensionInsurance.Web.Common;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    public class AgainEditViewModel
    {
        private readonly UrlHelper _urlHelper;
        private readonly IFetcher _fetcher;
        public AgainEditViewModel(UrlHelper urlHelper, IFetcher fetcher)
        {
            _urlHelper = urlHelper;
            _fetcher = fetcher;
        }

        public PurchaseOrder Order { get; set; }
        public PurchaseShoppingCart Cart { get; set; }
        public TrackingResultViewModel TrackingResult { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int GetStock(int pruductSupplierId, int projectId)
        {
            var productStock =
                _fetcher.Query<Entities.PurchaseProductStock>()
                    .FirstOrDefault(x => x.PurchaseProductSupplier.Id == pruductSupplierId && x.Project.Id == projectId);
            return productStock?.Stock ?? 0;
        }
    }
}