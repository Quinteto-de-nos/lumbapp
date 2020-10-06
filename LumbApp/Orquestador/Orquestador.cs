﻿using LumbApp.Conectores.ConectorFS;
using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Enums;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.FinalFeedbacker_;
using LumbApp.GUI;
using LumbApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LumbApp.Orquestador
{
    public class Orquestador : IOrquestador
    {
        public IGUIController IGUIController { get; set; }

        private IExpertoZE expertoZE;
        private IExpertoSI expertoSI;

        private DatosPracticante datosPracticante;
        private ModoSimulacion modoSeleccionado;

        private IConectorKinect conectorKinect;
        private IConectorSI conectorSI;

        private DateTime tiempoInicialDeEjecucion;
        private TimeSpan tiempoTotalDeEjecucion;

        private IFinalFeedbacker ffb;

        /// <summary>
        /// Constructor del Orquestrador.
        /// Se encarga de construir los expertos y la GUI manejando para manejar la comunicación entre ellos.
        /// </summary>
        public Orquestador(IGUIController gui, IConectorFS conectorFS)
        {
            if (gui == null)
                throw new Exception("Gui no puede ser null. Necesito un GUIController para crear un Orquestador.");
            IGUIController = gui;
            Calibracion calibracion;
            try
            {
                calibracion = conectorFS.LevantarArchivoDeTextoComoObjeto<Calibracion>("./zonaEsteril.json");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //Acá debería haber un nuevo mensaje por pantalla que me permita quitar las app, esto es incluso antes de la inicialización, asíq ue no puedo reintentar.
                throw new Exception("Error al tratar de cargar el archivo de calibracion.");
            }
            //conectorKinect = new ConectorKinect();
            //expertoZE = new ExpertoZE(conectorKinect, calibracion);
            expertoZE = new ExpertoZEMock(true);

            //conectorSI = new ConectorSI();
            //expertoSI = new ExpertoSI(conectorSI);
            expertoSI = new ExpertoSIMock(true);
        }

        public void SetDatosDeSimulacion(Models.DatosPracticante datosPracticante, ModoSimulacion modo)
        {
            this.datosPracticante = datosPracticante;

            this.modoSeleccionado = modo;
        }

        /// <summary>
        /// IniciarSimulacion: Informa a los expertos que deben inicar el sensado.
        /// 
        /// </summary>
        public void IniciarSimulacion()
        {

            expertoZE.IniciarSimulacion();

            expertoSI.IniciarSimulacion();


            if (modoSeleccionado == ModoSimulacion.ModoGuiado)
                IGUIController.IniciarSimulacionModoGuiado();
            else
                IGUIController.IniciarSimulacionModoEvaluacion();

            tiempoInicialDeEjecucion = DateTime.UtcNow;

        }

        /// <summary>
        /// Inicializar: Se encarga de mandar a inicializar los expertos y pedir la pantalla de ingreso de datos.
        /// - Si algun experto no pudo inicializar correctamente, envía a la GUI un mensaje de error.
        /// - Sucede al abrir la aplicación. La GUI muestra un gif "checkeando sensores" y nos manda a inicializar.
        /// </summary>
        /// <returns></returns>
        public async Task Inicializar()
        {

            try
            {
                //INICIALIZAR EXPERTO ZE
                expertoZE.CambioZE += CambioZE; //suscripción al evento CambioZE
                if (!expertoZE.Inicializar())
                    throw new Exception("No se pudo detectar correctamente la kinect.");

                //INICIALIZAR EXPERTO SI
                expertoSI.CambioSI += CambioSI; //suscripción al evento CambioSI
                if (!expertoSI.Inicializar())
                    throw new Exception("No se pudieron detectar correctamente los sensores internos.");

                //Mostrar pantalla de ingreso de datos, le mandamos el path por default donde se guarda la practica
                IGUIController.SolicitarDatosPracticante("C:\\Desktop");

            }
            catch (Exception ex)
            {
                expertoZE.CambioZE -= CambioZE;
                if (ex.Message.Contains("sensores"))
                    expertoSI.CambioSI -= CambioSI;

                IGUIController.MostrarErrorDeConexion(ex.Message);
            }

        }

        public async Task NuevaSimulacion()
        { //Funcion llamada por la GUI, devuelve void, respuesta por evento
            IGUIController.SolicitarDatosPracticante(datosPracticante.FolderPath);
        }

        /// <summary>
        /// Terminar simulación: indica a los expertos que dejen de tomar estadísticas y devuelvan la informacion que obtuvo cada uno.
        /// - Con la informacion regida genera un informe general que se envia al final feedbacker y se guarda.
        /// - Si el informe general se genero y guardo bien, levanta un evento ue es atrapado por la GUI para decirle que todo salio bien.
        /// </summary>
        public async Task TerminarSimulacion()
        { //Funcion llamada por la GUI, devuelve void, respuesta por evento

            InformeZE informeZE = expertoZE.TerminarSimulacion();
            InformeSI informeSI = expertoSI.TerminarSimulacion();

            tiempoTotalDeEjecucion = DateTime.UtcNow - tiempoInicialDeEjecucion;

            DateTime tiempoFinal = DateTime.Now;
            String ruta = ObtenerRuta(tiempoFinal);

            Informe informeFinal = new Informe(
                this.datosPracticante.Nombre,
                this.datosPracticante.Apellido,
                this.datosPracticante.Dni,
                this.datosPracticante.FolderPath,
                informeSI, informeZE, tiempoTotalDeEjecucion
                );

            ffb = new FinalFeedbacker((ruta + ".pdf"), datosPracticante, informeFinal.DatosPractica, tiempoFinal);

            informeFinal.SetPdfGenerado(ffb.GenerarPDF());

            // Esta linea guarda el video de la kinect.
            informeZE.Video.Save(ruta + ".mp4");

            IGUIController.MostrarResultados(informeFinal);

            //Informar a GUI con informe con un evento, que pase si el informe se genero bien, y si se guardó  bien (bool, bool)
        }

        private String ObtenerRuta(DateTime tiempo)
        {
            string ruta = datosPracticante.FolderPath;
            string carpetaAlumno = datosPracticante.Apellido + "_" + datosPracticante.Nombre + "_" + datosPracticante.Dni;

            string nombreArchivos = tiempo.Year.ToString() + "-" + tiempo.Month.ToString() +
                "-" + tiempo.Day.ToString() + "_" + tiempo.Hour.ToString() + "-" +
                tiempo.Minute.ToString() + "_" + datosPracticante.Apellido;

            if (!ruta.Contains(carpetaAlumno))
            {
                ruta += ("\\" + carpetaAlumno);
                if (!Directory.Exists(ruta))
                {
                    Console.WriteLine("Creando el directorio: {0}", ruta);
                    Directory.CreateDirectory(ruta);
                }
            }
            ruta += ("\\" + nombreArchivos);

            return ruta;
        }


        /// <summary>
        /// CambioSI: atrapa los eventos que indican un cambio en el sensado interno
        /// - En MODO GUIADO, informa los cambios a la GUI para que esta pueda mostrarlos
        /// - En MODO EVALUACION, ... (creo que no hace nada, ya que el informe lo arman los expertos)
        /// </summary>
        /// <param name="sender"> Remitente del evento </param>
        /// <param name="datosDelEvento"> Datos del cambio (capas, vertebras y correctitud del camino) </param>
        private void CambioSI(object sender, CambioSIEventArgs datosDelEvento)
        {
            //comunicar los cambios a la GUI levantando un evento
            if (modoSeleccionado == ModoSimulacion.ModoGuiado)
                IGUIController.MostrarCambioSI(datosDelEvento);
        }

        private void CambioZE(object sender, CambioZEEventArgs e)
        {
            //comunicar los cambios a la GUI levantando un evento
            if (modoSeleccionado == ModoSimulacion.ModoGuiado)
                IGUIController.MostrarCambioZE(e);
        }

        public IExpertoSI GetExpertoSI()
        {
            return expertoSI;
        }
        public void SetExpertoSI(IExpertoSI exp)
        {
            this.expertoSI = exp;
        }

        public void SetExpertoZE(IExpertoZE exp)
        {
            this.expertoZE = exp;
        }

    }
}
