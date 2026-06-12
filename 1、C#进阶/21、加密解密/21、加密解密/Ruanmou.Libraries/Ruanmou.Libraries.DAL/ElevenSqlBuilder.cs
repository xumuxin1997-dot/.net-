using Ruanmou.Libraries.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Libraries.DAL
{
    /// <summary>
    /// Eleven 缓存固定Sql
    /// </summary>
    public class ElevenSqlBuilder<T> where T : BaseModel
    {
        public static string FindSql = null;
        public static string FindAllSql = null;
        public static string AddSql = null;

        static ElevenSqlBuilder()
        {
            Type type = typeof(T);
            FindAllSql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"))} FROM [{type.Name}]";

            FindSql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"))} FROM [{type.Name}] WHERE ID=@Id";

            string columnString = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => $"[{p.Name}]"));
            string valueColumn = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => $"@{p.Name}"));
            AddSql = $"Insert [{type.Name}] ({columnString}) values({valueColumn})";
        }

    }
}
