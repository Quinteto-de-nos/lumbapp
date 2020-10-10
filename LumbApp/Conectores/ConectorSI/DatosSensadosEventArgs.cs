using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Conectores.ConectorSI
{
    public class DatosSensadosEventArgs : EventArgs
    {
        public String datosSensados { get; private set; }
        public DatosSensadosEventArgs(String datosNuevos)
        {
            this.datosSensados = datosNuevos;
        }
    }
}
