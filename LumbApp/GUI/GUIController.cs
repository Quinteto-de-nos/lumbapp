
using KinectCoordinateMapping;
using LumbApp.Orquestador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

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
            //SensorsCheckPage = sensorCheckPage;
        }

        //public GUIController(Orquestador.Orquestador orquestador)
        //{
        //    _orquestador = orquestador;
        //}

        public void Inicializar()
        {
            SensorsCheckPage = new SensorsCheck();
            MainWindow.NavigationService.Navigate(SensorsCheckPage);
            SensorsCheckPage.MostrarCheckeandoSensores();
            _orquestador = new Orquestador.Orquestador(this);
        }

        public void CheckearSensores()
        {
            //_orquestrator.Inicializar();  //debe solo iniciar sensores y llamar a mostrar error o no
        }

        public void MostrarErrorDeConexion(string mensaje)
        {
            SensorsCheckPage.MostrarErrorDeConexion(mensaje);
        }

        public void SolicitarDatosPracticante()
        {
            IngresoDatosPracticantePage = new IngresoDatosPracticante(this);
            MainWindow.NavigationService.Navigate(IngresoDatosPracticantePage);
        }

        public void IniciarSimulacion(DatosPracticante datosPracticante, string modoSeleccionado)
        {
            //_orquestrator.SetDatosPracticante(datosPracticante, modoSeleccionado);

        }


    }
}