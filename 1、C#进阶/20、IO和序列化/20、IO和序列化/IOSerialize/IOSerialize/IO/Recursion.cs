using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOSerialize.IO
{
    /// <summary>
    /// 递归的编程技巧
    /// 表达式目录树---递归--无法预测深度的查找--有重复的动作---就会用上递归---自己调用自己
    /// 
    /// 递归看别人写很简单，自己动手有点不好做，经常想不到
    /// 1 递归一定要有跳出条件
    /// 2 递归对内存有些压力
    /// 3 递归执行速度快，请谨慎使用
    /// </summary>
    public class Recursion
    {
        /// <summary>
        /// 找出全部的子文件夹
        /// </summary>
        /// <param name="rootPath">根目录</param>
        /// <returns></returns>
        public static List<DirectoryInfo> GetAllDirectory(string rootPath)
        {
            if (!Directory.Exists(rootPath))
                return new List<DirectoryInfo>();

            List<DirectoryInfo> directoryList = new List<DirectoryInfo>();//容器
            DirectoryInfo directory = new DirectoryInfo(rootPath);//root文件夹
            directoryList.Add(directory);

            return GetChild(directoryList, directory);

            //var childArray = directory.GetDirectories();
            //if (childArray != null && childArray.Length > 0)
            //{
            //    directoryList.AddRange(childArray);
            //    foreach (var child in childArray)
            //    {
            //        var childArray2 = child.GetDirectories();
            //        if (childArray2 != null && childArray2.Length > 0)
            //        {
            //            directoryList.AddRange(childArray2);
            //            foreach (var item in collection)
            //            {

            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 完成 文件夹--子目录--放入集合
        /// </summary>
        /// <param name="directoryList"></param>
        /// <param name="directoryCurrent"></param>
        /// <returns></returns>
        private static List<DirectoryInfo> GetChild(List<DirectoryInfo> directoryList, DirectoryInfo directoryCurrent)
        {
            var childArray = directoryCurrent.GetDirectories();
            if (childArray != null && childArray.Length > 0)
            {
                directoryList.AddRange(childArray);
                foreach (var child in childArray)
                {
                    GetChild(directoryList, child);
                }
            }
            return directoryList;
        }


        private void Wait()
        {
            if (DateTime.Now.Millisecond < 999)
            {
                //启动个多线程
                Wait();
                Thread.Sleep(5);//最多可能浪费4ms
            }
            else
                return;

        }
    }
}
