using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LumbApp.GUI.UserDataValidations
{
    public class DniRangeRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int dni = 0;

            try
            {
                if (((string)value).Length > 0)
                    dni = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"Caracteres no permitido o {e.Message}");
            }

            if ((dni < Min) || (dni > Max))
            {
                return new ValidationResult(false,
                  $"Por favor ingrese un dni en el rango: {Min}-{Max}.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
