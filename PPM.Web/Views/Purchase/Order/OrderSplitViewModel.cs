using System.Collections.Generic;
using System.Linq;
using Foundation.Data;
using PensionInsurance.Entities;
using PensionInsurance.Shared;
using PensionInsurance.Web.Views.TrackingResult;

namespace PensionInsurance.Web.Views.Purchase.Order
{
    //public class OrderSplitViewModel
    //{
    //    private readonly IFetcher _fetcher;
    //    public OrderSplitViewModel(IFetcher fetcher)
    //    {
    //        _fetcher = fetcher;
    //    }
    //    public Entities.PurchaseOrder PurchaseOrder { get; set; }
    //    public Dictionary<int, List<OrderItemDetailView>> GetOrderItemsByCategory()
    //    {
    //        var orderItemsByCategory = new Dictionary<int, List<OrderItemDetailView>>();
    //        var allProductCategories = _fetcher.Query<PurchaseProductCategory>();
    //        var categoryIds = PurchaseOrder.OrderItems.Select(x => x.PurchaseProduct.ProductCategory.Id).Distinct().ToList();
    //        foreach (var categoryId in categoryIds)
    //        {
    //            PurchaseProductCategory getFisrtClassCategory = allProductCategories.First(x => x.Id == categoryId);
    //            if (getFisrtClassCategory.ParentId > 0)
    //            {
    //                getFisrtClassCategory = allProductCategories.First(x => x.Id == getFisrtClassCategory.ParentId.Value);
    //            }

    //            var items = PurchaseOrder.OrderItems.Where(x => x.PurchaseProduct.ProductCategory.Id == categoryId)
    //                .Select(x => new OrderItemDetailView
    //                {
    //                    OrderItemId = x.Id,
    //                    DepartmentId = x.Department.Id,
    //                    DepartmentName = x.Department.Name,
    //                    Specification = x.PurchaseProduct.Specification,
    //                    ProductName = x.PurchaseProduct.Name,
    //                    ProductId = x.PurchaseProduct.Id,
    //                    SinglePrice = x.ActualPrice,
    //                    PriceId = x.PurchaseProductSupplier.Id,
    //                    Quantity = x.CheckQuantity,
    //                    Image = x.PurchaseProduct.ProductPictures.FirstOrDefault()?.ThumbnailPath,
    //                    Creator = x.Createtor.RealName,
    //                    CreatorId = x.Createtor.Id,
    //                    Supplier = x.PurchaseProductSupplier.PurchaseSupplier.Name,
    //                    SupplierId = x.PurchaseProductSupplier.PurchaseSupplier.Id,
    //                    CategoryName = getFisrtClassCategory.Name,
    //                    CategoryId = getFisrtClassCategory.Id,
    //                    Brand = x.PurchaseProduct.Brand,
    //                    IsAsset = x.PurchaseProduct.IsAsset
    //                }).ToList();
    //            orderItemsByCategory.Add(categoryId, items);
    //        }
    //        return orderItemsByCategory;
    //    }
    //    public List<OrderItemDetailView> GetOrderItemsWithFirstClass()
    //    {
    //        var dict = GetOrderItemsByCategory();
    //        var list = new List<OrderItemDetailView>();
    //        foreach (var d in dict)
    //        {
    //            list.AddRange(d.Value);
    //        }
    //        return list;
    //    }
    //    public List<OrderItemDetailView> GetOrderItemsWithFirstClassByCategory()
    //    {
    //        var orderItemsWithFirstClass = GetOrderItemsWithFirstClass();
    //        var result = new List<OrderItemDetailView>();
    //        var categoryIds = orderItemsWithFirstClass.Select(x => x.CategoryId).Distinct();
    //        foreach (var categoryId in categoryIds)
    //        {
    //            var tempItemsByCategoryId = orderItemsWithFirstClass.Where(x => x.CategoryId == categoryId);
    //            foreach (var orderItemByCategoryId in tempItemsByCategoryId)
    //            {
    //                if (result.Count(x => x.CategoryId == categoryId &
    //                                      x.ProductId == orderItemByCategoryId.ProductId &
    //                                      x.PriceId == orderItemByCategoryId.PriceId) > 0)
    //                {
    //                    continue;
    //                }
    //                var sameProducts = orderItemsWithFirstClass.Where(x =>
    //                    x.CategoryId == categoryId &
    //                    x.ProductId == orderItemByCategoryId.ProductId &
    //                    x.PriceId == orderItemByCategoryId.PriceId);
    //                var itemToAdd = sameProducts.First();
    //                itemToAdd.Quantity = sameProducts.Sum(x => x.Quantity);
    //                result.Add(itemToAdd);
    //            }
    //        }
    //        return result;
    //    }
    //    public List<OrderItemDetailView> GetOrderItemsWithFirstClassByCategoryAndDepartment()
    //    {
    //        var orderItemsWithFirstClass = GetOrderItemsWithFirstClass();
    //        var result = new List<OrderItemDetailView>();
    //        var categoryIds = orderItemsWithFirstClass.Select(x => x.CategoryId).Distinct();
    //        foreach (var categoryId in categoryIds)
    //        {
    //            var tempItemsByCategoryId = orderItemsWithFirstClass.Where(x => x.CategoryId == categoryId);
    //            foreach (var orderItemByCategoryId in tempItemsByCategoryId)
    //            {
    //                if (result.Count(x => x.CategoryId == categoryId &
    //                                      x.ProductId == orderItemByCategoryId.ProductId &
    //                                      x.PriceId == orderItemByCategoryId.PriceId &
    //                                      x.DepartmentId == orderItemByCategoryId.DepartmentId) > 0)
    //                {
    //                    continue;
    //                }
    //                var sameProducts = orderItemsWithFirstClass.Where(x =>
    //                    x.CategoryId == categoryId &
    //                    x.ProductId == orderItemByCategoryId.ProductId &
    //                    x.PriceId == orderItemByCategoryId.PriceId &
    //                    x.DepartmentId == orderItemByCategoryId.DepartmentId);
    //                var itemToAdd = sameProducts.First();
    //                itemToAdd.Quantity = sameProducts.Sum(x => x.Quantity);
    //                result.Add(itemToAdd);
    //            }
    //        }
    //        return result;
    //    }
    //}
}