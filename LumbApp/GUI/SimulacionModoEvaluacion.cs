using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoSI.Utils;
using LumbApp.Expertos.ExpertoZE;
using System;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SimulacionModoGuiado.xaml
    /// </summary>
    public partial class SimulacionModoEvaluacion : Page
    {
        public GUIController _controller { get; set; }

        //Path General de Carpeta de Imagenes
        private static readonly string _imagesFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\GUI\\Imagenes\\";


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
