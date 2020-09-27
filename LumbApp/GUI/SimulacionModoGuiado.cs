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


namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SimulacionModoGuiado.xaml
    /// </summary>
    public partial class SimulacionModoGuiado : Page
    {
        public GUIController _controller { get; set; }

        //Path General de Carpeta de Imagenes
        private static string _imagesFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\GUI\\Imagenes\\";

        //Paths de manos
        private static string _manoPerdidaSubPath = "Manos\\NoTraqueando\\";
        private static string _manoTrackeadaSubPath = "Manos\\Traqueando\\";
        private static string _manoFueraInicialPath = "mano-fuera-inicial.png";
        private static string _manoFueraPath = "mano-fuera.png";
        private static string _manoDentroPath = "mano-dentro.png";
        private static string _manoContaminadaPath = "mano-contaminadaX.png";

        //Paths de capas
        private static string _pielImagePath = _imagesFolderPath + "Capas\\piel.png";
        private static string _ligamentoInterespinalImagePath = _imagesFolderPath + "Capas\\ligamento-interespinoso.png";
        private static string _duramadreImagePath = _imagesFolderPath + "Capas\\duramadre.png";
        private static string _l3ArribaImagePath = _imagesFolderPath + "Capas\\l3-arriba.png";
        private static string _l3AbajoImagePath = _imagesFolderPath + "Capas\\l3-abajo.png";
        private static string _l4ArribaImagePath = _imagesFolderPath + "Capas\\l4-arriba.png";
        private static string _l4AbajoImagePath = _imagesFolderPath + "Capas\\l4-abajo.png";

        //Colores
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

            _controller = gui;
        }

        #region Cambios en Manos
        public void MostrarCambioZE(CambioZEEventArgs e)
        {
            //ManoIzquierda
            ManosImageConfig config = GetNuevaConfiguracionImagenMano(e.ManoIzquierda.Track, e.ManoIzquierda.Estado, e.ManoIzquierda.VecesContamino);

            ImageSource nuevaImagen = new BitmapImage(
               new Uri(_imagesFolderPath +config.TrackingPath +config.EstadoPath, UriKind.Absolute));
            ManoIzquierda.Source = nuevaImagen;
            ManoIzqLabel.Background = config.LabelColor;
            ManoIzqLabel.Foreground = config.FontColor;
            ManoIzqLabel.Content = config.Texto;

            //ManoDerecha
            config = GetNuevaConfiguracionImagenMano(e.ManoDerecha.Track, e.ManoDerecha.Estado, e.ManoDerecha.VecesContamino);

            nuevaImagen = new BitmapImage(
               new Uri(_imagesFolderPath + config.TrackingPath + config.EstadoPath, UriKind.Absolute));
            ManoDerecha.Source = nuevaImagen;
            ManoDerLabel.Background = config.LabelColor;
            ManoDerLabel.Foreground = config.FontColor;
            ManoDerLabel.Content = config.Texto;

            //Alerta sonido
            if(e.ContaminadoAhora)
                SystemSounds.Exclamation.Play();

            SalidasZELabel.Content = String.Format("Se contamino la Zona Estéril {0} Veces", e.VecesContaminado);
        }

        internal void MostrarCambioSI(CambioSIEventArgs datosDelEvento)
        {
            throw new NotImplementedException();
        }

        public ManosImageConfig GetNuevaConfiguracionImagenMano(Mano.Tracking track, Mano.Estados estado, int nroIngreso)
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

        #region Cambio en Vertebras

        public void MostrarCambioVertebras(CambioSIEventArgs e)
        {
            UpdateCapaActualLabel(e);
        }

        private void UpdateCapaActualLabel(CambioSIEventArgs e)
        {
            //ATRAVIESA LA PIEL
            if (e.TejidoAdiposo.Estado == Capa.Estados.Atravesando || e.TejidoAdiposo.Estado == Capa.Estados.AtravesandoNuevamente)
            {
                TejidoAdiposoImage.Source = new BitmapImage(new Uri(_pielImagePath, UriKind.Absolute));
                CapaActualLabel.Content = "TEJIDO ADIPOSO";
            }
            else
            {
                TejidoAdiposoImage.Source = null;
                CapaActualLabel.Content = "NINGUNA";
            }

            //ROZA UNA VERTEBRA
            if(e.RozandoAhora)
                CapaActualLabel.Content = "LIGAMENTO INTERESPINOSO";

            if (e.L3.Estado == Vertebra.Estados.Rozando || e.L3.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                if((e.L3.Sector == VertebraL3.Sectores.Abajo))
                {
                    L3Image.Source = new BitmapImage(new Uri(_l3AbajoImagePath, UriKind.Absolute));
                    //vista front l3
                }
                else
                {
                    L3Image.Source = new BitmapImage(new Uri(_l3ArribaImagePath, UriKind.Absolute));
                    //vista front l3
                }
            }
            else {
                L3Image.Source = null;
                //vista front l3
            }


            if (e.L4.Estado == Vertebra.Estados.Rozando || e.L4.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                switch (e.L4.Sector)
                {
                    case VertebraL4.Sectores.Abajo:
                        L4Image.Source = new BitmapImage(new Uri(_l4AbajoImagePath, UriKind.Absolute));
                        //vista front l4
                        break;
                    case VertebraL4.Sectores.ArribaIzquierda:
                        L4Image.Source = new BitmapImage(new Uri(_l4ArribaImagePath, UriKind.Absolute));
                        //vista front l4
                        break;
                    case VertebraL4.Sectores.ArribaCentro:
                        L4Image.Source = new BitmapImage(new Uri(_l4ArribaImagePath, UriKind.Absolute));
                        //vista front l4
                        break;
                    case VertebraL4.Sectores.ArribaDerecha:
                        L4Image.Source = new BitmapImage(new Uri(_l4ArribaImagePath, UriKind.Absolute));
                        //vista front l4
                        break;
                }
            }
            else
            {
                L4Image.Source = null;
                //vista front l4
            }

            //ATRAVIESA LA DURAMADRE
            if (e.Duramadre.Estado == Capa.Estados.Atravesando || e.Duramadre.Estado == Capa.Estados.AtravesandoNuevamente)
            {
                CapaActualLabel.Content = "DURAMADRE";
                DuramadreImage.Source = new BitmapImage(new Uri(_duramadreImagePath, UriKind.Absolute));
            }
            else
                DuramadreImage.Source = null;


        }
        //private void UpdateImagenCapas(CambioSIEventArgs e)
        //{
        //    TejidoAdiposoImage.Source = (e.TejidoAdiposo.Estado == Capa.Estados.Atravesando || e.TejidoAdiposo.Estado == Capa.Estados.AtravesandoNuevamente) ? 
        //        new BitmapImage(new Uri(_pielImagePath, UriKind.Absolute)) : null;

        //    LigamentoInterespinosoImage.Source = (e.RozandoAhora) ?
        //        new BitmapImage(new Uri(_ligamentoInterespinalImagePath, UriKind.Absolute)) : null;

        //    L3Image.Source = (e.L3.Estado == Vertebra.Estados.Rozando || e.L3.Estado == Vertebra.Estados.RozandoNuevamente)?
        //        new BitmapImage(new Uri((e.L3.Sector == VertebraL3.Sectores.Abajo)? _l3AbajoImagePath : _l3ArribaImagePath, UriKind.Absolute)) : null;

        //    L4Image.Source = (e.L4.Estado == Vertebra.Estados.Rozando || e.L4.Estado == Vertebra.Estados.RozandoNuevamente) ?
        //        new BitmapImage(new Uri((e.L4.Sector == VertebraL4.Sectores.Abajo) ? _l4AbajoImagePath : _l4ArribaImagePath, UriKind.Absolute)) : null;

        //    DuramadreImage.Source = (e.Duramadre.Estado == Capa.Estados.Atravesando || e.Duramadre.Estado == Capa.Estados.AtravesandoNuevamente)? 
        //        new BitmapImage(new Uri(_duramadreImagePath, UriKind.Absolute)) : null;

        //}

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
