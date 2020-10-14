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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GUIController gui = new GUIController(this);
            gui.Inicializar();
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
