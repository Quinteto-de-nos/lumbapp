using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private void ArmarDiccionarioOrdenado (InformeSI informeSI, InformeZE informeZE, TimeSpan tiempoTotalDeEjecucion) {
            DatosPractica = new OrderedDictionary();
            

            DatosPractica.Add("Contaminaciones totales", Convert.ToString(informeZE.Zona));
            DatosPractica.Add("Contaminaciones por mano izquierda", Convert.ToString(informeZE.ManoIzquierda));
            DatosPractica.Add("Contaminaciones por mano derecha", Convert.ToString(informeZE.ManoDerecha));
            DatosPractica.Add("Punciones tejido adiposo", Convert.ToString(informeSI.TejidoAdiposo));
            DatosPractica.Add("Roces sobre L2", Convert.ToString(informeSI.L2));
            DatosPractica.Add("Roces sobre L3 arriba", Convert.ToString(informeSI.L3Arriba));
            DatosPractica.Add("Roces sobre L3 abajo", Convert.ToString(informeSI.L3Abajo));
            DatosPractica.Add("Roces sobre L4 arriba izquierda", Convert.ToString(informeSI.L4ArribaIzquierda));
            DatosPractica.Add("Roces sobre L4 arriba derecha", Convert.ToString(informeSI.L4ArribaDerecha));
            DatosPractica.Add("Roces sobre L4 arriba centro", Convert.ToString(informeSI.L4ArribaCentro));
            DatosPractica.Add("Roces sobre L4 abajo", Convert.ToString(informeSI.L4Abajo));
            DatosPractica.Add("Roces sobre L5", Convert.ToString(informeSI.L5));
            DatosPractica.Add("Punciones duramadre", Convert.ToString(informeSI.Duramadre));
            DatosPractica.Add("Caminos correctos", Convert.ToString(informeSI.CaminoCorrecto));
            DatosPractica.Add("Caminos incorrectos", Convert.ToString(informeSI.CaminoIncorrecto));
            DatosPractica.Add("Tiempo total", string.Format("{0:D2}:{1:D2}:{2:D2}",
                tiempoTotalDeEjecucion.Hours, tiempoTotalDeEjecucion.Minutes, tiempoTotalDeEjecucion.Seconds));
        }
    }
}
