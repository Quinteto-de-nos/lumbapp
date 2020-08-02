using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        //public SensorsCheck SensorsCheckPage { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //var navWindow = Window.GetWindow(this) as NavigationWindow;
            //if (navWindow != null) navWindow.ShowsNavigationUI = false;
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
