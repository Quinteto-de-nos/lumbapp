using GUI;
using KinectCoordinateMapping;
using LumbApp.Orquestador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MainWindow = GUI.MainWindow;

namespace LumbApp
{
    public class GUIController
    {
        public IOrquestrator _orquestrator { get; set; }
        public MainWindow MainWindow { get; set; }
        public SensorsCheck SensorsCheckPage { get; set; }
        public IngresoDatosPracticante IngresoDatosPracticantePage { get; set; }

        public GUIController(IOrquestrator orquestrator)
        {
            _orquestrator = orquestrator;
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
