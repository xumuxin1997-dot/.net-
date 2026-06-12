using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruanmou.Framework.Data.ValidateExtend
{
    public abstract class AbstractValidateAttribute : Attribute
    {
        //private Func<object, ValidateErrorModle> _Func;
        //public AbstractValidateAttribute(Func<object, ValidateErrorModle> func)
        //{
        //    this._Func = func;
        //}
        //public ValidateErrorModle ValidateSelf(object oValue)
        //{
        //    //，，，，
        //    return this._Func.Invoke(oValue);
        //}
        ////子类不需要再去override方法  不同的校验方式通过构造函数用委托传递上来


        public abstract ValidateErrorModle Validate(object oValue);
    }
}
