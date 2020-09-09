using System.Windows;
using System.Windows.Controls;


namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class PasosPreparacion : Page
    {
        public GUIController _controller { get; set; }

        public PasosPreparacion(GUIController gui)
        {
            InitializeComponent();
            _controller = gui;
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
