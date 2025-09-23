using D365_API_Nomina.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace D365_API_Nomina.Core.Application.Common.Helper
{
    public static class FillStringHelper
    {
        public static string Fill(AlignDirection _direction, string _text, int _totalLenght, char _paddingChar )
        {
            string newString = string.Empty;

            if(_direction == AlignDirection.Left)
                newString = _text.PadLeft(_totalLenght, _paddingChar);
            else
                newString = _text.PadRight(_totalLenght, _paddingChar);

            return newString;
        }
    }
}
