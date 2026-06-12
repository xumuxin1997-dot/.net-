using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data
{
    /// <summary>
    /// 表示在属性上保存一个列名(ColumnName)的元数据，供其他代码通过反射读取。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnElevenAttribute : Attribute
    {
        private string _ColumnName = null;
        public ColumnElevenAttribute(string columnName)
        {
            this._ColumnName = columnName;
        }

        public string GetColumnName()
        {
            return this._ColumnName;
        }
    }
}
