using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data.ValidateExtend
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EmailAttribute : AbstractValidateAttribute
    {
        private string EmailRegular = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        public override ValidateErrorModle Validate(object oValue)
        {
            if (oValue == null)
            {
                return new ValidateErrorModle()
                {
                    Result = false,
                    Message = $"{nameof(EmailAttribute)} oValue is null"
                };
            }
            else if (string.IsNullOrWhiteSpace(oValue.ToString()))
            {
                return new ValidateErrorModle()
                {
                    Result = false,
                    Message = $"{nameof(EmailAttribute)} oValue is 空白"
                };
            }
            else if (!Regex.IsMatch(oValue.ToString(), EmailRegular))
            {
                return new ValidateErrorModle()
                {
                    Result = false,
                    Message = $"{nameof(EmailAttribute)} oValue 不满足格式约束"
                };
            }
            else
            {
                return new ValidateErrorModle()
                {
                    Result = true,
                    Message = $"{nameof(EmailAttribute)} 校验成功"
                };
            }
            //return oValue != null
            //    && !string.IsNullOrWhiteSpace(oValue.ToString())
            //    && Regex.IsMatch(oValue.ToString(), EmailRegular);
        }

    }
}
