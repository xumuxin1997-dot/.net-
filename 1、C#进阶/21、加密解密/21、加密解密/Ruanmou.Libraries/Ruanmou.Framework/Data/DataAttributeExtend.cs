using Ruanmou.Framework.Data.ValidateExtend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data
{
    /// <summary>
    /// Eleven  也就是 名称:Eleven；
    /// </summary>
    public static class DataAttributeExtend
    {
        private static string GetAttributeName(this PropertyInfo property, Func<PropertyInfo, bool> func, Func<PropertyInfo, string> funcString)//意义不是很大
        {
            if (func.Invoke(property))
            {
                return funcString.Invoke(property);
            }
            else
            {
                return property.Name;
            }
        }
        public static string GetDisplayName(this PropertyInfo property)
        {
            return property.GetAttributeName(p => p.IsDefined(typeof(DisplayElevenAttribute), true),
                p =>
                {
                    DisplayElevenAttribute attribute = (DisplayElevenAttribute)property.GetCustomAttribute(typeof(DisplayElevenAttribute), true);
                    return attribute.GetDisplayName();
                });

            //if (property.IsDefined(typeof(DisplayElevenAttribute), true))
            //{
            //    DisplayElevenAttribute attribute = (DisplayElevenAttribute)property.GetCustomAttribute(typeof(DisplayElevenAttribute), true);
            //    return attribute.GetDisplayName();
            //}
            //else
            //{
            //    return property.Name;
            //}
        }
        /// <summary>
        /// 列名称的获取
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetColumnName(this PropertyInfo property)
        {
            if (property.IsDefined(typeof(ColumnElevenAttribute), true))
            {
                ColumnElevenAttribute attribute = (ColumnElevenAttribute)property.GetCustomAttribute(typeof(ColumnElevenAttribute), true);
                //return $"[{attribute.GetColumnName()}] AS [{property.Name}]";

                return attribute.GetColumnName();
            }
            else
            {
                return property.Name;
            }
        }
        public static ValidateErrorModle Validate<T>(this T t)
        {
            Type type = t.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(AbstractValidateAttribute), true))
                {
                    object oValue = prop.GetValue(t);
                    foreach (AbstractValidateAttribute attribute in prop.GetCustomAttributes(typeof(AbstractValidateAttribute), true))
                    {
                        var result = attribute.Validate(oValue);
                        if (!result.Result)
                            return result;
                    }
                }
            }
            return new ValidateErrorModle()
            {
                Result = true,
                Message = "校验成功"
            };
        }
        public static List<ValidateErrorModle> ValidateAll<T>(this T t)
        {
            List<ValidateErrorModle> validates = new List<ValidateErrorModle>();
            Type type = t.GetType();
            foreach (var prop in type.GetProperties())
            {
                if (prop.IsDefined(typeof(AbstractValidateAttribute), true))
                {
                    object oValue = prop.GetValue(t);
                    foreach (AbstractValidateAttribute attribute in prop.GetCustomAttributes(typeof(AbstractValidateAttribute), true))
                    {
                        var result = attribute.Validate(oValue);
                        validates.Add(result);
                    }
                }
            }
            return validates;
            //validates.Where(m=>!m.Result)
        }
    }
}
