
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

        public GUIController() { }

        public GUIController(Orquestador.Orquestador orquestador)
        {
            _orquestador = orquestador;
        }

        public void Inicializar()
        {
            MainWindow = new MainWindow();
            SensorsCheckPage = new SensorsCheck(this);
            MainWindow.NavigationService.Navigate(SensorsCheckPage);
            Application app = new Application();
            app.Run(MainWindow);
        }

        public void CheckearSensores()
        {
            //_orquestrator.Inicializar();
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

        public void IniciarSimulacion(DatosPracticante datosPracticante)
        {
            //_orquestrator.IniciarSimulacion(datosPracticante);
        }


    }
}