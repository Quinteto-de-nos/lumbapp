using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Conectores.ConectorSI {
    public interface IConectorSI {
        void Conectar ();
        bool ChekearComunicacion ();
        void ActivarSensado ();
        void PausarSensado ();
        void Desconectar ();
        //Es necesario declarar el evento en la interfaz del conector para que pueda ser registrado por el experto
        event EventHandler<DatosSensadosEventArgs> HayDatos;
    }
}
