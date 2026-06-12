
using System;
using System.Linq;
using System.Xml;
using System.Reflection;
using System.Data;
using System.Collections.Generic;

namespace IOSerialize.Serialize
{
    public static class xHelper
    {
        /// <summary>   
        /// 实体转化为XML   
        /// 本质思想就是通过反射获取实体的属性和属性值，然后拼装成XML字符串。
        /// </summary>   
        public static string ParseToXml<T>(this T model, string fatherNodeName)
        {
            var xmldoc = new XmlDocument();
            var modelNode = xmldoc.CreateElement(fatherNodeName);//创建XML元素节点
            xmldoc.AppendChild(modelNode);//向xmldoc添加modelNode节点。

            if (model != null)
            {//拼装XML树
                foreach (PropertyInfo property in model.GetType().GetProperties())
                {
                    var attribute = xmldoc.CreateElement(property.Name);//在子节点下创建一个元素
                    if (property.GetValue(model, null) != null)//PropertyInfo.GetValue方法是获取某个对象的属性值
                        attribute.InnerText = property.GetValue(model, null).ToString();/*XmlElement.InnerText属性获取
                                                                                        或设置当前节点的文本内容*/
                    //else
                    //    attribute.InnerText = "[Null]";
                    modelNode.AppendChild(attribute);//向modelNode中添加attribute节点
                }
            }
            return xmldoc.OuterXml;
        }

        /// <summary>
        /// XML转换为实体,默认 fatherNodeName="body"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="fatherNodeName"></param>
        /// <returns></returns>
        public static T ParseToModel<T>(this string xml, string fatherNodeName = "body") where T : class ,new()
        {
            if (string.IsNullOrEmpty(xml))
                return default(T);
            var xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);//XmlDocument.LoadXml方法将一个 XML 格式的字符串解析成 XmlDocument 对象。
            T model = new T();
            var attributes = xmldoc.SelectSingleNode(fatherNodeName).ChildNodes;//找到fatherNodeName这个标签，并获得它的子节点集合
                                                                                //XmlNode.SelectSingleNode方法根据 XPath 表达式返回匹配的第一个 XmlNode。
                                                                                //XmlNode.ChildNodes表示获取当前节点的所有直接子节点。
            foreach (XmlNode node in attributes)
            {
                foreach (var property in model.GetType()
                                              .GetProperties()
                                              .Where(property => node.Name == property.Name))
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                    {//如果节点的文本内容不为空，则将文本内容转换为属性的类型，并设置到属性上
                        property.SetValue(model,
                                          property.PropertyType == typeof(Guid)
                                              ? new Guid(node.InnerText)
                                              : Convert.ChangeType(node.InnerText, property.PropertyType));//将节点的文本内容转换为属性的类型，并设置到属性上
                                                                                                           //三元表达式
                                                                                                           //如果属性的类型是Guid，则创建一个新的Guid对象，否则使用Convert.ChangeType方法将节点的文本内容转换为属性的类型。
                                                                                                           
                    }
                    else
                    {
                        property.SetValue(model, null);
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// XML转多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="headtag"></param>
        /// <returns></returns>
        public static List<T> XmlToObjList<T>(this string xml, string headtag)
            where T : new()
        {

            var list = new List<T>();
            XmlDocument doc = new XmlDocument();
            PropertyInfo[] propinfos = null;
            doc.LoadXml(xml);
            XmlNodeList nodelist = doc.SelectNodes(headtag);//查找所有符合条件的子节点，并返回一个节点集合。
            foreach (XmlNode node in nodelist)
            {
                T entity = new T();
                if (propinfos == null)
                {
                    Type objtype = entity.GetType();
                    propinfos = objtype.GetProperties();
                }
                foreach (PropertyInfo propinfo in propinfos)
                {
                    //实体类字段首字母变成小写的  
                    string name = propinfo.Name.Substring(0, 1) + propinfo.Name.Substring(1, propinfo.Name.Length - 1);//这里可以给首字母做转换
                    XmlNode cnode = node.SelectSingleNode(name);
                    string v = cnode.InnerText;
                    if (v != null)
                        propinfo.SetValue(entity, Convert.ChangeType(v, propinfo.PropertyType), null);
                }
                list.Add(entity);

            }
            return list;
        }
    }
}
