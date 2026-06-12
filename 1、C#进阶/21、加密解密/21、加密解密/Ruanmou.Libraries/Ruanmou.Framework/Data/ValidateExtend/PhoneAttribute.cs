using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data.ValidateExtend
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PhoneAttribute : AbstractValidateAttribute
    {
        private string _PhoneRegular = @"^[1]+[3,5]+\d{9}";

        public override ValidateErrorModle Validate(object oValue)
        {
            throw new Exception();
            //return oValue != null
            //    && !string.IsNullOrWhiteSpace(oValue.ToString())
            //    && Regex.IsMatch(oValue.ToString(), this._PhoneRegular);
        }

    }
}
