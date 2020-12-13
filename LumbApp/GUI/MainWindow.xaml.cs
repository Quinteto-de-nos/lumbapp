using System;
using System.Windows;
using System.Windows.Navigation;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        GUIController _gui { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _gui = new GUIController(this);
            _gui.Inicializar();
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            _gui.Finalizar();
        }

        [STAThread]
        public static void Main()
        {
            MainWindow window = new MainWindow();
            Application app = new Application();
            app.Run(window);
        }
    }

}
