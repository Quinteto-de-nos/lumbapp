using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Models
{
    public class TuplaDeStrings
    {
        public string Clave { get; private set; }
        public string Valor { get; private set; }

        public TuplaDeStrings (string clave, string valor)
        {
            this.Clave = clave;
            this.Valor = valor;
        }
    }
}
