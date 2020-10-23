using System;
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
            MostrarCheckeandoSensores();
            _controller.CheckearSensores();
        }

        public void MostrarCheckeandoSensores()
        {
            SpinerIcon.Visibility = Visibility.Visible;
            //escondo el boton y icono de error
            RetryButton.Visibility = Visibility.Hidden;
            ErrorIcon.Visibility = Visibility.Hidden;

            LabelMensaje.SetValue(Grid.RowSpanProperty, 2);

            Mensaje.Text = MensajeCheckeandoSensores;
        }

        public void MostrarErrorDeConexion(string mensaje)
        {
            //muestro el boton de Reintentar y el icono de error
            RetryButton.Visibility = Visibility.Visible;
            ErrorIcon.Visibility = Visibility.Visible;
            //escondo el spinner
            SpinerIcon.Visibility = Visibility.Hidden;

            LabelMensaje.SetValue(Grid.RowSpanProperty, 1);

            mensaje = mensaje.Replace(". ", "." + Environment.NewLine);
            Mensaje.Text = mensaje;
        }
    }
}
