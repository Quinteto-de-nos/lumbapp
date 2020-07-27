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
        public MainWindow _mainWindow { get; set; }

        public GUIController(IOrquestrator orquestrator)
        {
            _orquestrator = orquestrator;
            _mainWindow = new MainWindow();
            SensorsCheck sensorCheckPage = new SensorsCheck(this);
            _mainWindow.NavigationService.Navigate(sensorCheckPage);
            Application app = new Application();
            app.Run(_mainWindow);
        }

        public void GetUserData()
        {
            UserDataInput userDataInputPage = new UserDataInput(this);
            _mainWindow.NavigationService.Navigate(userDataInputPage);
        }


    }
}
