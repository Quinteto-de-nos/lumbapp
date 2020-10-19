using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using System;
using System.Collections.Specialized;

namespace LumbApp.Models
{
    public class Informe
    {
        public int Dni { get; private set; }
        public string Nombre { get; private set; }
        public string Apellido { get; private set; }
        public string FolderPath { get; private set; }
        public OrderedDictionary DatosPractica { get; private set; }
        public bool PdfGenerado { get; private set; }

        public Informe(string nombre, string apellido, int dni, string folderPath, InformeSI informeSI,
            InformeZE informeZE, TimeSpan tiempoTotalDeEjecucion)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FolderPath = folderPath;

            ArmarDiccionarioOrdenado(informeSI, informeZE, tiempoTotalDeEjecucion);
        }

        public void SetPdfGenerado(bool pdfGenerado) { PdfGenerado = pdfGenerado; }

        private void ArmarDiccionarioOrdenado(InformeSI informeSI, InformeZE informeZE, TimeSpan tiempoTotalDeEjecucion)
        {
            DatosPractica = new OrderedDictionary
            {
                { "Contaminaciones totales", Convert.ToString(informeZE.Zona) },
                { "Contaminaciones por mano izquierda", Convert.ToString(informeZE.ManoIzquierda) },
                { "Contaminaciones por mano derecha", Convert.ToString(informeZE.ManoDerecha) },
                { "Punciones tejido adiposo", Convert.ToString(informeSI.TejidoAdiposo) },
                { "Roces L2", Convert.ToString(informeSI.L2) },
                { "Roces L3 arriba", Convert.ToString(informeSI.L3Arriba) },
                { "Roces L3 abajo", Convert.ToString(informeSI.L3Abajo) },
                { "Roces L4 arriba izquierda", Convert.ToString(informeSI.L4ArribaIzquierda) },
                { "Roces L4 arriba derecha", Convert.ToString(informeSI.L4ArribaDerecha) },
                { "Roces L4 arriba centro", Convert.ToString(informeSI.L4ArribaCentro) },
                { "Roces L4 abajo", Convert.ToString(informeSI.L4Abajo) },
                { "Roces L5", Convert.ToString(informeSI.L5) },
                { "Punciones duramadre", Convert.ToString(informeSI.Duramadre) },
                { "Caminos correctos", Convert.ToString(informeSI.CaminoCorrecto) },
                { "Caminos incorrectos", Convert.ToString(informeSI.CaminoIncorrecto) },
                {
                    "Tiempo total",
                    string.Format("{0:D2}:{1:D2}:{2:D2}",
                tiempoTotalDeEjecucion.Hours, tiempoTotalDeEjecucion.Minutes, tiempoTotalDeEjecucion.Seconds)
                }
            };
        }
    }
}
