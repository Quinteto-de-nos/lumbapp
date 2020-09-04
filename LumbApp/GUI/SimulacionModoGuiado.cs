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
        private static string[] coloresContaminando = { "#dcf192", "#f1ef92", "#f1d792", "#f1b392", "#f19292", "#f3817e", "#ff6161" };
        private Mano.Tracking manoIzqTrack { get; set; }
        private Mano.Tracking manoDerTrack { get; set; }
        private string manoIzqEstadoPath { get; set; }
        private string manoDerEstadoPath { get; set; }
        private string manoIzqTrackPath { get; set; }
        private string manoDerTrackPath { get; set; }
        private BrushConverter brushConverter { get; set; }

        public SimulacionModoGuiado(GUIController gui)
        {
            InitializeComponent();

            //inicializo manos como no trackeadas
            manoIzqTrack = manoDerTrack = Mano.Tracking.Perdido;

            //inicializo paths de estados de manos
            manoIzqEstadoPath = manoDerEstadoPath = _manoFueraInicialPath;

            //inicializo paths de tracking de manos
            manoIzqTrackPath = manoDerTrackPath = _manoPerdidaSubPath;

            //inicializo imagenes de manos
            ImageSource startHandsSource = new BitmapImage(
                new Uri(_imagesFolderPath + manoIzqTrackPath + manoIzqEstadoPath, UriKind.Absolute));
            ManoIzquierda.Source = startHandsSource;
            ManoDerecha.Source = startHandsSource;
            brushConverter = new BrushConverter();
            
            //inicializo los labels de las manos
            var colorLabel = (Brush)brushConverter.ConvertFrom("#ffffff");
            ManoIzqLabel.Background = colorLabel;
            ManoIzqLabel.Content = "Inicial";
            ManoDerLabel.Background = colorLabel;
            ManoDerLabel.Content = "Inicial";
            //cargar primeras imagenes de capas y vertebra
            //..............
            _controller = gui;
        }

        #region Cambios en Manos

            #region Cambios Entradas y Salidas
            
            public void MostrarCambioZE(bool esManoIzq, string newEstadoPath, Brush colorLabel, string textoLabel)
            {
                ImageSource nuevaImagen = new BitmapImage(
                    new Uri(_imagesFolderPath +
                    (esManoIzq ? manoIzqTrackPath : manoDerTrackPath) +
                    newEstadoPath, UriKind.Absolute));

                if (esManoIzq)
                {
                    manoIzqEstadoPath = newEstadoPath;
                    ManoIzquierda.Source = nuevaImagen;
                    ManoIzqLabel.Background = colorLabel;
                    ManoIzqLabel.Content = textoLabel;
                }
                else
                {
                    manoDerEstadoPath = newEstadoPath;
                    ManoDerecha.Source = nuevaImagen;
                    ManoDerLabel.Background = colorLabel;
                    ManoDerLabel.Content = textoLabel;
                }
            }
            public void MostrarPrimerIngresoZE(bool esManoIzq)
            {
                var colorLabel = (Brush)brushConverter.ConvertFrom("#a7f192");
                MostrarCambioZE(esManoIzq, _manoDentroPath, colorLabel, "Dentro");
            }

            public void MostrarSalidaDeZE(bool esManoIzq)
            {
                var colorLabel = (Brush)brushConverter.ConvertFrom("#bc100c");
                MostrarCambioZE(esManoIzq, _manoFueraPath, colorLabel, "Fuera");
            }

            public void MostrarIngresoContaminadoZE(bool esManoIzq, int nroIngreso)
            {
                int nroContaminacion = (nroIngreso > 7 ? 7 : nroIngreso);

                string manoContaminadaPath = _manoContaminadaPath;
                    manoContaminadaPath.Replace("X", nroContaminacion.ToString() );

                var colorLabel = (Brush)brushConverter.ConvertFrom(coloresContaminando[nroContaminacion-1]);
                
                MostrarCambioZE(esManoIzq, manoContaminadaPath, colorLabel, "Contaminando");
            }

            #endregion

            #region Cambios Traqueo

            public void MostrarCambioTrackeo(bool esManoIzq)
            {
                Mano.Tracking nuevoEstado = 
                    ((esManoIzq ? manoIzqTrack : manoDerTrack) == Mano.Tracking.Perdido) ? 
                    Mano.Tracking.Trackeado : Mano.Tracking.Perdido;

                string nuevoEstadoPath = 
                    (nuevoEstado == Mano.Tracking.Perdido ? _manoPerdidaSubPath : _manoTrackeadaSubPath);

                ImageSource nuevaImagen = new BitmapImage(
                    new Uri(_imagesFolderPath +
                    //cambio el estado de tracking
                    nuevoEstadoPath + 
                    //dejo el estado actual
                    (esManoIzq? manoIzqEstadoPath : manoDerEstadoPath) , UriKind.Absolute));

                if (esManoIzq)
                {
                    manoIzqTrack = nuevoEstado;
                    manoIzqTrackPath = nuevoEstadoPath;
                    ManoIzquierda.Source = nuevaImagen;
                }
                else
                {
                    manoDerTrack = nuevoEstado;
                    manoDerTrackPath = nuevoEstadoPath;
                    ManoDerecha.Source = nuevaImagen;
                }

            }

        #endregion

        #endregion

        #region Finalizar Simulacion

        private void FinalizarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.DetenerSimulacion();
        }

        #endregion

    }
}
