using System;

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
