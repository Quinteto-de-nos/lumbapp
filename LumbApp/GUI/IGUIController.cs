using LumbApp.Expertos.ExpertoZE;
using LumbApp.Models;

namespace LumbApp.GUI
{
    public interface IGUIController
    {
        void Inicializar();
        void CheckearSensores();
        void MostrarErrorDeConexion(string mensaje);
        void SolicitarDatosPracticante();
        void MostrarCambioZE(CambioZEEventArgs e);
        void IniciarSimulacionModoEvaluacion();
        void IniciarSimulacionModoGuiado();
        void MostrarResultados(Informe informe);
    }
}