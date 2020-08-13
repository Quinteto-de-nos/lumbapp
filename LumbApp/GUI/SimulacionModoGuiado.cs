using LumbApp;
using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Orquestador;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SimulacionModoGuiado.xaml
    /// </summary>
    public partial class SimulacionModoGuiado : Page
    {
        public GUIController _controller { get; set; }
        private string _imagesFolderPath { get; set; }

        public SimulacionModoGuiado(GUIController gui)
        {
            InitializeComponent();
            _imagesFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\GUI\\Imagenes\\";
            ImageSource startHandsSource = new BitmapImage(new Uri(_imagesFolderPath + "Manos\\NoTraqueando\\mano-fuera-inicial.png", UriKind.Absolute));

            ManoIzquierda.Source = startHandsSource;
            ManoDerecha.Source = startHandsSource;
            //cargar primeras imagenes de capas y vertebra
            _controller = gui;
        }

        #region Cambios en Manos
        public void MostrarPrimerIngresoZE(bool manoIzq)
        {
            ImageSource source = new BitmapImage(new Uri(_imagesFolderPath + "Manos\\Traqueando\\mano-dentro.png", UriKind.Absolute));
            if (manoIzq)
                ManoIzquierda.Source = source;
            else
                ManoDerecha.Source = source;
        }

        public void MostrarSalidaDeZE(bool manoIzq)
        {

        }

        public void MostrarIngresoContaminadoZE(bool manoIzq, int nroIngreso)
        {

        }

        public void MostrarPerdidaTrackeo(bool manoIzq)
        {

        }

        internal void MostrarVuelveTrackeo(bool manoIzq)
        {
            
        }

        #endregion

    }
}
