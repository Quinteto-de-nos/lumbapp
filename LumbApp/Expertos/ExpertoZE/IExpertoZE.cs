using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoZE
{
    public interface IExpertoZE
    {
        event EventHandler<CambioZEEventArgs> CambioZE;
        bool Inicializar();
        bool IniciarSimulacion(IVideo videoHelper);
        InformeZE TerminarSimulacion();
        void Finalizar();
    }
}
