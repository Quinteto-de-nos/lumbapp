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


namespace GUI
{
    /// <summary>
    /// Interaction logic for SensorsCheck.xaml
    /// </summary>
    public partial class SensorsCheck : Page
    {
        public GUIController controller { get; set; }

        public SensorsCheck()
        {
            InitializeComponent();
            retryButton.Visibility = Visibility.Hidden;
        }
        public SensorsCheck(GUIController _gui)
        {
            InitializeComponent();
            controller = _gui;
            //retryButton.Visibility = Visibility.Hidden;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //CheckSensors();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void retryButton_Click(object sender, RoutedEventArgs e)
        {
            controller.ReintentarConectarSensores();
        }

        //public void CheckSensors()
        //{
        //try { 
        //ListBoxItem itm = new ListBoxItem();
        //itm.Content = "Iniciando Kinect";
        //checkedSensors.Items.Add(itm);

        //conn = new ConectorKinect();
        //expert = new ExpertoZE(conn);
        //expert.Inicializar();

        //ListBoxItem itm2 = new ListBoxItem();
        //itm2.Content = "Chequeando Capa1";
        //checkedSensors.Items.Add(itm2);

        ////await Task.Delay(2000);

        //ListBoxItem itm3 = new ListBoxItem();
        //itm3.Content = "Chequeando Capa2";
        //checkedSensors.Items.Add(itm3);

        ////await Task.Delay(2000);
        //}
        //catch(Exception ex) {

        //    ListBoxItem error = new ListBoxItem();
        //    error.Content = "Error: " + ex;
        //    checkedSensors.Items.Add(error);

        //    retryButton.Visibility = Visibility.Visible;
        //}
        //}
    }
}
