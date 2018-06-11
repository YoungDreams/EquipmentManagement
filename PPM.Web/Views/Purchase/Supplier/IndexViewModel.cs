using System.Collections.Generic;
using System.Web.Mvc;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;
using System.Text;
using System.Linq;
using Foundation.Core;

namespace PensionInsurance.Web.Views.Purchase.Supplier
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public PurchaseSupplierQuery Query { get; set; }
        public PagedData<PurchaseSupplierViewModel> Items { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
        public object DeleteCommand(int id, string strUrl)
        {
            return new WebCommand
            {
                Url = _urlHelper.Action("Delete", "Supplier"),
                Command = new DeletePurchaseSupplierCommand { Id = id, ReturnUrl = strUrl }
            };
        }
    }

    public class PurchaseSupplierViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AreaIds { get; set; }
        public string AreaNames { get; set; }
        public string ProjectIds { get; set; }
        public string ProjectNames { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string Note { get; set; }
        public bool Status { get; set; }
    }

    public static class PurchaseSupplierConverter
    {
        public static List<PurchaseSupplierViewModel> Convert(this List<Entities.PurchaseSupplier> source)
        {
            var result = new List<PurchaseSupplierViewModel>();
            foreach (var item in source)
            {
                result.Add(new PurchaseSupplierViewModel {
                    Id = item.Id,
                    AreaIds = item.AreaIds,
                    BankAccount = item.BankAccount,
                    ContactPerson = item.ContactPerson,
                    BankName = item.BankName,
                    ContactPhone = item.ContactPhone,
                    Email = item.Email,
                    Name = item.Name,
                    Note = item.Note,
                    ProjectIds = item.ProjectIds,
                    Status = item.Status
                });
            }
            return result;
        }

        public static void SetAreaNamesProjectNames(this List<PurchaseSupplierViewModel> source, List<Entities.Area> cities, List<Entities.Project> projects)
        {
            foreach (var item in source)
            {
                if (!string.IsNullOrEmpty(item.AreaIds)&&!string.IsNullOrEmpty(item.ProjectIds))
                {
                    var aredIds = item.AreaIds.SplitToList<int>(',');
                    var projectIds = item.ProjectIds.SplitToList<int>(',');
                    var areanames = new List<string>();
                    var projectnames = new List<string>();
                    foreach (var areaId in aredIds)
                    {
                        var cityname = cities.Single(x => x.Id == areaId).Name;
                        areanames.Add(cityname);
                    }
                    item.AreaNames = string.Join(",", areanames);
                    foreach (var projectId in projectIds)
                    {
                        var projectname = projects.Single(x => x.Id == projectId).Name;
                        projectnames.Add(projectname);
                    }
                    item.ProjectNames = string.Join(",", projectnames);
                }
            }
        }
    }
}