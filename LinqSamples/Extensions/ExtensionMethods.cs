using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqSamples.Extensions
{
    internal static class ExtensionMethods
    {
        // Quersumme berechnen
        public static int DigitSum(this int number)
        {
            // Ein String ist ein character Array weswegen wir hier Linq benutzen koennen
            return number.ToString().Sum(c => (int)char.GetNumericValue(c));
        }
    }
}
