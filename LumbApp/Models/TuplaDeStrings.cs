using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Models
{
    public class TuplaDeStrings
    {
        string Clave { get; }
        string Valor { get; }

        public TuplaDeStrings (string clave, string valor)
        {
            this.Clave = clave;
            this.Valor = valor;
        }
    }
}
