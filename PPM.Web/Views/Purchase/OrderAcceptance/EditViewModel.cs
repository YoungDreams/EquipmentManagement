using System;
using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Purchase.OrderAcceptance
{
    public class EditViewModel
    {
        private readonly IFetcher _fetcher;

        public EditViewModel(IFetcher fetcher)
        {
            _fetcher = fetcher;
            PurchaseOrderItems = new List<PurchaseOrderItem>();
            PurchaseOrderOtherItems = new List<PurchaseOrderOtherItem>();
        }
        public PurchaseOrderAcceptance PurchaseOrderAcceptance { get; set; }
        public List<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public List<PurchaseOrderOtherItem> PurchaseOrderOtherItems { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public int TotalProductQuantity { get; set; }
        public decimal TotalProductPrice { get; set; }
        public DateTime CheckTime { get; set; }
        public string CheckRemarks { get; set; }
        /// <summary>
        /// 当前流程第一人
        /// </summary>
        public User CurrentWorkflowStepUser { get; set; }
        public int GetStock(int pruductSupplierId, int projectId)
        {
            var productStock =
                _fetcher.Query<Entities.PurchaseProductStock>()
                    .FirstOrDefault(x => x.PurchaseProductSupplier.Id == pruductSupplierId && x.Project.Id == projectId);
            return productStock?.Stock ?? 0;
        }
    }

    public class ViewAttachmentViewModel
    {
        public int OrderId { get; set; }
        public string ImageBase64String { get; set; }
    }
}