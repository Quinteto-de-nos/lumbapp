using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI {
    public interface IExpertoSI {
        event EventHandler<CambioSIEventArgs> CambioSI;
        bool Inicializar ();
        bool IniciarSimulacion ();
        InformeSI TerminarSimulacion ();
        void Finalizar ();
    }
}
