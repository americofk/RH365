using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Filter
{
    public class SearchFilter
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }

    public class SearchFilter<T>
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public object ObjectPropertyValue { get; set; }
        public Type PropertyType { get; set; }
        private bool SuccessConvert { get; set; } = false;

        public SearchFilter(string _PropertyName, string _PropertyValue)
        {
            this.PropertyName = _PropertyName == null ? "": _PropertyName.Substring(3); //Aplicar substring para buscar el nombre de la propiedad
            this.PropertyValue = _PropertyValue == null ? "" : _PropertyValue;

            this.PropertyType = typeof(T).GetProperty(PropertyName)?.PropertyType;

            if (this.PropertyType == typeof(DateTime))
            {
                this.SuccessConvert = DateTime.TryParseExact(this.PropertyValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime a);
                this.ObjectPropertyValue = a;
            }    

            if (this.PropertyType == typeof(decimal))
            {
                this.SuccessConvert = decimal.TryParse(this.PropertyValue, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal a);
                this.ObjectPropertyValue = a;
            }
            
            if (this.PropertyType == typeof(string))
            {
                this.ObjectPropertyValue = this.PropertyValue;
                this.SuccessConvert = true;
            }
        }

        public bool IsValid()
        {
            return typeof(T).GetProperty(PropertyName) != null
                    && !string.IsNullOrEmpty(PropertyValue)
                    && SuccessConvert == true;
        }
    }
}
