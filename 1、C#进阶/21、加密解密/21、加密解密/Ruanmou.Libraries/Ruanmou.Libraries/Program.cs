using Ruanmou.Libraries.Factory;
using Ruanmou.Libraries.IDAL;
using Ruanmou.Libraries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Libraries
{
    /// <summary>
    /// 解决方案搜索 Eleven，查看项目注意点
    /// 
    /// 高级班的传统，准备好学习的小伙伴儿，给Eleven老师刷个字母E，然后课程就要正式开始啦。。。不等人啊不等人
    /// 
    /// Eleven  
    /// 项目分层：UI控制台--数据库访问层---数据库访问层抽象---Model---Framework
    /// 
    /// 把更新&删除实现一下，
    /// 把生成器做个单独项目，winform，配置下模板就更好
    /// 
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Eleven直播写作业了。。。。");

                {
                    //【1】通过工厂模式创建数据访问层实例
                    IBaseDAL baseDAL = SimpleFactory.CreateInstance();
                    //【2】通过数据访问层实例，调用方法，获取数据
                    User user = baseDAL.FindT<User>(1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
