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
        private static Brush[] coloresContaminando { get; set; }
        private BrushConverter brushConverter { get; set; }
        private Brush white { get; set; }
        private Brush black { get; set; }
        private Brush green { get; set; }
        private Brush darkred { get; set; }

        public SimulacionModoGuiado(GUIController gui)
        {
            InitializeComponent();

            //inicializo imagenes de manos
            ImageSource startHandsSource = new BitmapImage(
                new Uri(_imagesFolderPath + _manoPerdidaSubPath + _manoFueraInicialPath, UriKind.Absolute));

            ManoIzquierda.Source = startHandsSource;
            ManoDerecha.Source = startHandsSource;

            //inicializo colores
            brushConverter = new BrushConverter();
            white = (Brush)brushConverter.ConvertFrom("#ffffff");
            black = (Brush)brushConverter.ConvertFrom("#323232");
            green= (Brush)brushConverter.ConvertFrom("#a7f192");
            darkred = (Brush)brushConverter.ConvertFrom("#bc100c");
            string[] colores = { "#dcf192", "#f1ef92", "#f1d792", "#f1b392", "#f19292", "#f3817e", "#ff6161" };
            coloresContaminando = new Brush[colores.Count()];
            for(int i =0; i< colores.Count() ;i++)
            {
                coloresContaminando[i]= (Brush)brushConverter.ConvertFrom(colores[i]);
            }

            //inicializo los labels de las manos
            var colorLabel = white;
            ManoIzqLabel.Background = colorLabel;
            ManoIzqLabel.Content = "Inicial";
            ManoDerLabel.Background = colorLabel;
            ManoDerLabel.Content = "Inicial";
            //cargar primeras imagenes de capas y vertebra
            //..............
            _controller = gui;
        }

        #region Cambios en Manos

        public void MostrarCambioZE(CambioZEEventArgs e)
        {
            //ManoIzquierda
            ManosImageConfig config = GetNuevaConfiguracionImagen(e.ManoIzquierda.Track, e.ManoIzquierda.Estado, e.ManoIzquierda.VecesContamino);

            ImageSource nuevaImagen = new BitmapImage(
               new Uri(_imagesFolderPath +config.TrackingPath +config.EstadoPath, UriKind.Absolute));
            ManoIzquierda.Source = nuevaImagen;
            ManoIzqLabel.Background = config.LabelColor;
            ManoIzqLabel.Foreground = config.FontColor;
            ManoIzqLabel.Content = config.Texto;

            //ManoDerecha
            config = GetNuevaConfiguracionImagen(e.ManoDerecha.Track, e.ManoDerecha.Estado, e.ManoDerecha.VecesContamino);

            nuevaImagen = new BitmapImage(
               new Uri(_imagesFolderPath + config.TrackingPath + config.EstadoPath, UriKind.Absolute));
            ManoDerecha.Source = nuevaImagen;
            ManoDerLabel.Background = config.LabelColor;
            ManoDerLabel.Foreground = config.FontColor;
            ManoDerLabel.Content = config.Texto;
        }            

        public ManosImageConfig GetNuevaConfiguracionImagen(Mano.Tracking track, Mano.Estados estado, int nroIngreso)
        {
            ManosImageConfig config = new ManosImageConfig();

            //config trackeo
            config.TrackingPath = (track == Mano.Tracking.Perdido ? _manoPerdidaSubPath : _manoTrackeadaSubPath);

            //config estado
            switch (estado)
            {
                case Mano.Estados.Inicial:
                    config.EstadoPath = _manoFueraInicialPath;
                    config.LabelColor = white;
                    config.FontColor = black;
                    config.Texto = "Inicial";
                    break;
                case Mano.Estados.Trabajando:
                    config.EstadoPath = _manoDentroPath;
                    config.LabelColor = green;
                    config.FontColor = black;
                    config.Texto = "Trabajando";
                    break;
                case Mano.Estados.Fuera:
                    config.EstadoPath = _manoFueraPath;
                    config.LabelColor = darkred;
                    config.FontColor = white;
                    config.Texto = "Fuera";
                    break;
                case Mano.Estados.Contaminando:
                    {
                        int nroContaminacion = (nroIngreso > 7 ? 7 : nroIngreso);
                        config.EstadoPath = _manoContaminadaPath.Replace("X", nroContaminacion.ToString()); ;
                        config.LabelColor = coloresContaminando[nroContaminacion-1];
                        config.FontColor = black;
                        config.Texto = "Contaminando";
                        break;
                    }
            }

            return config;
        }

        #endregion

        #region Finalizar Simulacion

        private void FinalizarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.FinalizarSimulacion();
        }

        #endregion

    }

     public class ManosImageConfig
    {
        public Brush LabelColor { get; set; }
        public Brush FontColor { get; set; }
        public string Texto { get; set; }
        public string EstadoPath { get; set; }
        public string TrackingPath { get; set; }

    }
}
