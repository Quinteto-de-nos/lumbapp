using System;

namespace LumbApp.Expertos.ExpertoSI
{
    public interface IExpertoSI
    {
        event EventHandler<CambioSIEventArgs> CambioSI;
        bool Inicializar();
        bool IniciarSimulacion();
        InformeSI TerminarSimulacion();
        void Finalizar();
    }
}
