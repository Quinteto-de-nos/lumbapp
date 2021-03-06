﻿using LumbApp.Enums;
using LumbApp.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
namespace LumbApp.GUI
{
    /// <summary>
    /// Interaction logic for UserDataInput.xaml
    /// </summary>
    public partial class IngresoDatosPracticante : Page
    {
        public GUIController _controller { get; set; }

        public static DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(IngresoDatosPracticante),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

        public static DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(IngresoDatosPracticante),
            new PropertyMetadata(null)
        );

        public string Text
        {
            get { return GetValue(TextProperty) as string; }
            set { SetValue(TextProperty, value); }
        }

        public string Description
        {
            get { return GetValue(DescriptionProperty) as string; }
            set { SetValue(DescriptionProperty, value); }
        }

        private readonly Regex regexApYN = new Regex("^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$");
        //@ escapa toda la string que sigue, no necesitamos \\ para escribir una \
        //Este mail acepta letras, numeros, -, _ y . en el nombre
        //Acepta letras, numeros, _ y . en el dominio
        //Debe terminar con . y 2 o 3 letras
        private readonly Regex regexMail = new Regex(@"^[\w\.\-]+@[\w\.]+\.\w{2,3}$");
        private bool dniValido;
        private bool nombreValido;
        private bool apellidoValido;
        private bool mailValido;

        public IngresoDatosPracticante(GUIController guiController, DatosPracticante datosPrevios)
        {
            InitializeComponent();
            _controller = guiController;
            IniciarSimulacion.IsEnabled = false;
            mailValido = true;

            Nombre.Text = datosPrevios.Nombre;
            Apellido.Text = datosPrevios.Apellido;
            if(datosPrevios.Dni != 0)
                Dni.Text = datosPrevios.Dni.ToString();
            Mail.Text = datosPrevios.Email;
            FolderPath.Content = datosPrevios.FolderPath;
        }

        /// <summary>
        /// Calcula y devuelve los datos del practicante
        /// </summary>
        /// <returns></returns>
        public DatosPracticante ProcesarDatos()
        {
            DatosPracticante datosPracticante = new DatosPracticante
            {
                Nombre = Nombre.Text,
                Apellido = Apellido.Text,
                Dni = Int32.Parse(Dni.Text),
                FolderPath = FolderPath.Content.ToString(),
                Email = Mail.Text
            };
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
                if (result == DialogResult.OK)
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
            const int min = 1000000;
            const int max = 999999999;
            string errMsg = "Ingrese un número válido entre " + min + " y " + max;
            int dni = 0;

            try
            {
                if (Dni.Text.Length > 0)
                    dni = Int32.Parse((String)Dni.Text);
            }
            catch
            {
                dniValido = false;
                ErrorDni.Content = errMsg;
            }

            if (dni < min || dni > max)
            {
                dniValido = false;
                ErrorDni.Content = errMsg;
            }
            else
            {
                dniValido = true;
                ErrorDni.Content = "";
            }

            IniciarSimulacion.IsEnabled = DatosPracticanteValidos();
        }

        private void ValidarApellido(object sender, TextChangedEventArgs e)
        {
            if (Apellido.Text.Length > 0 && regexApYN.IsMatch(Apellido.Text))
            {
                apellidoValido = true;
                ErrorApellido.Content = "";
            }
            else
            {
                apellidoValido = false;
                ErrorApellido.Content = "Ingrese un apellido válido";
            }
            IniciarSimulacion.IsEnabled = DatosPracticanteValidos();
        }

        private void ValidarNombre(object sender, TextChangedEventArgs e)
        {
            if (Nombre.Text.Length > 0 && regexApYN.IsMatch(Nombre.Text))
            {
                nombreValido = true;
                ErrorNombre.Content = "";
            }
            else
            {
                nombreValido = false;
                ErrorNombre.Content = "Ingrese un Nombre válido";
            }
            IniciarSimulacion.IsEnabled = DatosPracticanteValidos();
        }

        private void ValidarMail(object sender, TextChangedEventArgs e)
        {
            if (Mail.Text.Length == 0 || (Mail.Text.Length > 0 && regexMail.IsMatch(Mail.Text)))
            {
                mailValido = true;
                ErrorMail.Content = "";
            }
            else
            {
                mailValido = false;
                ErrorMail.Content = "Ingrese un mail válido";
            }
            IniciarSimulacion.IsEnabled = DatosPracticanteValidos();
        }

        private bool DatosPracticanteValidos()
        {
            return nombreValido && apellidoValido && dniValido && mailValido;
        }
        #endregion
    }
}
