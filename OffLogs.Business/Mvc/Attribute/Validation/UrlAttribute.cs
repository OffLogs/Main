using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrBusiness.NetCore.MVC.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsUrlAttribute : RegularExpressionAttribute
    {
        public IsUrlAttribute() : base(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$")
        { }
    }
}
