using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RH365.Core.Application.Attributes
{
    public class CustomFilterAttribute: Attribute
    {
        public string PropertyName;

        public CustomFilterAttribute(string _propertyName)
        {
            this.PropertyName = _propertyName;
        }
    }
}
