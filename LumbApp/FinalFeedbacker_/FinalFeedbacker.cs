using iTextSharp.text;
using iTextSharp.text.pdf;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LumbApp.FinalFeedbacker_ {
    public class FinalFeedbacker : IFinalFeedbacker {
        private DatosPracticante _datosPracticante;
        private OrderedDictionary _datosPractica;
        private String _path;
        private DateTime _fecha;

        public FinalFeedbacker (String path, DatosPracticante datosPracticante,
            OrderedDictionary datosPractica, DateTime fecha) {

            if(path==null || path=="" || datosPracticante == null || datosPractica == null || fecha == null)
                throw new Exception("Los datos de entrada no peden ser nulos, los necesito para crear el informe en PDF.");

            this._path = path;
            this._datosPracticante = datosPracticante;
            this._datosPractica = datosPractica;
            this._fecha = fecha;
        }

        public bool GenerarPDF () {
            try {
                Document doc = new Document(PageSize.A4);
                // Indicamos donde vamos a guardar el documento
                PdfWriter writer = PdfWriter.GetInstance(doc,
                                            new FileStream(_path, FileMode.Create));

                // Le colocamos el título y el autor
                // **Nota: Esto no será visible en el documento
                doc.AddTitle("Informe Final");
                doc.AddCreator("LumbApp");

                // Abrimos el archivo
                doc.Open();

                Font _standardFont = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);
                
                Font fuenteTitulos = new Font(Font.FontFamily.COURIER, 12, Font.BOLD, BaseColor.GREEN);
                fuenteTitulos.SetColor(0, 126, 93);
                BaseColor backgroundTitulos = new BaseColor(186, 232, 210);

                // Escribimos el encabezamiento en el documento
                doc.Add(new Paragraph("INFORME FINAL DE LA PRÁCTICA"));
                doc.Add(Chunk.NEWLINE);

                #region Datos del alumno
                Chunk chInfoAlumno = new Chunk("Información del alumno", fuenteTitulos);
                chInfoAlumno.SetBackground(backgroundTitulos);
                doc.Add(new Paragraph(chInfoAlumno));

                // Creamos una tabla que contendrá los datos del alumno
                PdfPTable tblDatosAlumno = new PdfPTable(4);
                tblDatosAlumno.WidthPercentage = 100;

                #region Headers
                // Configuramos el título de las columnas de la tabla
                PdfPCell columnaNombre = new PdfPCell(new Phrase("Nombre del alumno", _standardFont));
                columnaNombre.BorderWidth = 0;
                columnaNombre.BorderWidthBottom = 0.75f;

                PdfPCell columnaDNI = new PdfPCell(new Phrase("DNI del alumno", _standardFont));
                columnaDNI.BorderWidth = 0;
                columnaDNI.BorderWidthBottom = 0.75f;

                PdfPCell columnaEmail = new PdfPCell(new Phrase("E-mail", _standardFont));
                columnaEmail.BorderWidth = 0;
                columnaEmail.BorderWidthBottom = 0.75f;

                PdfPCell columnaFecha = new PdfPCell(new Phrase("Fecha", _standardFont));
                columnaFecha.BorderWidth = 0;
                columnaFecha.BorderWidthBottom = 0.75f;

                // Añadimos las celdas a la tabla
                tblDatosAlumno.AddCell(columnaNombre);
                tblDatosAlumno.AddCell(columnaDNI);
                tblDatosAlumno.AddCell(columnaEmail);
                tblDatosAlumno.AddCell(columnaFecha);
                #endregion

                #region Llenamos la tabla con información
                columnaNombre = new PdfPCell(new Phrase((_datosPracticante.Nombre + " " + _datosPracticante.Apellido),
                    _standardFont));
                columnaNombre.BorderWidth = 0;

                columnaDNI = new PdfPCell(new Phrase(_datosPracticante.Dni.ToString(), _standardFont));
                columnaDNI.BorderWidth = 0;

                columnaEmail = new PdfPCell(new Phrase(_datosPracticante.Email, _standardFont));
                columnaEmail.BorderWidth = 0;

                string fechaString = (_fecha.Year.ToString() + "/" + _fecha.Month.ToString() +
                    "/" + _fecha.Day.ToString() + " " + _fecha.Hour.ToString() + ":" + _fecha.Minute.ToString());

                columnaFecha = new PdfPCell(new Phrase(fechaString, _standardFont));
                columnaFecha.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosAlumno.AddCell(columnaNombre);
                tblDatosAlumno.AddCell(columnaDNI);
                tblDatosAlumno.AddCell(columnaEmail);
                tblDatosAlumno.AddCell(columnaFecha);
                #endregion

                doc.Add(tblDatosAlumno);
                #endregion

                doc.Add(Chunk.NEWLINE);

                #region Datos de la practica
                Chunk chEstadisticas = new Chunk("Información de la práctica", fuenteTitulos);
                chEstadisticas.SetBackground(backgroundTitulos);
                doc.Add(new Paragraph(chEstadisticas));
                
                // Creamos una tabla que contendrá los datos de la practica
                PdfPTable tblDatosPractica = new PdfPTable(2);
                tblDatosPractica.WidthPercentage = 100;

                // Configuramos el título de las columnas de la tabla
                PdfPCell columnaDescripcion = new PdfPCell(new Phrase("Descripción", _standardFont));
                columnaDescripcion.BorderWidth = 0;
                columnaDescripcion.BorderWidthBottom = 0.75f;

                PdfPCell columnaCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
                columnaCantidad.BorderWidth = 0;
                columnaCantidad.BorderWidthBottom = 0.75f;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);

                foreach(DictionaryEntry de in _datosPractica)
                {
                    columnaDescripcion = new PdfPCell(new Phrase(de.Key.ToString(), _standardFont));
                    columnaDescripcion.BorderWidth = 0;

                    columnaCantidad = new PdfPCell(new Phrase(de.Value.ToString(), _standardFont));
                    columnaCantidad.BorderWidth = 0;

                    // Añadimos las celdas a la tabla
                    tblDatosPractica.AddCell(columnaDescripcion);
                    tblDatosPractica.AddCell(columnaCantidad);
                }

                doc.Add(tblDatosPractica);

                #endregion

                doc.Close();
                writer.Close();

                return true;
            } catch (Exception ex){
                Debug.WriteLine(ex);
                return false;
            }
            
        }
    }
}
