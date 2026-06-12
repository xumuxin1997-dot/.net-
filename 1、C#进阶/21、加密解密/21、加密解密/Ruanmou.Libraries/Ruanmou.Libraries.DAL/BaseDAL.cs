using Ruanmou.Framework.Data;
using Ruanmou.Libraries.IDAL;
using Ruanmou.Libraries.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Libraries.DAL
{
    /// <summary>
    /// Eleven 为什么要约束？
    /// 1 希望调用者不要犯错，避免将其他实体传进来
    /// 2 BaseModel 保证一定有ID 而且是int 自增主键
    /// </summary>
    public class BaseDAL : IBaseDAL
    {
        private static string ConnectionStringCustomers = ConfigurationManager.ConnectionStrings["Customers"].ConnectionString;
        public bool Add<T>(T t) where T : BaseModel
        {
            //Eleven
            //id是自增的  所以不能新增

            Type type = t.GetType();

            string columnString = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)//不要父类的
                                                                                                                                              //.Where(p=>!p.Name.Equals("Id"))//去掉id--主键约束了
                .Select(p => $"[{p.Name}]"));
            //string valueColumn = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Select(p => $"'{p.GetValue(t)}'"));
            //引号怎么加---sqlserver 任意值都可以加单引号
            //假如都加引号，如果Name的值里面有个单引号，sql变成什么样的  Eleven's  sql会错
            //还有sql注入风险
            //所以要参数化
            string valueColumn = string.Join(",", type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => $"@{p.Name}"));
            var parameterList = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Select(p => new SqlParameter($"@{p.Name}", p.GetValue(t) ?? DBNull.Value));//注意可空类型

            string sql = $"Insert [{type.Name}] ({columnString}) values({valueColumn})";
            using (SqlConnection conn = new SqlConnection(ConnectionStringCustomers))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddRange(parameterList.ToArray());
                conn.Open();
                return command.ExecuteNonQuery() == 1;
                //新增后把id拿出来？  可以的，在sql后面增加个 Select @@Identity; ExecuteScalar
            }
        }
        /// <summary>
        /// 可以提供给增删改查 到处用的
        /// 自己试试，怎样来调用这个方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private T ExecuteSql<T>(string sql, Func<SqlCommand, T> func)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionStringCustomers))
            {
                //conn.BeginTransaction()
                //try
                //{
                //}
                //catch (Exception)
                //{
                //   //ROLLBack
                //    throw;
                //}
                SqlCommand command = new SqlCommand(sql, conn);
                //command.Parameters.AddRange(parameterList.ToArray());
                conn.Open();
                return func.Invoke(command);
                //新增后把id拿出来？  可以的，在sql后面增加个 Select @@Identity; ExecuteScalar
            }
        }


        public bool Delete<T>(T t) where T : BaseModel
        {
            //t.Id
            throw new NotImplementedException();
        }

        public List<T> FindAll<T>() where T : BaseModel
        {
            Type type = typeof(T);
            //string sql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{p.Name}]"))} FROM [{type.Name}]";

            //你的类名称如果跟命名空间重复了，也不能用
            string sql = ElevenSqlBuilder<T>.FindAllSql;
            using (SqlConnection conn = new SqlConnection(ConnectionStringCustomers))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                var reader = command.ExecuteReader();
                List<T> tList = new List<T>();
                //object oObject = Activator.CreateInstance(type);
                while (reader.Read())
                {
                    //object oObject = Activator.CreateInstance(type);
                    ////Eleven 放在外面 对象是同一个，引用类型
                    //foreach (var prop in type.GetProperties())
                    //{
                    //    prop.SetValue(oObject, reader[prop.Name] is DBNull ? null : reader[prop.Name]);
                    //}

                    tList.Add(this.Trans<T>(type, reader));
                }
                return tList;
            }
        }

        #region Private Method
        private T Trans<T>(Type type, SqlDataReader reader)
        {
            object oObject = Activator.CreateInstance(type);
            foreach (var prop in type.GetProperties())
            {
                //prop.SetValue(oObject, reader[prop.Name]]);
                //Eleven 可空类型，如果数据库存储的是null，直接SetValue会报错的
                //prop.SetValue(oObject, reader[prop.GetColumnName()] is DBNull ? null : reader[prop.Name]);
                prop.SetValue(oObject, reader[prop.Name] is DBNull ? null : reader[prop.Name]);
            }
            return (T)oObject;
        }
        #endregion


        public T FindT<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            //如果数据库的表/字段名称和程序中实体不一致，尝试用特性提供，解决增删改查
            //2种做法，要么拼装和绑定都用特性  要么就是AS一下
            //string sql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}]"))} FROM [{type.Name}] WHERE ID={id}";
            string sql = $"SELECT {string.Join(",", type.GetProperties().Select(p => $"[{p.GetColumnName()}] AS [{p.Name}]"))} FROM [{type.Name}] WHERE ID={id}";
            using (SqlConnection conn = new SqlConnection(ConnectionStringCustomers))
            {
                SqlCommand command = new SqlCommand(sql, conn);
                conn.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    //object oObject = Activator.CreateInstance(type);
                    //foreach (var prop in type.GetProperties())
                    //{
                    //    //prop.SetValue(oObject, reader[prop.Name]]);
                    //    //Eleven 可空类型，如果数据库存储的是null，直接SetValue会报错的
                    //    prop.SetValue(oObject, reader[prop.Name] is DBNull ? null : reader[prop.Name]);
                    //}
                    return this.Trans<T>(type, reader);
                }
                else
                {
                    return null;//Eleven  数据库没有，应该返回null  而不是一个默认对象
                }
            }
        }

        public bool Update<T>(T t) where T : BaseModel
        {
            throw new NotImplementedException();
        }
    }
}
