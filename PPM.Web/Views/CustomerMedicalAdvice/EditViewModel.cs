using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PensionInsurance.Entities;

namespace PensionInsurance.Web.Views.CustomerMedicalAdvice
{
    public class EditViewModel
    {
        public int CustomerMedicalAdviceId { get; set; }
        public int CustomerAccountId { get; set; }
        public MedicalAdviceType MedicalAdviceType { get; set; }
        public DateTime? StartTime { get; set; }
        public User StartDoctor { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 医嘱内容
        /// </summary>
        public string MedicalAdviceContent { get; set; }
        /// <summary>
        /// 单次剂量
        /// </summary>
        public string Dose { get; set; }
        /// <summary>
        /// 频次
        /// </summary>
        public string Frequency { get; set; }
        /// <summary>
        /// 给药途径
        /// </summary>
        public RouteOfAdministration? RouteOfAdministration { get; set; }
        /// <summary>
        /// 是否药品
        /// </summary>
        public bool? IsDrugs { get; set; }
        /// <summary>
        /// 用药时间
        /// </summary>
        public string MedicationTime { get; set; }
    }
}