using System;

namespace Foundation.Data
{
    public abstract class Entity
    {
        /// <summary>
        /// 主键ID(自增列)
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime LastModifiedOn { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreatedOn { get; set; }
    }
}
