using System.Windows;
using System.Windows.Controls;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class SensorsCheck : Page
    {
        public GUIController _controller { get; set; }
        static readonly string MensajeCheckeandoSensores = "Chequeando Sensores";

        public SensorsCheck(GUIController gui)
        {
            _controller = gui;
            InitializeComponent();
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            SpinerIcon.Visibility = Visibility.Visible;
            RetryButton.Visibility = Visibility.Hidden;
            ErrorIcon.Visibility = Visibility.Hidden;
            Mensaje.Content = MensajeCheckeandoSensores;
            _controller.CheckearSensores();
        }

        public void MostrarCheckeandoSensores()
        {
            SpinerIcon.Visibility = Visibility.Visible;
            RetryButton.Visibility = Visibility.Hidden;
            ErrorIcon.Visibility = Visibility.Hidden;
            Mensaje.Content = MensajeCheckeandoSensores;
        }

        public void MostrarErrorDeConexion(string mensaje)
        {
            RetryButton.Visibility = Visibility.Visible;
            ErrorIcon.Visibility = Visibility.Visible;
            SpinerIcon.Visibility = Visibility.Hidden;
            Mensaje.Content = mensaje;
        }
    }
}
