using System;

namespace PensionInsurance.Web.Views.CustomerFamily
{
    public class CreateViewModel
    {
        /// <summary>
        /// 客户ID
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 与被养护人关系
        /// </summary>
        public string Ship { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCardNo { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        public string WorkUnit { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Mailbox { get; set; }
        /// <summary>
        /// 通讯地址
        /// </summary>
        public string Address { get; set; }
    }

    public class EditCustomerFamilyAvatarViewModel
    {
        
        public int CustomerFamilyId { get; set; }
        public string AvatarFilePath { get; set; }
    }
}