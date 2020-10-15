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
using System.IO;
using System.Threading.Tasks;

namespace LumbApp.Orquestador
{
    public class Orquestador : IOrquestador
    {
        #region Variables
        public IGUIController IGUIController { get; set; }

        private IExpertoZE expertoZE;
        private IExpertoSI expertoSI;
        private IFinalFeedbacker ffb;
        private IConectorFS fileSystem;
        private string ruta;

        private DatosPracticante datosPracticante;
        private ModoSimulacion modoSeleccionado;

        private DateTime tiempoInicialDeEjecucion;
        private TimeSpan tiempoTotalDeEjecucion;
        #endregion


        /// <summary>
        /// Constructor del Orquestrador.
        /// Se encarga de construir los expertos y la GUI manejando para manejar la comunicación entre ellos.
        /// </summary>
        public Orquestador(IGUIController gui, IConectorFS conectorFS)
        {
            if (gui == null)
                throw new Exception("Gui no puede ser null. Necesito un GUIController para crear un Orquestador.");

            IGUIController = gui;
            fileSystem = conectorFS;
        }

        #region ABM simulacion
        /// <summary>
        /// Lo llama GUI Controller cuando el usuario termina de ingresar sus datos
        /// </summary>
        /// <param name="datosPracticante"></param>
        /// <param name="modo">Modo de simulacion</param>
        public void SetDatosDeSimulacion(DatosPracticante datosPracticante, ModoSimulacion modo)
        {
            this.datosPracticante = datosPracticante;
            this.modoSeleccionado = modo;
        }

        /// <summary>
        /// IniciarSimulacion: Informa a los expertos que deben inicar el sensado.
        /// </summary>
        public void IniciarSimulacion()
        {
            Console.WriteLine("Iniciando simulacion en " + modoSeleccionado);
            tiempoInicialDeEjecucion = DateTime.Now;
            ruta = ObtenerRuta(tiempoInicialDeEjecucion);

            expertoZE.IniciarSimulacion(new Video(ruta + ".mp4"));
            expertoSI.IniciarSimulacion();

            if (modoSeleccionado == ModoSimulacion.ModoGuiado)
                IGUIController.IniciarSimulacionModoGuiado();
            else
                IGUIController.IniciarSimulacionModoEvaluacion();
        }

        /// <summary>
        /// Manda a inicializar los expertos y pide la pantalla de ingreso de datos.
        /// Si algun experto no pudo inicializar correctamente, envía a la GUI un mensaje de error.
        /// Lo llama la GUI mientras muestra "checkeando sensores" al iniciar la app
        /// </summary>
        /// <returns></returns>
        public async Task Inicializar()
        {
            Console.WriteLine("Inicializando...");
            try
            {
                #region Inicializar ZE
                Calibracion calibracion;
                try
                {
                    calibracion = fileSystem.LevantarArchivoDeTextoComoObjeto<Calibracion>("./zonaEsteril.json");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //Mejoro el mensaje para el usuario
                    throw new Exception("Error al tratar de cargar el archivo de calibracion. Por favor, calibre el sistema antes de usarlo.");
                }

                var conectorKinect = new ConectorKinect();
                expertoZE = new ExpertoZE(conectorKinect, calibracion);
                //expertoZE = new ExpertoZEMock(true);

                expertoZE.CambioZE += CambioZE; //suscripción al evento CambioZE
                if (!expertoZE.Inicializar())
                    throw new Exception("No se pudo detectar correctamente la kinect. Asegúrese de que esté conectada e intente nuevamente.");
                #endregion

                #region Inicializar SI
                //var conectorSI = new ConectorSI();
                //expertoSI = new ExpertoSI(conectorSI);
                expertoSI = new ExpertoSIMock(true);

                expertoSI.CambioSI += CambioSI; //suscripción al evento CambioSI
                if (!expertoSI.Inicializar())
                    throw new Exception("No se pudieron detectar correctamente los sensores internos. Asegúrese de que el simulador esté conectado e intente nuevamente");
                #endregion

                //Mostrar pantalla de ingreso de datos, le mandamos el path por default donde se guarda la practica
                var datos = new DatosPracticante()
                {
                    FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                };
                IGUIController.SolicitarDatosPracticante(datos);
            }
            catch (Exception ex)
            {
                //Descarto lo que se habia creado. Cuando lo agarre el garbage collector, se desuscribe solo
                expertoZE = null;
                expertoSI = null;

                Console.WriteLine("Error de inicializacion: " + ex);
                IGUIController.MostrarErrorDeConexion(ex.Message);
            }
        }

        /// <summary>
        /// Lo llama GUI Controller cuando termino una simulacion y el usuario pide comenzar otra.
        /// Redirige a la pantalla de ingreso de datos.
        /// </summary>
        /// <returns></returns>
        public async Task NuevaSimulacion()
        {
            IGUIController.SolicitarDatosPracticante(datosPracticante);
        }

        /// <summary>
        /// Terminar simulación: indica a los expertos que dejen de tomar estadísticas y devuelvan la informacion que obtuvo cada uno.
        /// - Con la informacion regida genera un informe general que se envia al final feedbacker y se guarda.
        /// - Si el informe general se genero y guardo bien, levanta un evento ue es atrapado por la GUI para decirle que todo salio bien.
        /// </summary>
        public async Task TerminarSimulacion()
        {
            Console.WriteLine("Terminando simulacion...");
            InformeZE informeZE = expertoZE.TerminarSimulacion();
            InformeSI informeSI = expertoSI.TerminarSimulacion();

            DateTime tiempoFinal = DateTime.Now;
            tiempoTotalDeEjecucion = tiempoFinal - tiempoInicialDeEjecucion;

            Informe informeFinal = new Informe(
                this.datosPracticante.Nombre,
                this.datosPracticante.Apellido,
                this.datosPracticante.Dni,
                this.datosPracticante.FolderPath,
                informeSI, informeZE, tiempoTotalDeEjecucion
                );

            ffb = new FinalFeedbacker(ruta + ".pdf", datosPracticante, informeFinal.DatosPractica, tiempoFinal);
            informeFinal.SetPdfGenerado(ffb.GenerarPDF());
            informeZE.Video.Save();

            IGUIController.MostrarResultados(informeFinal);

            //Informar a GUI con informe con un evento, que pase si el informe se genero bien, y si se guardó  bien (bool, bool)
        }
        #endregion

        #region Cosas de simulacion
        /// <summary>
        /// Crea la ruta donde guardar los datos de la practica a partir del tiempo de inicio y los datos del practicante.
        /// </summary>
        /// <param name="tiempo">Tiempo en que se inicia la simulacion, para incluir en el nombre de la carpeta</param>
        /// <returns></returns>
        private string ObtenerRuta(DateTime tiempo)
        {
            string ruta = datosPracticante.FolderPath;
            string carpetaAlumno = datosPracticante.Apellido + "_" + datosPracticante.Nombre + "_" + datosPracticante.Dni;

            string nombreArchivos = string.Format("{0:D4}-{1:D2}-{2:D2}_{3:D2}-{4:D2}_{5}",
                tiempo.Year, tiempo.Month, tiempo.Day, tiempo.Hour.ToString(), tiempo.Minute, datosPracticante.Apellido);

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

        /// <summary>
        /// Responde a los eventos de cambio en la zona esteril. En modo guiado pasa el cambio
        /// al front, en modo evaluacion lo ignora.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CambioZE(object sender, CambioZEEventArgs e)
        {
            //comunicar los cambios a la GUI levantando un evento
            if (modoSeleccionado == ModoSimulacion.ModoGuiado)
                IGUIController.MostrarCambioZE(e);
        }
        #endregion

        #region Metodos de test
        /// <summary>
        /// Funcion de test para mockear el experto SI
        /// </summary>
        /// <param name="exp"></param>
        public void SetExpertoSI(IExpertoSI exp)
        {
            this.expertoSI = exp;
        }

        /// <summary>
        /// Funcion de test para mockear el experto ZE
        /// </summary>
        /// <param name="exp"></param>
        public void SetExpertoZE(IExpertoZE exp)
        {
            this.expertoZE = exp;
        }
        #endregion
    }
}
