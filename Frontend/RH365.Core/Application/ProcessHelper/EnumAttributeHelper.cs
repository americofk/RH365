using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RH365.Core.Application.ProcessHelper
{
    public static class EnumAttributeHelper<T>
    {
        public static int GetAttributeName(string valueToSearch, out int realValue)
        {
            realValue = 9999;
            var members = typeof(T).GetFields();
            int valueMember = 9999;

            foreach (var item in members)
            {
                var a = item.GetCustomAttributes(typeof(DisplayAttribute), false);

                if (a.Count() != 0)
                {
                    var b = (DisplayAttribute)a.First();
                    if (b.Name.ToLower() == valueToSearch.ToLower())
                    {
                        realValue = (int)Enum.Parse(typeof(T), item.Name);
                        return valueMember = (int)Enum.Parse(typeof(T), item.Name);
                    }
                }
            }
            return valueMember;
        }
    }
}
