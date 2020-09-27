using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;

namespace LumbApp.GUI
{
    public interface IGUIController
    {
        void Inicializar();
        void CheckearSensores();
        void MostrarErrorDeConexion(string mensaje);
        void SolicitarDatosPracticante(string folderPath);
        void MostrarCambioZE(CambioZEEventArgs e);
        void IniciarSimulacionModoEvaluacion();
        void IniciarSimulacionModoGuiado();
        void MostrarCambioSI(CambioSIEventArgs datosDelEvento);
    }
}