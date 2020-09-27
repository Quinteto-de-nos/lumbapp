using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Models
{
    public class Informe
    {
        public int Dni { get; }
        public string Nombre { get; }
        public string Apellido { get; }
        public string FolderPath { get; }
        public TimeSpan TiempoTotalDeEjecucion { get; }
        public int Zona { get; }
        public int ManoIzquierda { get; }
        public int ManoDerecha { get; }
        public int TejidoAdiposo { get; }
        public int L2 { get; }
        public int L3Arriba { get; }
        public int L3Abajo { get; }
        public int L4ArribaIzquierda { get; }
        public int L4ArribaDerecha { get; }
        public int L4ArribaCentro { get; }
        public int L4Abajo { get; }
        public int L5 { get; }
        public int Duramadre { get; }
        public int caminoCorrecto { get; }
        public int caminoIncorrecto { get; }
        public bool PdfGenerado { get; }
        
        public Informe(string nombre, string apellido, int dni, string folderPath, TimeSpan tiempoTotalDeEjecucion, int zona, int manoIzquierda, int manoDerecha, int tejidoAdiposo, int l2, int l3Arriba, int l3Abajo, int l4ArribaIzquierda, int l4ArribaDerecha, int l4ArribaCentro, int l4Abajo, int l5, int duramadre, int caminoCorrecto, int caminoIncorrecto, bool pdfGenerado)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FolderPath = folderPath;
            TiempoTotalDeEjecucion = tiempoTotalDeEjecucion;
            Zona = zona;
            ManoIzquierda = manoIzquierda;
            ManoDerecha = manoDerecha;
            TejidoAdiposo = tejidoAdiposo;
            L2 = l2;
            L3Arriba = l3Arriba;
            L3Abajo = l3Abajo;
            L4ArribaIzquierda = l4ArribaIzquierda;
            L4ArribaDerecha = l4ArribaDerecha;
            L4ArribaCentro = l4ArribaCentro;
            L4Abajo = l4Abajo;
            L5 = l5;
            Duramadre = duramadre;
            this.caminoCorrecto = caminoCorrecto;
            this.caminoIncorrecto = caminoIncorrecto;
            PdfGenerado = pdfGenerado;
        }
    }
}
