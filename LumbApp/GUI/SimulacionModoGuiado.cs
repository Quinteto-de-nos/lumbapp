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
        private static string _capasFrontPath = _imagesFolderPath + "Capas\\Frente\\";
        private static string _capasSidePath = _imagesFolderPath + "Capas\\Costado\\";

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

            //inicializo imagenes de capas
            PielSideImage.Source = new BitmapImage(new Uri(_imagesFolderPath + "Capas\\Costado\\piel_OFF.png", UriKind.Absolute));
            SideBaseImage.Source = new BitmapImage( new Uri(_imagesFolderPath + "Capas\\Costado\\base.png", UriKind.Absolute));
            FrontBaseImage.Source = new BitmapImage(new Uri(_imagesFolderPath + "Capas\\Frente\\base.png", UriKind.Absolute));

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

        public void MostrarCambioSI(CambioSIEventArgs e)
        {
            MostrarAlertas(e);
            EntradasCapasLabel.Content = String.Format("{0} Tejido Adiposo" + Environment.NewLine + "{1} Duramadre", e.TejidoAdiposo.VecesAtravesada, e.Duramadre.VecesAtravesada);

            //ATRAVIESA LA PIEL
            if (e.TejidoAdiposo.Estado == Capa.Estados.Atravesando || e.TejidoAdiposo.Estado == Capa.Estados.AtravesandoNuevamente)
            {
                PielSideImage.Source = new BitmapImage(new Uri( _capasSidePath + "piel_ON.png", UriKind.Absolute));
                CapaActualLabel.Content = "TEJIDO ADIPOSO";
            }
            else
            {
                PielSideImage.Source = new BitmapImage(new Uri( _capasSidePath + "piel_OFF.png", UriKind.Absolute));
                CapaActualLabel.Content = "NINGUNA";
            }

            //ROZA UNA VERTEBRA
            if (e.L2.Estado == Vertebra.Estados.Rozando || e.L2.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                CapaActualLabel.Content = "LIGAMENTO INTERESPINOSO";
                RozandoLabel.Content = "L2";
                L2SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L2 adelante.png", UriKind.Absolute)); 
                L2SFrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L2 adelante.png", UriKind.Absolute));
            }

            if (e.L3.Estado == Vertebra.Estados.Rozando || e.L3.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                CapaActualLabel.Content = "LIGAMENTO INTERESPINOSO";
                if ((e.L3.Sector == VertebraL3.Sectores.Abajo))
                {
                    RozandoLabel.Content = "Parte Inferior de L3";
                    L3FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L3 abajo.png", UriKind.Absolute));
                    L3SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L3 abajo.png", UriKind.Absolute));
                }
                else
                {
                    RozandoLabel.Content = "Parte Inferior de L3";
                    L3FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L3 arriba.png", UriKind.Absolute));
                    L3SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L3 arriba.png", UriKind.Absolute));
                }
            }
            else 
            {
                L3FrontImage.Source = null;
                L3SideImage.Source = null;
            }


            if (e.L4.Estado == Vertebra.Estados.Rozando || e.L4.Estado == Vertebra.Estados.RozandoNuevamente)
            {
                CapaActualLabel.Content = "LIGAMENTO INTERESPINOSO";

                switch (e.L4.Sector)
                {
                    case VertebraL4.Sectores.Abajo:
                        RozandoLabel.Content = "Parte Inferior de L4";
                        L4SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L4 abajo.png", UriKind.Absolute));
                        L4FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L4 abajo.png", UriKind.Absolute));
                        break;
                    case VertebraL4.Sectores.ArribaIzquierda:
                        RozandoLabel.Content = "Parte Arriba Izquierda de L4";
                        L4SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L4 arriba izquierda.png", UriKind.Absolute));
                        L4FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L4 arriba izquierda.png", UriKind.Absolute));
                        break;
                    case VertebraL4.Sectores.ArribaCentro:
                        RozandoLabel.Content = "Parte Arriba Centro de L4";
                        L4SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L4 arriba centro.png", UriKind.Absolute));
                        L4FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L4 arriba centro.png", UriKind.Absolute));
                        break;
                    case VertebraL4.Sectores.ArribaDerecha:
                        RozandoLabel.Content = "Parte Arriba Derecha de L4";
                        L4SideImage.Source = new BitmapImage(new Uri(_capasSidePath + "L4 arriba derecha.png", UriKind.Absolute));
                        L4FrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "L4 arriba derecha.png", UriKind.Absolute));
                        break;
                }
            }
            else
            {
                L4SideImage.Source = null;
                L4FrontImage.Source = null;
            }

            //ATRAVIESA LA DURAMADRE
            if (e.Duramadre.Estado == Capa.Estados.Atravesando || e.Duramadre.Estado == Capa.Estados.AtravesandoNuevamente)
            {
                CapaActualLabel.Content = "DURAMADRE";
                DuramadreSideImage.Source = new BitmapImage(new Uri(_capasSidePath + "duramadre.png", UriKind.Absolute));
                DuramadreFrontImage.Source = new BitmapImage(new Uri(_capasFrontPath + "duramadre.png", UriKind.Absolute));
            }
            else
            {
                DuramadreSideImage.Source = null;
                DuramadreFrontImage.Source = null;
            }
        }

        private void MostrarAlertas(CambioSIEventArgs e)
        {
            if((e.L3RozandoAhora && e.L3.Sector == VertebraL3.Sectores.Arriba) ||
               (e.L4RozandoAhora && e.L4.Sector == VertebraL4.Sectores.Abajo) ||
                e.L2RozandoAhora || e.L5RozandoAhora)
            {
                AlertaLabel.Content = "Hey!! mira donde estas pinchando!";
                AlertaLabelBorder.Opacity=0.7;
                SystemSounds.Exclamation.Play();
                StartAlertTimer();
            }
            else if (e.L4RozandoAhora && 
                e.L4.Sector != VertebraL4.Sectores.Abajo && 
                e.L4.Sector != VertebraL4.Sectores.ArribaCentro)
            {
                AlertaLabel.Content = "Casi casi!";
                AlertaLabelBorder.Opacity = 0.7;
                SystemSounds.Exclamation.Play();
                StartAlertTimer();
            }

        }

        private void StartAlertTimer()
        {
            timeLeft = 5;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += alertTimerTick;
            timer.Start();
        }

        private void StopAlertTimer()
        {
            AlertaLabel.Content = "";
            AlertaLabelBorder.Opacity = 0;
            timer.Stop();
        }

        private void alertTimerTick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
                timeLeft -= 1;
            else
            {
                StopAlertTimer();
            }
        }
        #endregion

        #region Finalizar Simulacion
        private void FinalizarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            StopAlertTimer();
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
