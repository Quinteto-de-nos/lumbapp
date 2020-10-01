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
        public TuplaDeStrings[] DatosPractica { get; }
        public bool PdfGenerado { get; }
        
        public Informe(string nombre, string apellido, int dni, string folderPath, TuplaDeStrings[] datosPractica, bool pdfGenerado)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            FolderPath = folderPath;
            DatosPractica = datosPractica;
            PdfGenerado = pdfGenerado;
        }
    }
}
