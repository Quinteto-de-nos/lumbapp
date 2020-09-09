using System.Windows;
using System.Windows.Controls;


namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class ResultadosSimulacion : Page
    {
        public GUIController _controller { get; set; }

        public ResultadosSimulacion(GUIController gui)
        {
            InitializeComponent();
            _controller = gui;
        }

        private void IniciarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.SolicitarDatosPracticante();
        }

    }
}
