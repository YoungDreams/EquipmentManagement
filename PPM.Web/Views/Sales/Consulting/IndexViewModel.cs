using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Foundation.Data;
using PensionInsurance.Commands;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.Sales.Consulting
{
    public class IndexViewModel
    {
        private readonly UrlHelper _urlHelper;
        public IndexViewModel(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public ConsultingQuery Query { get; set; }
        public PagedData<ConsultingDetail> Items { get; set; }
        public List<User> Sales { get; set; }
        public List<SelectListItem> FavoriteFolderSelectList { get; set; }
        public IEnumerable<SelectListItem> ProjectList { get; set; }

        public string PopulateDetailUrl(int consultingId,int page,ConsultingQuery query)
        {
            RouteValueDictionary routeValue = PopulateUrl(consultingId, page, query);

            return _urlHelper.Action("Detail", "Consulting", routeValue);
        }

        public string PopulateEditUrl(int consultingId, int page, ConsultingQuery query)
        {
            RouteValueDictionary routeValue = PopulateUrl(consultingId, page, query);
            routeValue.Add("Query.IsEditNext", true);

            return _urlHelper.Action("Edit", "Consulting", routeValue);
        }

        private static RouteValueDictionary PopulateUrl(int consultingId, int page, ConsultingQuery query)
        {
            RouteValueDictionary routeValue = new RouteValueDictionary();
            routeValue.Add("page", page);
            routeValue.Add("pageSize", 1);
            routeValue.Add("id", consultingId);
            routeValue.Add("Query.Keywords", query.Keywords);
            routeValue.Add("Query.StartLastModifiedOn", query.StartLastModifiedOn);
            routeValue.Add("Query.EndLastModifiedOn", query.EndLastModifiedOn);
            routeValue.Add("Query.ProjectId", query.ProjectId);
            routeValue.Add("Query.StartTrackingTime", query.StartTrackingTime);
            routeValue.Add("Query.EndTrackingTime", query.EndTrackingTime);
            routeValue.Add("Query.ConsultingName", query.ConsultingName);
            routeValue.Add("Query.VisitorName", query.VisitorName);
            routeValue.Add("Query.SalesUserName", query.SalesUserName);
            
            if (query.ConsultingLevels != null && query.ConsultingLevels.Any())
            {
                for (int i = 0; i < query.ConsultingLevels.Count; i++)
                {
                    routeValue.Add($"Query.ConsultingLevels[{i}]", query.ConsultingLevels[i]);
                }
            }

            if (query.ConsultingTrackingTypes != null && query.ConsultingTrackingTypes.Any())
            {
                for (int i = 0; i < query.ConsultingTrackingTypes.Count; i++)
                {
                    routeValue.Add($"Query.ConsultingTrackingTypes[{i}]", query.ConsultingTrackingTypes[i]);
                }
            }

            return routeValue;
        }
    }
}   