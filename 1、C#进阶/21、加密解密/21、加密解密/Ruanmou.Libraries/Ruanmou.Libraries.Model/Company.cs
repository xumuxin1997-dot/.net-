using Ruanmou.Framework.Data;
using System;

namespace Ruanmou.Libraries.Model
{
    /// <summary>
    /// 公司表
    /// 需要存储名称、创建时间、创建人、最后修改人、最后修改时间
    /// </summary>
    public class Company : BaseModel
    {
        [DisplayEleven("名称")]//自定义特性，作用：在显示的时候显示中文名称
        public string Name { get; set; }
        [DisplayEleven("创建时间")]
        public System.DateTime CreateTime { get; set; }
        public int CreatorId { get; set; }
        /// <summary>
        /// Eleven 
        /// int?可空字段 既可以是int  也可以是null
        /// 数据库设计的时候  字段是可空null
        /// </summary>
        public int? LastModifierId { get; set; }//Nullable<int>
        public DateTime? LastModifyTime { get; set; }
    }
}