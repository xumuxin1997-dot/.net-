using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayElevenAttribute : Attribute
    {
        private string _DisplayName = null;
        public DisplayElevenAttribute(string displayName)
        {
            this._DisplayName = displayName;
        }

        public string GetDisplayName()
        {
            return this._DisplayName;
        }
    }
}
