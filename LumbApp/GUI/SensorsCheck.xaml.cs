﻿using LumbApp;
using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Orquestador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class SensorsCheck : Page
    {
        public GUIController _controller { get; set; }
        static readonly string MensajeCheckeandoSensores = "Chequeando Sensores";

        public SensorsCheck()
        {
            InitializeComponent();
            Mensaje.Content = "Arranco";
        }

        public void setGuiController(GUIController gui)
        {
            _controller = gui;
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            SpinerIcon.Visibility = Visibility.Visible;
            RetryButton.Visibility = Visibility.Hidden;
            ErrorIcon.Visibility = Visibility.Hidden;
            Mensaje.Content = MensajeCheckeandoSensores;
            _controller.CheckearSensores();
        }

        public void MostrarCheckeandoSensores()
        {
            SpinerIcon.Visibility = Visibility.Visible;
            RetryButton.Visibility = Visibility.Hidden;
            ErrorIcon.Visibility = Visibility.Hidden;
            Mensaje.Content = "Chequeando Sensores";
            //Mensaje.Visibility = Visibility.Visible;
        }

        public void MostrarErrorDeConexion(string mensaje)
        {
            RetryButton.Visibility = Visibility.Visible;
            ErrorIcon.Visibility = Visibility.Visible;
            SpinerIcon.Visibility = Visibility.Hidden;
            Mensaje.Content = mensaje;
        }

    }
}
