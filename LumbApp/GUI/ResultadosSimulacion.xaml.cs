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
        }

        private void NuevaSimulacion_Click(object sender, RoutedEventArgs e)
        {
            _controller.NuevaSimulacion();
        }

        public void MostrarResultados(Informe informe)
        {
            NombrePracticante.Content = 
                String.Format(
                    "{0}, {1}" + Environment.NewLine +
                    "Resultados de la práctica en {2}", informe.Apellido, informe.Nombre,informe.FolderPath);

            DniPracticante.Content = "DNI: " + informe.Dni.ToString();

            int halfPoint = informe.DatosPractica.Count / 2;
            int i = 1;

            foreach (DictionaryEntry dato in informe.DatosPractica)
            {
                if(i <= halfPoint)
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
        }
    }
}
