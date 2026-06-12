using Ruanmou.Framework.Data;
using Ruanmou.Framework.Data.ValidateExtend;
using System;

namespace Ruanmou.Libraries.Model
{
    /// <summary>
    /// 使用者类
    /// 保存名字、账号、密码、邮箱电话、公司信息、状态、用户类型、最后登录时间、创建时间、创建人、最后修改人等信息
    /// UserModel
    /// </summary>

    public class User : BaseModel
    {
        public string Name { get; set; }
        [Required]
        [StringLength(10, 20)]
        public string Account { get; set; }
        [Required]
        [StringLength(10, 30)]
        //数字+字母 2种以上
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }

        /// <summary>
        /// 数据库是State 但是程序有规范Status
        /// </summary>
        [ColumnEleven("State")]
        public int Status { get; set; }
        public int UserType { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreatorId { get; set; }
        public int? LastModifierId { get; set; }

    }
}
