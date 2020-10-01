using iTextSharp.text;
using iTextSharp.text.pdf;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.FinalFeedbacker_ {
    public class FinalFeedbacker : IFinalFeedbacker {
        private DatosPracticante _datosPracticante;
        private InformeZE _informeZE;
        private InformeSI _informeSI;
        private TimeSpan _tiempoTotal;
        private String _path;
        private DateTime _fecha;

        public FinalFeedbacker (String path, DatosPracticante datosPracticante, InformeZE infZE, InformeSI infSI, TimeSpan tiempoTotal, DateTime fecha) {
            if(path==null || path=="" || datosPracticante == null || infSI == null || infZE == null || tiempoTotal == null)
                throw new Exception("Los datos de entrada no peden ser nulos, los necesito para crear el informe en PDF.");

            this._path = path;
            this._datosPracticante = datosPracticante;
            this._informeZE = infZE;
            this._informeSI = infSI;
            this._tiempoTotal = tiempoTotal;
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

                // Escribimos el encabezamiento en el documento
                doc.Add(new Paragraph("INFORME FINAL DE LA PRÁCTICA"));
                doc.Add(Chunk.NEWLINE);

                #region Datos del alumno
                Font fuenteTitulos = new Font(Font.FontFamily.COURIER, 12, Font.BOLD, BaseColor.GREEN);
                //fuenteTitulos.SetColor(123, 421, 231);
                doc.Add(new Paragraph("Información del alumno", fuenteTitulos));

                // Creamos una tabla que contendrá los datos del alumno
                PdfPTable tblDatosAlumno = new PdfPTable(4);
                tblDatosAlumno.WidthPercentage = 100;

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

                // Llenamos la tabla con información
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

                doc.Add(tblDatosAlumno);
                #endregion

                doc.Add(Chunk.NEWLINE);

                #region Datos de la practica
                doc.Add(new Paragraph("Estadísticas", new Font(Font.FontFamily.COURIER,12, Font.BOLD, BaseColor.GREEN)));
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

                // Llenamos la tabla con información
                #region zona esteril
                columnaDescripcion = new PdfPCell(new Phrase("Contaminaciones Zona Estéril", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeZE.Zona.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region mano derecha
                columnaDescripcion = new PdfPCell(new Phrase("Contaminaciones Mano Derecha", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeZE.ManoDerecha.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region mano izquierda
                columnaDescripcion = new PdfPCell(new Phrase("Contaminaciones Mano Izquierda", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeZE.ManoIzquierda.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region tejido adiposo
                columnaDescripcion = new PdfPCell(new Phrase("Punciones Tejido Adiposo", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.TejidoAdiposo.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region L2
                columnaDescripcion = new PdfPCell(new Phrase("Roces L2", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L2.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region L3
                columnaDescripcion = new PdfPCell(new Phrase("Roces L3 Arriba", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L3Arriba.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);

                columnaDescripcion = new PdfPCell(new Phrase("Roces L3 Abajo", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L3Abajo.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region L4
                columnaDescripcion = new PdfPCell(new Phrase("Roces L4 Arriba Izquierda", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L4ArribaIzquierda.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);

                columnaDescripcion = new PdfPCell(new Phrase("Roces L4 Arriba Derecha", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L4ArribaDerecha.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);

                columnaDescripcion = new PdfPCell(new Phrase("Roces L4 Arriba Centro", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L4ArribaCentro.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);

                columnaDescripcion = new PdfPCell(new Phrase("Roces L4 Abajo", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L4Abajo.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region L5
                columnaDescripcion = new PdfPCell(new Phrase("Roces L5", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.L5.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region duramadre
                columnaDescripcion = new PdfPCell(new Phrase("Punciones Duramadre", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_informeSI.Duramadre.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

                #region tiempo total
                columnaDescripcion = new PdfPCell(new Phrase("Tiempo total", _standardFont));
                columnaDescripcion.BorderWidth = 0;

                columnaCantidad = new PdfPCell(new Phrase(_tiempoTotal.ToString(), _standardFont));
                columnaCantidad.BorderWidth = 0;

                // Añadimos las celdas a la tabla
                tblDatosPractica.AddCell(columnaDescripcion);
                tblDatosPractica.AddCell(columnaCantidad);
                #endregion

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
