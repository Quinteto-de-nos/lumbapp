using LumbApp.GUI;
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
    /// Interaction logic for UserDataInput.xaml
    /// </summary>
    public partial class IngresoDatosPracticante : Page
    {
        public GUIController _controller { get; set; }

        public IngresoDatosPracticante(GUIController guiController)
        {
            InitializeComponent();
            _controller = guiController;
        }

        public DatosPracticante ProcesarDatos()
        {
            DatosPracticante datosPracticante = new DatosPracticante();
            datosPracticante.Nombre = Nombre.Text;
            datosPracticante.Apellido = Apellido.Text;
            return datosPracticante;
        }

        private void IniciarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            DatosPracticante datosPracticante = ProcesarDatos();
            _controller.IniciarSimulacion(datosPracticante);
        }
    }
}
