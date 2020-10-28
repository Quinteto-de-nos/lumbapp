using Accord;
using LumbApp.Enums;
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
    public partial class SimulacionModoGuiado : Page
    {
        public GUIController _controller { get; set; }

        //para alertas
        private DispatcherTimer timer;
        int timeLeft { get; set; }
        private CambioSIEventArgs cambiosSI;
        private CambioZEEventArgs cambiosZE;

        //Path General de Carpeta de Imagenes
        private static readonly string _imagesFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\GUI\\Imagenes\\";

        //Paths de manos
        private static readonly string _manoPerdidaSubPath = "Manos\\NoTraqueando\\";
        private static readonly string _manoTrackeadaSubPath = "Manos\\Traqueando\\";
        private static readonly string _manoFueraInicialPath = "mano-fuera-inicial.png";
        private static readonly string _manoFueraPath = "mano-fuera.png";
        private static readonly string _manoDentroPath = "mano-dentro.png";
        private static readonly string _manoContaminadaPath = "mano-contaminadaX.png";

        //Paths de capas
        private static readonly string _capasFrontPath = _imagesFolderPath + "Capas\\Frente\\";
        private static readonly string _capasSidePath = _imagesFolderPath + "Capas\\Costado\\";

        //Colores
        private static Brush[] coloresContaminando { get; set; }
        private BrushConverter brushConverter { get; set; }
        private Brush white { get; set; }
        private Brush black { get; set; }
        private Brush lightred { get; set; }
        private Brush lightyellow { get; set; }
        private Brush lightgreen { get; set; }
        private Brush darkred { get; set; }

        public SimulacionModoGuiado(GUIController gui)
        {
            InitializeComponent();

            //inicializo imagenes de manos
            ImageSource startHandsSource = new BitmapImage(
                new Uri(_imagesFolderPath + _manoPerdidaSubPath + _manoFueraInicialPath, UriKind.Absolute));

            ManoIzquierda.Source = startHandsSource;
            ManoDerecha.Source = startHandsSource;

            //inicializo imagenes de capas
            PielSideImage.Source = new BitmapImage(new Uri(_imagesFolderPath + "Capas\\Costado\\piel_OFF.png", UriKind.Absolute));
            SideBaseImage.Source = new BitmapImage(new Uri(_imagesFolderPath + "Capas\\Costado\\base.png", UriKind.Absolute));
            FrontBaseImage.Source = new BitmapImage(new Uri(_imagesFolderPath + "Capas\\Frente\\base.png", UriKind.Absolute));

            //inicializo colores
            brushConverter = new BrushConverter();
            white = (Brush)brushConverter.ConvertFrom("#ffffff");
            black = (Brush)brushConverter.ConvertFrom("#323232");
            lightgreen = (Brush)brushConverter.ConvertFrom("#a7f192");
            darkred = (Brush)brushConverter.ConvertFrom("#bc100c");
            lightred = (Brush)brushConverter.ConvertFrom("#ff6161");
            lightyellow = (Brush)brushConverter.ConvertFrom("#ecf192");
            string[] colores = { "#dcf192", "#f1ef92", "#f1d792", "#f1b392", "#f19292", "#f3817e", "#ff6161" };
            coloresContaminando = new Brush[colores.Count()];
            for (int i = 0; i < colores.Count(); i++)
            {
                coloresContaminando[i] = (Brush)brushConverter.ConvertFrom(colores[i]);
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
            this.cambiosZE = e;
            this.Dispatcher.Invoke(handlerZE);
        }
        private void handlerZE()
        {
            //ManoIzquierda
            var e = cambiosZE;
            ManosImageConfig config = GetNuevaConfiguracionImagenMano(e.ManoIzquierda.Track, e.ManoIzquierda.Estado, e.ManoIzquierda.VecesContamino);

            ImageSource nuevaImagen = new BitmapImage(
               new Uri(_imagesFolderPath + config.TrackingPath + config.EstadoPath, UriKind.Absolute));
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
            /*
            if (e.ContaminadoAhora)
                SystemSounds.Exclamation.Play();
            */
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
                    config.LabelColor = lightgreen;
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
                        config.LabelColor = coloresContaminando[nroContaminacion - 1];
                        config.FontColor = black;
                        config.Texto = "Contaminando";
                        break;
                    }
            }

            return config;
        }

        #endregion

        #region Cambio en Vertebras
        public void MostrarCambioSI(CambioSIEventArgs datos)
        {
            datos.MostrarCambios();
            cambiosSI = datos;
            this.Dispatcher.Invoke(handlerSI);
        }
        private void handlerSI()
        {
            //MostrarAlertas(cambiosSI);

            //ATRAVIESA LA PIEL
            if (cambiosSI.TejidoAdiposo.Estado == Capa.Estados.Atravesando || cambiosSI.TejidoAdiposo.Estado == Capa.Estados.AtravesandoNuevamente)
            {
                PielSideImage.Source = new BitmapImage(new Uri(_capasSidePath + "piel_ON.png", UriKind.Absolute));
                CapaActualLabel.Content = "TEJIDO ADIPOSO";
            }
            else
            {
                PielSideImage.Source = new BitmapImage(new Uri(_capasSidePath + "piel_OFF.png", UriKind.Absolute));
                CapaActualLabel.Content = "NINGUNA";
            }

            RozandoLabel.Content = "NINGUNA";
            RozandoBackgroundLabel.Background = white;
            //ROZANDO L2
            if (cambiosSI.L2.Estado == Vertebra.Estados.Rozando || cambiosSI.L2.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                RozandoLabel.Content = "L2";
                RozandoBackgroundLabel.Background = lightred;
                L2SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L2 adelante.png", UriKind.Absolute));
                L2SFrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L2 adelante.png", UriKind.Absolute));
            }
            else
            {
                L2SFrontImage.Source = null;
                L2SideImage.Source = null;
            }

            //ROZANDO L3
            if (cambiosSI.L3.Estado == Vertebra.Estados.Rozando || cambiosSI.L3.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                RozandoBackgroundLabel.Background = lightred;
                RozandoLabel.Content = "L3";
                L3FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath +
                    BaseEnumManager<VertebraL3.Sectores>.GetDisplayName(cambiosSI.L3.Sector) + ".png", UriKind.Absolute));
                L3SideImage.Source = new BitmapImage(new Uri(_capasSidePath +
                    BaseEnumManager<VertebraL3.Sectores>.GetDisplayName(cambiosSI.L3.Sector) + ".png", UriKind.Absolute));
            }
            else
            {
                L3FrontImage.Source = null;
                L3SideImage.Source = null;
            }

            //ROZANDO L4
            if (cambiosSI.L4.Estado == Vertebra.Estados.Rozando || cambiosSI.L4.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                L4SideImage.Source = new BitmapImage(new Uri(_capasSidePath +
                    BaseEnumManager<VertebraL3.Sectores>.GetDisplayName(cambiosSI.L4.Sector) + ".png", UriKind.Absolute));
                L4FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath +
                    BaseEnumManager<VertebraL3.Sectores>.GetDisplayName(cambiosSI.L4.Sector) + ".png", UriKind.Absolute));
                RozandoLabel.Content = "L4";

                if (cambiosSI.L4.Sector == VertebraL4.Sectores.ArribaCentro)
                    RozandoBackgroundLabel.Background = lightgreen;
                else
                {
                    if (cambiosSI.L4.Sector == VertebraL4.Sectores.Abajo)
                        RozandoBackgroundLabel.Background = lightred;
                    else
                        RozandoBackgroundLabel.Background = lightyellow;
                }
            }
            else
            {
                L4SideImage.Source = null;
                L4FrontImage.Source = null;
            }

            //ROZANDO L5
            if (cambiosSI.L5.Estado == Vertebra.Estados.Rozando || cambiosSI.L5.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                RozandoLabel.Content = "L5";
                RozandoBackgroundLabel.Background = lightred;
                L5SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L5 adelante.png", UriKind.Absolute));
                L5FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L5 adelante.png", UriKind.Absolute));
            }
            else
            {
                L5SideImage.Source = null;
                L5FrontImage.Source = null;
            }

            //ATRAVIESA LA DURAMADRE
            if (cambiosSI.Duramadre.Estado == Capa.Estados.Atravesando || cambiosSI.Duramadre.Estado == Capa.Estados.AtravesandoNuevamente)
            {
                CapaActualLabel.Content = "DURAMADRE";
                RozandoBackgroundLabel.Background = white;
                DuramadreSideImage.Source = new BitmapImage(new Uri(_capasSidePath + "duramadre.png", UriKind.Absolute));
                DuramadreFrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "duramadre.png", UriKind.Absolute));
            }
            else
            {
                DuramadreSideImage.Source = null;
                DuramadreFrontImage.Source = null;
            }
        }
        /*
        private void MostrarAlertas(CambioSIEventArgs e)
        {
            if ((e.L3RozandoAhora && e.L3.Sector == VertebraL3.Sectores.Arriba) ||
               (e.L4RozandoAhora && e.L4.Sector == VertebraL4.Sectores.Abajo) ||
                e.L2RozandoAhora || e.L5RozandoAhora)
            {
                SystemSounds.Exclamation.Play();
            }
            else if (e.L4RozandoAhora &&
                e.L4.Sector != VertebraL4.Sectores.Abajo &&
                e.L4.Sector != VertebraL4.Sectores.ArribaCentro)
            {
                SystemSounds.Exclamation.Play();
            }

        }
        */
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
