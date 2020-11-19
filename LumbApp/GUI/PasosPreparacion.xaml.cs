using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class PasosPreparacion : Page
    {
        public GUIController _controller { get; set; }
        private static readonly string _imagesFolderPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "\\GUI\\Imagenes\\Preparacion\\";
        private readonly DispatcherTimer timer;
        private int timeLeft { get; set; }

        public PasosPreparacion(GUIController gui)
        {
            InitializeComponent();
            _controller = gui;
            timeLeft = 30;
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += timer_Tick;
            timer.Start();

            string uri = "pack://application:,,,/LumbApp Desktop;component/GUI/Imagenes/Preparacion/";
            //seteo imagenes
            HandWashingImage.Source = new BitmapImage(new Uri(@uri + "handwashing.png")); 
            CapImage.Source = new BitmapImage(new Uri(@uri + "cap-facemask-eye-protection.png"));
            AlcoholGelImage.Source = new BitmapImage(new Uri(@uri + "alcohol-gel.png"));
            GownImage.Source = new BitmapImage(new Uri(@uri + "gown.png"));
            RubberGlovesImage.Source = new BitmapImage(new Uri(@uri + "rubber-gloves.png"));
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft -= 1;
                LabelTimer.Content = timeLeft;
            }
            else
            {
                timer.Stop();
                SystemSounds.Exclamation.Play();
                _controller.FinPreparacion();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //iniciar timer
        }

        private void SaltearButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            _controller.FinPreparacion();
        }

    }
}
