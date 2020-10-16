using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Models;

namespace LumbApp.GUI
{
    public interface IGUIController
    {
        void Inicializar();
        void CheckearSensores();
        void MostrarErrorDeConexion(string mensaje);
        void SolicitarDatosPracticante(DatosPracticante folderPath);
        void MostrarCambioZE(CambioZEEventArgs e);
        void IniciarSimulacionModoEvaluacion();
        void IniciarSimulacionModoGuiado();
        void MostrarCambioSI(CambioSIEventArgs datosDelEvento);
        void MostrarResultados(Informe informe);
    }
}