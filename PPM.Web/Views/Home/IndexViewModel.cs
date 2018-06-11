using System;
using System.Collections.Generic;
using PensionInsurance.Entities;
using PensionInsurance.Entities.DetailViews;

namespace PensionInsurance.Web.Views.Home
{
    public class IndexViewModel
    {
        public IEnumerable<DailyReport> DailyReport { get; set; }
        public IEnumerable<Entities.Contract> IneffectiveContracts { get; set; }
        public IEnumerable<Entities.Contract> IneffectiveContractsByToday { get; set; }
        public IEnumerable<Entities.Contract> InvalidContracts { get; set; }
        public IEnumerable<Entities.Contract> InvalidContractsByToday { get; set; }
        public IEnumerable<Entities.Contract> InvalidContractsBydays { get; set; }
        public IEnumerable<string> CustomerChangeDesc { get; set; }
        public IEnumerable<CustomerAccountDetail> CustomerAccountArrearages { get; set; }
        public IEnumerable<Addtional> IneffectiveAddtionals { get; set; }
        public DateTime FirstDayOfNowMonth { get; set; }
        public IEnumerable<ConsultingDetail> ConsultingDetails { get; set; }
    }
}