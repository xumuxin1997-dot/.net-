using Ruanmou.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework
{
    public static class ConsoleExtend
    {
        public static void Show<T>(this T t)
        {
            Type type = t.GetType();
            Console.WriteLine("**************type.Name--Show--Start**************");
            foreach (var prop in type.GetProperties())
            {
                Console.WriteLine($"{type.Name}.{prop.GetDisplayName()}={prop.GetValue(t)}");
            }
            Console.WriteLine("**************type.Name--Show--End**************");
        }
        //public static void ShowList<T>(this IEnumerable<T> t)
        //{
        //    Type type = t.GetType();
        //    Console.WriteLine("**************type.Name--Show--Start**************");
        //    foreach (var prop in type.GetProperties())
        //    {
        //        Console.WriteLine($"{type.Name}.{prop.Name}={prop.GetValue(t)}");
        //    }
        //    Console.WriteLine("**************type.Name--Show--End**************");
        //}
    }
}
