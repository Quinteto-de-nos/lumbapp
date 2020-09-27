using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;

namespace LumbApp.GUI
{
    public interface IGUIController
    {
        void Inicializar();
        void CheckearSensores();
        void MostrarErrorDeConexion(string mensaje);
        void SolicitarDatosPracticante();
        void MostrarCambioZE(CambioZEEventArgs e);
        void MostrarCambioSI(CambioSIEventArgs e);
        void IniciarSimulacionModoEvaluacion();
        void IniciarSimulacionModoGuiado();
    }
}