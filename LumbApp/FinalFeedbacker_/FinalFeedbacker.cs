using iTextSharp.text;
using iTextSharp.text.pdf;
using LumbApp.Models;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;

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

                #region Fuentes
                Font _fuenteEstandar = new Font(FontFactory.GetFont("Century Gothic").BaseFont, 11, 
                    Font.NORMAL, BaseColor.BLACK);
                
                Font _fuenteSubtitulos = new Font(FontFactory.GetFont("Century Gothic").BaseFont, 12, 
                    Font.BOLD, BaseColor.GREEN);
                _fuenteSubtitulos.SetColor(0, 126, 93);
                BaseColor backgroundTitulos = new BaseColor(186, 232, 210);

                Font _fuenteTitulo = new Font(FontFactory.GetFont("Century Gothic").BaseFont, 18, Font.BOLD, 
                    BaseColor.GREEN);
                _fuenteTitulo.SetColor(0, 126, 93);

                Font _fuenteCaractAlumno = new Font(FontFactory.GetFont("Century Gothic").BaseFont, 11, 
                    Font.BOLD, BaseColor.GREEN);
                _fuenteCaractAlumno.SetColor(0, 126, 93);
                #endregion

                //Agragamos el logo de LumbApp
                // Creamos la imagen y le ajustamos el tamaño
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..");
                path = Path.Combine(path, @"..\Images\logo.png");
                
                Image logo = Image.GetInstance(path);
                logo.ScaleToFit(100, 100F);
                logo.SetAbsolutePosition(500,750);
                //logo.Alignment = Element.ALIGN_RIGHT;
                doc.Add(logo);

                // Escribimos el encabezamiento en el documento
                Paragraph titulo = new Paragraph("INFORME FINAL DE LA PRÁCTICA", _fuenteTitulo);
                titulo.SpacingAfter = 20f;
                doc.Add(titulo);

                #region Datos del alumno
                Chunk chInfoAlumno = new Chunk("Información del alumno", _fuenteSubtitulos);
                chInfoAlumno.SetBackground(backgroundTitulos);
                doc.Add(new Paragraph(chInfoAlumno));

                // Creamos una tabla que contendrá los datos del alumno
                PdfPTable tblDatosAlumno = new PdfPTable(4);
                tblDatosAlumno.WidthPercentage = 100;
                tblDatosAlumno.DefaultCell.Border = Rectangle.NO_BORDER;
                tblDatosAlumno.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                tblDatosAlumno.DefaultCell.MinimumHeight = 50f;
                tblDatosAlumno.SpacingBefore = 10f;
                tblDatosAlumno.SpacingAfter = 10f;

                #region Nombre del Alumno
                // Configuramos el título de las columnas de la tabla
                PdfPCell columna1 = new PdfPCell(new Phrase("Nombre del\nalumno", _fuenteCaractAlumno));
                columna1.Border = Rectangle.NO_BORDER;
                //columna1.BorderWidthBottom = 0.75f;
                columna1.BorderWidthTop = 0.75f;
                columna1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblDatosAlumno.AddCell(columna1);

                PdfPCell columna2 = new PdfPCell(new Phrase((_datosPracticante.Nombre + " " + _datosPracticante.Apellido),
                    _fuenteEstandar));
                columna2.Border = Rectangle.NO_BORDER;
                //columna2.BorderWidthBottom = 0.75f;
                columna2.BorderWidthTop = 0.75f;
                columna2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblDatosAlumno.AddCell(columna2);
                #endregion
                #region DNI
                PdfPCell columna3 = new PdfPCell(new Phrase("DNI del alumno", _fuenteCaractAlumno));
                columna3.Border = Rectangle.NO_BORDER;
                //columna3.BorderWidthBottom = 0.75f;
                columna3.BorderWidthTop = 0.75f;
                columna3.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblDatosAlumno.AddCell(columna3);

                PdfPCell columna4 = new PdfPCell(new Phrase(_datosPracticante.Dni.ToString(), _fuenteEstandar));
                columna4.Border = Rectangle.NO_BORDER;
                //columna4.BorderWidthBottom = 0.75f;
                columna4.BorderWidthTop = 0.75f;
                columna4.VerticalAlignment = Element.ALIGN_MIDDLE;
                tblDatosAlumno.AddCell(columna4);
                #endregion
                #region E-mail
                columna1 = new PdfPCell(new Phrase("E-mail", _fuenteCaractAlumno));
                columna1.Border = Rectangle.NO_BORDER;
                columna1.BorderWidthBottom = 0.75f;
                //columna1.BorderWidthTop = 0.75f;
                columna1.VerticalAlignment = Element.ALIGN_MIDDLE;
                columna1.SpaceCharRatio = 10f;
                tblDatosAlumno.AddCell(columna1);

                columna2 = new PdfPCell(new Phrase(_datosPracticante.Email, _fuenteEstandar));
                columna2.Border = Rectangle.NO_BORDER;
                columna2.BorderWidthBottom = 0.75f;
                //columna2.BorderWidthTop = 0.75f;
                columna2.VerticalAlignment = Element.ALIGN_MIDDLE;
                columna2.SpaceCharRatio = 10f;
                tblDatosAlumno.AddCell(columna2);
                #endregion
                #region Fecha
                columna3 = new PdfPCell(new Phrase("Fecha", _fuenteCaractAlumno));
                columna3.Border = Rectangle.NO_BORDER;
                columna3.BorderWidthBottom = 0.75f;
                columna3.VerticalAlignment = Element.ALIGN_MIDDLE;
                columna3.SpaceCharRatio = 10f;
                tblDatosAlumno.AddCell(columna3);

                string fechaString = (_fecha.Year.ToString() + "/" + _fecha.Month.ToString() +
                    "/" + _fecha.Day.ToString() + " " + _fecha.Hour.ToString() + ":" + _fecha.Minute.ToString());

                columna4 = new PdfPCell(new Phrase(fechaString, _fuenteEstandar));
                columna4.Border = Rectangle.NO_BORDER;
                columna4.BorderWidthBottom = 0.75f;
                columna4.VerticalAlignment = Element.ALIGN_MIDDLE;
                columna4.SpaceCharRatio = 10f;
                tblDatosAlumno.AddCell(columna4);
                #endregion

                doc.Add(tblDatosAlumno); // Añado la tabla al documento
                #endregion

                doc.Add(Chunk.NEWLINE); // Espacio

                #region Datos de la practica
                string stringEstadisticas = "Estadísticas                                                     ";
                Chunk chEstadisticas = new Chunk(stringEstadisticas, _fuenteSubtitulos);
                chEstadisticas.SetBackground(backgroundTitulos);
                doc.Add(new Paragraph(chEstadisticas));
                
                // Creamos una tabla que contendrá los datos de la practica
                PdfPTable tblDatosPractica = new PdfPTable(2);
                tblDatosPractica.WidthPercentage = 100;
                tblDatosPractica.SpacingBefore = 10f;

                // Configuramos el título de las columnas de la tabla
                PdfPCell columnaDescripcion = new PdfPCell(new Phrase("Descripción", _fuenteEstandar));
                //columnaDescripcion.BorderWidth = 0;
                //columnaDescripcion.BorderWidthTop = 0.75f;

                PdfPCell columnaCantidad = new PdfPCell(new Phrase("Cantidad", _fuenteEstandar));
                //columnaCantidad.BorderWidth = 0;
                //columnaCantidad.BorderWidthBottom = 0.75f;
                //columnaCantidad.HorizontalAlignment = Element.ALIGN_RIGHT;

                // Añadimos las celdas a la tabla
                //tblDatosPractica.AddCell(columnaDescripcion);
                //tblDatosPractica.AddCell(columnaCantidad);

                foreach(DictionaryEntry de in _datosPractica)
                {
                    columnaDescripcion = new PdfPCell(new Phrase(de.Key.ToString(), _fuenteEstandar));
                    columnaDescripcion.BorderWidth = 0;
                    columnaDescripcion.BorderWidthBottom = 0.75f;
                    columnaDescripcion.BorderColor = BaseColor.DARK_GRAY;

                    columnaCantidad = new PdfPCell(new Phrase(de.Value.ToString(), _fuenteEstandar));
                    columnaCantidad.BorderWidth = 0;
                    columnaCantidad.BorderWidthBottom = 0.75f;
                    columnaCantidad.HorizontalAlignment = Element.ALIGN_RIGHT;
                    columnaCantidad.BorderColor = BaseColor.DARK_GRAY;

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
                Console.WriteLine(ex);
                Console.WriteLine("no se pudo generar el pdf");
                return false;
            }
            
        }
    }
}
