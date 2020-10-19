using LumbApp.Enums;
using LumbApp.Models;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Conectores.ConectorFS;
using LumbApp.Expertos.ExpertoSI;

namespace LumbApp.GUI
{
    public class GUIController : IGUIController
    {
        private Orquestador.Orquestador _orquestador { get; set; }
        private MainWindow MainWindow { get; set; }
        private SensorsCheck SensorsCheckPage { get; set; }
        private IngresoDatosPracticante IngresoDatosPracticantePage { get; set; }
        private SimulacionModoGuiado SimulacionModoGuiadoPage { get; set; }
        private SimulacionModoEvaluacion SimulacionModoEvaluacionPage { get; set; }
        private ResultadosSimulacion ResultadosSimulacionPage { get; set; }

        public GUIController(MainWindow mainWindow)
        {

            MainWindow = mainWindow;
        }

        /// <summary>
        /// Lo llama el main window para instanciar el orquestador
        /// </summary>
        public void Inicializar()
        {
            SensorsCheckPage = new SensorsCheck(this);
            MainWindow.NavigationService.Navigate(SensorsCheckPage);
            SensorsCheckPage.MostrarCheckeandoSensores();
            _orquestador = new Orquestador.Orquestador(this, new ConectorFS());
            _ = _orquestador.Inicializar();
        }

        public void CheckearSensores()
        {
            _ = _orquestador.Inicializar();  //si fallo la primera vez reintento
        }

        public void NuevaSimulacion()
        {
            _ = _orquestador.NuevaSimulacion();  //si fallo la primera vez reintento
        }

        /// <summary>
        /// Lo llama el orquestador  para mostrar por que fallo la inicializacion de los sensores
        /// </summary>
        /// <param name="mensaje"></param>
        public void MostrarErrorDeConexion(string mensaje)
        {
            SensorsCheckPage.MostrarErrorDeConexion(mensaje);
        }

        /// <summary>
        /// Lo llama el orquestador si finalizo bien la inicializacion de los sensores para mostrar el 'login' del practicante
        /// </summary>
        public void SolicitarDatosPracticante(DatosPracticante datosPrevios)
        {
            IngresoDatosPracticantePage = new IngresoDatosPracticante(this, datosPrevios);
            MainWindow.NavigationService.Navigate(IngresoDatosPracticantePage);
        }

        /// <summary>
        /// Lo llama la pagina cuando el usuario finaliza de ingresar los datos y selecciona 'Iniciar Simulacion'
        /// </summary>
        /// <param name="datosPracticante"></param>
        /// <param name="modoSeleccionado"></param>
        public void FinIngresoDatos(Models.DatosPracticante datosPracticante, ModoSimulacion modoSeleccionado)
        {
            _orquestador.SetDatosDeSimulacion(datosPracticante, modoSeleccionado);
            MostrarPasosPreparacion();
        }

        /// <summary>
        /// Muestra los pasos de preparacion
        /// </summary>
        private void MostrarPasosPreparacion()
        {
            PasosPreparacion pasosPreparacionPage = new PasosPreparacion(this);
            MainWindow.NavigationService.Navigate(pasosPreparacionPage);
        }

        /// <summary>
        /// Lo llama la pagina cuando termina el timer o selecciona Saltear Preparacion
        /// </summary>
        public void FinPreparacion()
        {
            _orquestador.IniciarSimulacion();
        }

        /// <summary>
        /// Muestra la pantalla de la simulacion 
        /// </summary>
        public void IniciarSimulacionModoGuiado()
        {
            SimulacionModoGuiadoPage = new SimulacionModoGuiado(this);
            MainWindow.NavigationService.Navigate(SimulacionModoGuiadoPage);
        }

        /// <summary>
        /// Muestra la pantalla de la simulacion 
        /// </summary>
        public void IniciarSimulacionModoEvaluacion()
        {
            SimulacionModoEvaluacionPage = new SimulacionModoEvaluacion(this);
            MainWindow.NavigationService.Navigate(SimulacionModoEvaluacionPage);
        }

        public void MostrarCambioZE(CambioZEEventArgs e)
        {
            SimulacionModoGuiadoPage.MostrarCambioZE(e);
        }

        public void MostrarCambioSI(CambioSIEventArgs datosDelEvento)
        {
            SimulacionModoGuiadoPage.MostrarCambioSI(datosDelEvento);
        }

        public void FinalizarSimulacion()
        {
            ResultadosSimulacionPage = new ResultadosSimulacion(this);
            MainWindow.NavigationService.Navigate(ResultadosSimulacionPage);
            _ = _orquestador.TerminarSimulacion();
        }

        public void Finalizar()
        {
            _orquestador.Finalizar();
        }

        public void MostrarResultados(Informe informe)
        {
            ResultadosSimulacionPage.MostrarResultados(informe);
        }

    }
}
