using Foundation.Data;
using PensionInsurance.Query;

namespace PensionInsurance.Web.Views.WeChatOfficial
{
    public class IndexViewModel
    {
        public WeChatOfficialVisitQuery Query { get; set; }
        public WeChatOfficialOpinionQuery OpinionQuery { get; set; }
        public PagedData<Entities.WeChatOfficialVisit> WeChatOfficialVisits { get; set; }
        public PagedData<Entities.WeChatOfficialOpinion> WeChatOfficialOpinions { get; set; }
    }
}