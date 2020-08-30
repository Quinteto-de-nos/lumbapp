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
        private static string _imagesFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\GUI\\Imagenes\\";
        private static string _manoPerdidaSubPath = "Manos\\NoTraqueando\\";
        private static string _manoTrackeadaSubPath = "Manos\\Traqueando\\";
        private static string _manoFueraInicialPath = "mano-fuera-inicial.png";
        private static string _manoFueraPath = "mano-fuera.png";
        private static string _manoDentroPath = "mano-dentro.png";
        private static string _manoContaminadaPath = "mano-contaminadaX.png";
        private Mano.Tracking manoIzqTrack { get; set; }
        private Mano.Tracking manoDerTrack { get; set; }
        private string manoIzqPathActual { get; set; }
        private string manoDerPathActual { get; set; }

        public SimulacionModoGuiado(GUIController gui)
        {
            InitializeComponent();
            ImageSource startHandsSource = new BitmapImage(
                new Uri(_imagesFolderPath + _manoPerdidaSubPath + _manoFueraInicialPath, UriKind.Absolute));
            ManoIzquierda.Source = startHandsSource;
            ManoDerecha.Source = startHandsSource;
            //cargar primeras imagenes de capas y vertebra
            //..............
            _controller = gui;
        }

        #region Cambios en Manos

            #region Cambios Entradas y Salidas
            public void MostrarPrimerIngresoZE(bool esManoIzq)
            {
                ImageSource source = new BitmapImage(new Uri(_imagesFolderPath + _manoTrackeadaSubPath + _manoDentroPath, UriKind.Absolute));
                if (esManoIzq)
                    ManoIzquierda.Source = source;
                else
                    ManoDerecha.Source = source;
            }

            public void MostrarSalidaDeZE(bool esManoIzq)
            {

            }

            public void MostrarIngresoContaminadoZE(bool esManoIzq, int nroIngreso)
            {

            }

            #endregion

            #region Cambios Traqueo

            public void MostrarCambioTrackeo(bool esManoIzq)
            {
                if((esManoIzq? manoIzqTrack: manoDerTrack) == Mano.Tracking.Perdido)
                {

                }

            }

            #endregion

        #endregion

    }
}
