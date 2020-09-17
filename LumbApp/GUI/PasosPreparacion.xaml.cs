using System;
using System.Media;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class PasosPreparacion : Page
    {
        public GUIController _controller { get; set; }
        private DispatcherTimer timer;
        private int timeLeft { get; set; }

        public PasosPreparacion(GUIController gui)
        {
            InitializeComponent();
            _controller = gui;
            timeLeft = 30;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();

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
            _controller.FinPreparacion();
        }

    }
}
