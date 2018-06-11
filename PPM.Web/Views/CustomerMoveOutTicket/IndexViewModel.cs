using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Foundation.Data;
using PensionInsurance.Query;
using PensionInsurance.Web.Common;
using PensionInsurance.Commands;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.CustomerMoveOutTicket
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }
        public CustomerMoveOutTicketQuery Query { get; set; }
        public PagedData<CustomerMoveOutTicketDetail> Items { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }
        public string PopulateDetailUrl(int id, int page, CustomerMoveOutTicketQuery query)
        {
            RouteValueDictionary routeValue = PopulateUrl(id, page, query);

            return _urlHelper.Action("Detail", "CustomerMoveOutTicket", routeValue);
        }
        private static RouteValueDictionary PopulateUrl(int id, int page, CustomerMoveOutTicketQuery query)
        {
            RouteValueDictionary routeValue = new RouteValueDictionary();
            routeValue.Add("page", page);
            routeValue.Add("pageSize", 1);
            routeValue.Add("id", id);
            routeValue.Add("Query.CustomerName", query.CustomerName);
            routeValue.Add("Query.MoveOutStartTime", query.MoveOutStartTime);
            routeValue.Add("Query.MoveOutEndTime", query.MoveOutEndTime);
            routeValue.Add("Query.ContractNo", query.ContractNo);

            if (query.ProjectIds != null && query.ProjectIds.Any())
            {
                for (int i = 0; i < query.ProjectIds.Count; i++)
                {
                    routeValue.Add($"Query.ProjectIds[{i}]", query.ProjectIds[i]);
                }
            }

            return routeValue;
        }
    }
}