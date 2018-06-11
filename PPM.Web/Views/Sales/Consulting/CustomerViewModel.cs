using System.Collections.Generic;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.Sales.Consulting
{
    public class ConsultationViewModel
    {
        public IEnumerable<Setting> ConsultingLevelList { get; set; }
        public IEnumerable<Setting> ConsultingTypeList { get; set; }
        public IEnumerable<Setting> HouseTypeList { get; set; }
        public IEnumerable<Setting> ConsultingSourceList { get; set; }
        public IEnumerable<Setting> InformedChannelList { get; set; }
        public IEnumerable<Setting> ProjectList { get; set; }
        public IEnumerable<Setting> NursingTypeList { get; set; }
        public IEnumerable<Setting> HealthStatusList { get; set; }
        public IEnumerable<Setting> EducationalBackgroundList { get; set; }
        public IEnumerable<Setting> WorkIndustryList { get; set; }
        public IEnumerable<Setting> PayerList { get; set; }
        public IEnumerable<Setting> SpouseInformationList { get; set; }
        public IEnumerable<Setting> ChildrenInformationList { get; set; }
        public IEnumerable<Setting> WorkIndustryOfChildrenList { get; set; }
        public IEnumerable<Setting> RecommenderTypeList { get; set; }
        public IEnumerable<Setting> RecommenderList { get; set; }
        public IEnumerable<Setting> RelationshipWithClientList { get; set; }
    }
}