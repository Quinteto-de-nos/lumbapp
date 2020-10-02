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
            

            DatosPractica.Add("Contaminaciones Zona", Convert.ToString(informeZE.Zona));
            DatosPractica.Add("Contaminaciones Mano Izquierda", Convert.ToString(informeZE.ManoIzquierda));
            DatosPractica.Add("Contaminaciones Mano Derecha", Convert.ToString(informeZE.ManoDerecha));
            DatosPractica.Add("Punciones Tejido Adiposo", Convert.ToString(informeSI.TejidoAdiposo));
            DatosPractica.Add("Roces L2", Convert.ToString(informeSI.L2));
            DatosPractica.Add("Roces L3 Arriba", Convert.ToString(informeSI.L3Arriba));
            DatosPractica.Add("Roces L3 Abajo", Convert.ToString(informeSI.L3Abajo));
            DatosPractica.Add("Roces L4 Arriba Izquierda", Convert.ToString(informeSI.L4ArribaIzquierda));
            DatosPractica.Add("Roces L4 Arriba Derecha", Convert.ToString(informeSI.L4ArribaDerecha));
            DatosPractica.Add("Roces L4 Arriba Centro", Convert.ToString(informeSI.L4ArribaCentro));
            DatosPractica.Add("Roces L4 Abajo", Convert.ToString(informeSI.L4Abajo));
            DatosPractica.Add("Roces L5", Convert.ToString(informeSI.L5));
            DatosPractica.Add("Punciones Duramadre", Convert.ToString(informeSI.Duramadre));
            DatosPractica.Add("Camino Correcto", Convert.ToString(informeSI.CaminoCorrecto));
            DatosPractica.Add("Camino Incorrecto", Convert.ToString(informeSI.CaminoIncorrecto));
            DatosPractica.Add("Tiempo Total", (tiempoTotalDeEjecucion.Hours + ":" +
                tiempoTotalDeEjecucion.Minutes + ":" + tiempoTotalDeEjecucion.Seconds));
        }
    }
}
