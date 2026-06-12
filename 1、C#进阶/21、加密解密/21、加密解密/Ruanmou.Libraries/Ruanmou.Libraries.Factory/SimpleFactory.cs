using Ruanmou.Framework;
using Ruanmou.Libraries.IDAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Libraries.Factory
{
    /// <summary>
    /// 动态创建一个对象
    /// Eleven  类库里面的配置文件，都是用的程序的
    /// 代码最终是程序启动运行的，
    /// </summary>
    public class SimpleFactory
    {
        private static string DllName = StaticConstraint.IBaseDALConfig.Split(',')[1];
        private static string TypeName = StaticConstraint.IBaseDALConfig.Split(',')[0];
        //
        public static IBaseDAL CreateInstance()
        {
            Assembly assembly = Assembly.Load(DllName);//加载文件
            Type type = assembly.GetType(TypeName);//获取类型
            return (IBaseDAL)Activator.CreateInstance(type);//返回一个type对象
        }

    }
}
