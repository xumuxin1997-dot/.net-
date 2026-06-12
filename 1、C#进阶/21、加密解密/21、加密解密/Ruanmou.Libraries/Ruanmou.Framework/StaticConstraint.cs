using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework
{
    /// <summary>
    /// Eleven
    /// 配置文件初始化类 常量类
    /// 集中管理配置文件
    /// </summary>
    public class StaticConstraint
    {
        /// <summary>
        /// 工厂生成DAL的配置文件
        /// </summary>
        public readonly static string IBaseDALConfig = ConfigurationManager.AppSettings["IBaseDALConfig"];

        //数据库连接  也可以放
    }
}
