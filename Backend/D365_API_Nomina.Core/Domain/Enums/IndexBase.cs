using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Domain.Enums
{
    public enum IndexBase
    {
        Hour = 0, //Hora
        PayPeriod = 1,// Periodo de pago
        Monthly = 2,//Mensual
        Yearly = 3, //Anual
        FixedAmount = 4, //Monto fijo
        Retroactive = 5, //Retroactivo
        StandardWageRate = 6, // Indice salarial estandar
        EarningPercent = 7, // Porcenatge de ganancia
        EarningHours = 8 // Horas de trabajo
    }
}
