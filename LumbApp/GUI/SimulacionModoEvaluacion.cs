using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SimulacionModoGuiado.xaml
    /// </summary>
    public partial class SimulacionModoEvaluacion : Page
    {
        public GUIController _controller { get; set; }

        public SimulacionModoEvaluacion(GUIController gui)
        {
            InitializeComponent();

            _controller = gui;
        }


        #region Finalizar Simulacion
        private void FinalizarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.FinalizarSimulacion();
        }

        #endregion

    }
}
