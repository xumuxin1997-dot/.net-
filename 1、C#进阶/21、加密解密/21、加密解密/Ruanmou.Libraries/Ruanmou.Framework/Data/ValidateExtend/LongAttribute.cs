using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data.ValidateExtend
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LongAttribute : AbstractValidateAttribute
    {
        private long _Min = 0;
        private long _Max = 0;
        public LongAttribute(long min, long max)
        {
            this._Min = min;
            this._Max = max;
        }

        public override ValidateErrorModle Validate(object oValue)
        {
            throw new Exception();
            //return oValue != null
            //    && long.TryParse(oValue.ToString(), out long lValue)
            //    && lValue >= this._Min
            //    && lValue <= this._Max;
        }

    }
}
