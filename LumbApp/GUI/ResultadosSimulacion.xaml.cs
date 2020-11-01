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
            NuevaSimulacion.IsEnabled = false;
            CheckIcon.Visibility = Visibility.Hidden;
        }

        private void NuevaSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.NuevaSimulacion();
        }

        public void MostrarResultados(Informe informe)
        {
            DatosPracticante.Content =
                String.Format(
                    "{0}, {1}" + Environment.NewLine +
                    "DNI: {2}", informe.Apellido, informe.Nombre, informe.Dni);

            RutaResultados.Content = "     Guardando resultados en"
                + Environment.NewLine + informe.FolderPath;

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
            if (informe.DatosPractica.Count % 2 != 0)
            {
                ReporteItemTitulo2.Content += "  " + Environment.NewLine;
                ReporteItemValor2.Content += "  " + Environment.NewLine;
            }
        }

        public void ResultadosGuardados()
        {
            RutaResultados.Content = RutaResultados.Content.ToString()
                .Replace("Guardando resultados", "Resultados guardado");
            SpinerIcon.Visibility = Visibility.Hidden;
            CheckIcon.Visibility = Visibility.Visible;
            NuevaSimulacion.IsEnabled = true;
        }
    }
}
