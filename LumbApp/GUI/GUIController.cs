
using KinectCoordinateMapping;
using LumbApp.Enums;
using LumbApp.Orquestador;
using LumbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using LumbApp.Expertos.ExpertoZE;

namespace LumbApp.GUI
{
    public class GUIController
    {
        private Orquestador.Orquestador _orquestador { get; set; }
        private MainWindow MainWindow { get; set; }
        private SensorsCheck SensorsCheckPage { get; set; }
        private IngresoDatosPracticante IngresoDatosPracticantePage { get; set; }
        private SimulacionModoGuiado SimulacionModoGuiadoPage { get; set; }
        private ResultadosSimulacion ResultadosSimulacionPage { get; set; }
        private Mano _manoIzquierda { get; set; }
        private Mano _manoDerecha { get; set; }

        public GUIController(MainWindow mainWindow) {

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
            _orquestador = new Orquestador.Orquestador(this);
            _orquestador.Inicializar();
        }

        public void CheckearSensores()
        {
            _orquestador.Inicializar();  //si fallo la primera vez reintento
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
        public void SolicitarDatosPracticante()
        {
            IngresoDatosPracticantePage = new IngresoDatosPracticante(this);
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
        private void MostrarPasosPreparacion() {
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

        }

        public void MostrarCambioZE(CambioZEEventArgs e)
        {
            CheckearCambioTracking(_manoIzquierda.Track, e.ManoIzquierda.Track, true);
            CheckearCambioTracking(_manoDerecha.Track, e.ManoDerecha.Track, false);

            if (e.ContaminadoAhora)
            {
                ChequearSiContamino(_manoIzquierda.Estado, e.ManoIzquierda.Estado, e.ManoIzquierda.VecesContamino, true);
                ChequearSiContamino(_manoDerecha.Estado, e.ManoDerecha.Estado, e.ManoDerecha.VecesContamino, false);
            }
            else
            {
                ChequearSiTrabajaOSalio(_manoIzquierda.Estado, e.ManoIzquierda.Estado, true);
                ChequearSiTrabajaOSalio(_manoIzquierda.Estado, e.ManoIzquierda.Estado, false);
            }
        }

        private void CheckearCambioTracking(Mano.Tracking oldTrack, Mano.Tracking track, bool esIzquierda)
        {
            if( oldTrack != track )
            {
                SimulacionModoGuiadoPage.MostrarCambioTrackeo(esIzquierda);
            }
        }

        private void ChequearSiTrabajaOSalio(Mano.Estados oldState, Mano.Estados state, bool esIzquierda)
        {
            if(oldState != state)
            {
                if (state == Mano.Estados.Trabajando)
                    SimulacionModoGuiadoPage.MostrarPrimerIngresoZE(esIzquierda);
                if (state == Mano.Estados.Fuera)
                    SimulacionModoGuiadoPage.MostrarSalidaDeZE(esIzquierda);
            }
        }

        public void DetenerSimulacion()
        {
            _orquestador.TerminarSimulacion();
        }

        private void ChequearSiContamino(Mano.Estados oldState, Mano.Estados state, int vecesContamino, bool esIzquierda)
        {
            if (state == Mano.Estados.Contaminando && oldState == Mano.Estados.Fuera)
                SimulacionModoGuiadoPage.MostrarIngresoContaminadoZE(esIzquierda, vecesContamino);
        }

        public void FinalizarSimulacion()
        {
            _orquestador.TerminarSimulacion();
            ResultadosSimulacionPage = new ResultadosSimulacion(this);
            MainWindow.NavigationService.Navigate(ResultadosSimulacionPage);
        }

        public void MostrarResultados( Informe informe )
        {

        }

    }
}
