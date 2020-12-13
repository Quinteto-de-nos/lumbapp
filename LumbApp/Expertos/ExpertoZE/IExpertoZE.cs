using System;

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
