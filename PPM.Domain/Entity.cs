using Foundation.Data;

namespace PPM.Entities
{
    public abstract class MyEntity : Entity
    {
        /// <summary>
        /// 创建人员
        /// </summary>
        public virtual string CreatedBy { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public virtual string LastModifiedBy { get; set; }
    }
}
