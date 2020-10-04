using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using System;

namespace LumbApp.Expertos.ExpertoZE
{
    public class ExpertoZE : IExpertoZE
    {
        #region Variables
        /// <summary>
        /// CambioZE es un evento que se produce durante la simulacion cada vez que se da un cambio en el estado de alguna
        /// mano.
        /// </summary>
        public event EventHandler<CambioZEEventArgs> CambioZE;

        private readonly IConectorKinect kinect;
        private readonly ZonaEsteril zonaEsteril;

        private Mano manoDerecha;
        private Mano manoIzquierda;

        private bool simulando;
        private bool inicializado;

        private Video videoWriter;
        #endregion

        #region Metodos de experto
        /// <summary>
        /// Constructor de Experto en Zona Esteril.
        /// Este experto necesita una kinect para trabajar, que lo recibe por parametro. Tira una excepcion si recibe null.
        /// </summary>
        /// <param name="kinect">Conector a la kinect</param>
        /// <param name="calibracion">Datos de calibracion para la zona esteril</param>
        public ExpertoZE(IConectorKinect kinect, Calibracion calibracion)
        {
            if (kinect == null)
                throw new Exception("Kinect no puede ser null. Necesito un conector a una kinect para crear un experto en zona esteril");
            this.kinect = kinect;

            zonaEsteril = new ZonaEsteril(calibracion); //Puede tirar una excepcion si calibracion esta mal formado
        }

        #region Exclusivo de calibracion
        /// <summary>
        /// Constructor para calibracion. No usar en una simulacion, porque no va a tener una ZE definida.
        /// Este experto necesita una kinect para trabajar, que lo recibe por parametro. Tira una excepcion si recibe null.
        /// </summary>
        /// <param name="kinect">Conector a la kinect</param>
        public ExpertoZE(IConectorKinect kinect)
        {
            if (kinect == null)
                throw new Exception("Kinect no puede ser null. Necesito un conector a una kinect para crear un experto en zona esteril");
            this.kinect = kinect;
        }
        #endregion

        /// <summary>
        /// Inicializa todo lo necesario y queda listo para aceptar simulaciones.
        /// </summary>
        /// <returns>True si se inicializo todo bien, false si algo fallo y no podra aceptar simulaciones</returns>
        public bool Inicializar()
        {
            try
            {
                kinect.Conectar();
                kinect.SubscribeFramesReady(allFramesReady);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ZE: No pude inicializar el Experto en Zona Esteril por error con la Kinect. Error: " + ex);
                inicializado = false;
                return false;
            }
            inicializado = true;
            Console.WriteLine("ZE: Inicializado");
            return true;
        }

        /// <summary>
        /// Inicia una simulacion. A partir de este momento, registrara todos los datos relevantes y generara eventos
        /// para el orchestrator.
        /// </summary>
        /// <returns>True si se inicializo todo bien y efectivamente comenzo a sensar.
        /// False si esta funcion se llama antes de Inicializar</returns>
        public bool IniciarSimulacion()
        {
            if (!inicializado || zonaEsteril == null)
            {
                Console.WriteLine("ZE: No pude iniciar la simulación. Inicialice primero");
                return false;
            }

            manoDerecha = new Mano();
            manoIzquierda = new Mano();
            zonaEsteril.Resetear();
            videoWriter = new Video();
            simulando = true;
            Console.WriteLine("ZE: Simulación iniciada");
            return true;
        }

        /// <summary>
        /// Termina la simulacion y devuelve un resumen de los datos recopilados.
        /// No registrara datos nuevos ni generara eventos de CambioZE hasta que se inicie una nueva simulacion.
        /// Si no se estaba simulando, devuelve un informe vacio.
        /// </summary>
        /// <returns>Informe con un resumen de los datos recopilados</returns>
        public InformeZE TerminarSimulacion()
        {
            if (!simulando)
                return new InformeZE(0, 0, 0, videoWriter);

            Console.WriteLine("ZE: Simulación terminada");
            simulando = false;
            return new InformeZE(zonaEsteril.Contaminacion, manoDerecha.VecesContamino, manoIzquierda.VecesContamino, videoWriter);
        }

        /// <summary>
        /// Desconecta todo lo necesario para cerrar la aplicacion.
        /// </summary>
        public void Finalizar()
        {
            kinect.Desconectar();
            simulando = false;
            Console.WriteLine("ZE: Finalizado");
        }
        #endregion

        #region Metodos de procesamiento
        /// <summary>
        /// Esta funcion se llama cada vez que la kinect termina de calcular todas las frames.
        /// Solo procesa durante una simulacion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (!simulando)
                return;

            // Proceso esqueleto
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    Skeleton[] allSkeletons = new Skeleton[6];
                    frame.CopySkeletonDataTo(allSkeletons);
                    foreach (Skeleton skeleton in allSkeletons)
                    {
                        if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            processSkeleton(skeleton);
                            break; //Procesa solo el primer skeleton trackeado
                        }
                    }
                }
            }

            // Proceso video
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                    videoWriter.addFrame(frame);
            }
        }

        /// <summary>
        /// Procesa el esqueleto en un frame. Si ocurre algun cambio, invoca al evento CambioZE
        /// </summary>
        /// <param name="skeleton">Esqueleto a procesar</param>
        private void processSkeleton(Skeleton skeleton)
        {
            CambioZEEventArgs args = new CambioZEEventArgs(manoDerecha, manoIzquierda);

            bool cambio = processHand(skeleton.Joints[JointType.HandRight], manoDerecha, args);
            cambio = processHand(skeleton.Joints[JointType.HandLeft], manoIzquierda, args) || cambio;

            if (cambio)
            {
                args.VecesContaminado = zonaEsteril.Contaminacion;
                CambioZE.Invoke(this, args);
            }
        }

        /// <summary>
        /// Procesa una mano.
        /// </summary>
        /// <param name="pos">SkeletonPoint con la posicion de la mano</param>
        /// <param name="mano">Objeto Mano</param>
        /// <param name="eventArgs">Argumentos por si hay que generar un evento</param>
        /// <returns>True si hubo algun cambio</returns>
        private bool processHand(Joint joint, Mano mano, CambioZEEventArgs eventArgs)
        {
            //Tracking
            bool cambioTrack = false;
            cambioTrack = mano.ActualizarTrack(joint.TrackingState == JointTrackingState.Tracked);
            if (mano.Track == Mano.Tracking.Perdido)
                return cambioTrack;

            //Zona esteril
            bool cambioZE;
            var pos = joint.Position;
            if (zonaEsteril.EstaDentro(pos))
            {
                cambioZE = mano.Entrar();
                if (cambioZE && mano.Estado == Mano.Estados.Contaminando)
                {
                    eventArgs.ContaminadoAhora = true;
                    zonaEsteril.Contaminar();
                }
            }
            else cambioZE = mano.Salir();

            return cambioTrack || cambioZE;
        }
        #endregion
    }
}
