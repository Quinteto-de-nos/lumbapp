using LumbApp.Models;
using System;
using System.Collections;
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
            DatosPracticaBorder.Visibility = Visibility.Hidden;
            DatosPracticaLabel.Visibility = Visibility.Hidden;
            DatosPracticanteBorder.Visibility = Visibility.Hidden;
            DatosPracticanteLabel.Visibility = Visibility.Hidden;
            SpinerIcon.Visibility = Visibility.Visible;
            TextoEspera.Visibility = Visibility.Visible;
            BorderEspera.Visibility = Visibility.Visible;
            NombrePracticante.Content = "";
            DniPracticante.Content = "";
            ReporteItemTitulo1.Content = "";
            ReporteItemTitulo2.Content = "";
            ReporteItemValor1.Content = "";
            ReporteItemValor2.Content = "";
        }

        private void NuevaSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.NuevaSimulacion();
        }

        public void MostrarResultados(Informe informe)
        {
            DatosPracticaBorder.Visibility = Visibility.Visible;
            DatosPracticaLabel.Visibility = Visibility.Visible;
            DatosPracticanteBorder.Visibility = Visibility.Visible;
            DatosPracticanteLabel.Visibility = Visibility.Visible;
            SpinerIcon.Visibility = Visibility.Hidden;
            TextoEspera.Visibility = Visibility.Hidden;
            BorderEspera.Visibility = Visibility.Hidden;

            NombrePracticante.Content =
                String.Format(
                    "{0}, {1}" + Environment.NewLine +
                    "Resultados de la práctica en {2}", informe.Apellido, informe.Nombre,informe.FolderPath);

            DniPracticante.Content = "DNI: " + informe.Dni.ToString();

            var halfPoint = Math.Ceiling(informe.DatosPractica.Count / 2.0);
            int i = 1;

            foreach (DictionaryEntry dato in informe.DatosPractica)
            {
                if (i <= halfPoint)
                {
                    ReporteItemTitulo1.Content += String.Format("{0}" + Environment.NewLine, dato.Key);
                    ReporteItemValor1.Content += String.Format("{0}" + Environment.NewLine, dato.Value);
                }
                else
                {
                    ReporteItemTitulo2.Content += String.Format("{0}" + Environment.NewLine, dato.Key);
                    ReporteItemValor2.Content += String.Format("{0}" + Environment.NewLine, dato.Value);
                }
                i++;
            }
            if(informe.DatosPractica.Count % 2 != 0)
            {
                ReporteItemTitulo2.Content += "  " + Environment.NewLine;
                ReporteItemValor2.Content += "  " + Environment.NewLine;
            }
        }

        public void ResultadosGuardados()
        {
            throw new NotImplementedException();
        }
    }
}
