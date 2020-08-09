
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
            //orquestador.IniciarSimulacion();????
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
            SimulacionModoGuiado simulacionModoGuiadoPage = new SimulacionModoGuiado(this);
            MainWindow.NavigationService.Navigate(simulacionModoGuiadoPage);
        }

        /// <summary>
        /// Muestra la pantalla de la simulacion 
        /// </summary>
        public void IniciarSimulacionModoEvaluacion()
        {

        }

        public void MostrarCambioZE(CambioZEEventArgs e)
        {
            
        }

        public void FinalizarSimulacion()
        {
            _orquestador.TerminarSimulacion();
            //mostrar_procesado_resultados();
        }

        public void MostrarResultados( Informe informe)
        {

        }

    }
}