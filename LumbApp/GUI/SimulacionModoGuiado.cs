using LumbApp;
using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Orquestador;
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
    /// Interaction logic for SimulacionModoGuiado.xaml
    /// </summary>
    public partial class SimulacionModoGuiado : Page
    {
        public GUIController _controller { get; set; }

        public SimulacionModoGuiado(GUIController gui)
        {
            InitializeComponent();
            ImageViewer1.Source = new BitmapImage(new Uri("Imagenes/zona actual.png", UriKind.Relative));
            _controller = gui;
        }

    }
}
