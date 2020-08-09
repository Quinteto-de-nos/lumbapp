using LumbApp.Enums;
using LumbApp.GUI;
using LumbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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

        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(IngresoDatosPracticante), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(IngresoDatosPracticante), new PropertyMetadata(null));
        public string Text { get { return GetValue(TextProperty) as string; } set { SetValue(TextProperty, value); } }
        public string Description { get { return GetValue(DescriptionProperty) as string; } set { SetValue(DescriptionProperty, value); } }
        private Regex regexApYN = new Regex("^[a-zA-Z ]*$");


        public IngresoDatosPracticante(GUIController guiController)
        {
            InitializeComponent();
            _controller = guiController;
            IniciarSimulacion.IsEnabled = false;
        }

        public DatosPracticante ProcesarDatos()
        {
            DatosPracticante datosPracticante = new DatosPracticante();
            datosPracticante.Nombre = Nombre.Text;
            datosPracticante.Apellido = Apellido.Text;
            datosPracticante.Dni = Int32.Parse( Dni.Text );
            datosPracticante.FolderPath = FolderPath.Content.ToString();
            return datosPracticante;
        }

        private void IniciarSimulacion_Click(object sender, RoutedEventArgs e)
        {
            ModoSimulacion modoSeleccionado;
            DatosPracticante datosPracticante = ProcesarDatos();
            ComboBoxItem item = (ComboBoxItem)ComboModo.SelectedItem;
            if (item.Name == "ModoGuiado")
                modoSeleccionado = ModoSimulacion.ModoGuiado;
            else
                modoSeleccionado = ModoSimulacion.ModoEvaluacion;
            _controller.FinIngresoDatos(datosPracticante, modoSeleccionado);
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = Description;
                dlg.SelectedPath = Text;
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    FolderPath.Content = dlg.SelectedPath;
                    BindingExpression be = GetBindingExpression(TextProperty);
                    if (be != null)
                        be.UpdateSource();
                }
            }
        }

        #region Validaciones de Datos del Practicante

        private void ValidarDni(object sender, TextChangedEventArgs e)
        {

            IniciarSimulacion.IsEnabled = (DatosPracticanteValidos() ? true : false);

            int dni=0;

            try
            {
                if (((string)Dni.Text).Length > 0)
                {
                    dni = Int32.Parse((String)Dni.Text);
                }
                    
            }
            catch
            {
                ErrorDni.Content = "Ingrese un número válido entre 1000000 y 999999999";
            }

            if ((dni < 1000000) || (dni > 999999999))
            {
                ErrorDni.Content = "Ingrese un número válido entre 1000000 y 999999999";
            }
            else
            {
                ErrorDni.Content = "";
            }
        }

        private void ValidarApellido(object sender, TextChangedEventArgs e)
        {
            IniciarSimulacion.IsEnabled = (DatosPracticanteValidos() ? true : false);

            if (!regexApYN.IsMatch(Apellido.Text)) {
                ErrorApellido.Content = "Ingrese un Apellido válido";
            }
            else
            {
                ErrorApellido.Content = "";
            }
        }

        private void ValidarNombre(object sender, TextChangedEventArgs e)
        {
            IniciarSimulacion.IsEnabled = (DatosPracticanteValidos() ? true : false);

            if (!regexApYN.IsMatch(Nombre.Text))
            {
                ErrorNombre.Content = "Ingrese un Nombre válido";
            }
            else
            {
                ErrorNombre.Content = "";
            }
        }

        private void ValidarMail(object sender, TextChangedEventArgs e)
        {
            IniciarSimulacion.IsEnabled = (DatosPracticanteValidos() ? true : false);

            try
            {
                MailAddress m = new MailAddress(Mail.Text);

                ErrorMail.Content = "";
            }
            catch (FormatException)
            {
                ErrorMail.Content = "Ingrese un mail válido";
            }

        }

        private bool DatosPracticanteValidos()
        {
            if(Nombre.Text.Length > 0 && regexApYN.IsMatch(Nombre.Text) &&
                Apellido.Text.Length > 0 && regexApYN.IsMatch(Apellido.Text) && 
                Dni.Text.Length > 0)
            {
                try
                {
                    var dni = Int32.Parse((String)Dni.Text);
                    if ((dni >= 1000000) && (dni <= 999999999))
                    {
                        return true;
                    }
                }
                catch { return false; }

            }
            return false;
        }


        #endregion

    }
}
