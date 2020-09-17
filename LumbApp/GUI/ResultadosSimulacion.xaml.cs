using LumbApp.Models;
using System;
using System.Windows;
using System.Windows.Controls;


namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class ResultadosSimulacion : Page
    {
        public GUIController _controller { get; set; }

        public ResultadosSimulacion(GUIController gui)
        {
            InitializeComponent();
            _controller = gui;
        }

        private void NuevaSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.CheckearSensores();
        }

        public void MostrarResultados(Informe informe)
        {
            
        }
    }
}
