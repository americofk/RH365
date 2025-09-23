using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Helper
{
    public static class BuildDtoHelper<T> where T: class
    { 
        public static T OnBuild(object _FromClass , T _ToClass)
        {
            var propertiesFrom = _FromClass.GetType().GetProperties();
            var propertiesTo = _ToClass.GetType().GetProperties();

            foreach (var propertyInfo in propertiesFrom)
            {
                var propertyInfoTo = propertiesTo.Where(x => x.Name == propertyInfo.Name).FirstOrDefault();

                if(propertyInfoTo != null)
                {
                    propertyInfoTo.SetValue(_ToClass, propertyInfo.GetValue(_FromClass));
                }

            }

            return _ToClass;
        }
    }
}
