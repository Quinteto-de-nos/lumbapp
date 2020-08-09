using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using System;

namespace LumbApp.Expertos.ExpertoZE
{
    public class ExpertoZE
    {
        /// <summary>
        /// CambioZE es un evento que se produce durante la simulacion cada vez que se da un cambio en el estado de alguna
        /// mano.
        /// </summary>
        public event EventHandler<CambioZEEventArgs> CambioZE;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private IConectorKinect kinect;
        private ZonaEsteril zonaEsteril;

        private Mano manoDerecha;
        private Mano manoIzquierda;

        private bool simulando;

        /// <summary>
        /// Constructor de Expero en Zona Esteril.
        /// Este experto necesita una kinect para trabajar, que lo recibe por parametro. Tira una excepcion si recibe null.
        /// </summary>
        /// <param name="kinect">Conector a la kinect</param>
        public ExpertoZE(IConectorKinect kinect)
        {
            if (kinect == null)
                throw new Exception("Kinect no puede ser null. Necesito un conector a una kinect para crear un experto en zona esteril");
            this.kinect = kinect;
        }

        public event EventHandler<CambioZEEventArgs> CambioZE;

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
                logger.Error(ex, "No pude inicializar el Experto en Zona Esteril por error con la Kinect");
                return false;
            }

            zonaEsteril = new ZonaEsteril();
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
            if (zonaEsteril == null)
                return false;

            manoDerecha = new Mano();
            manoIzquierda = new Mano();
            zonaEsteril.Resetear();
            simulando = true;
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
                return new InformeZE(0,0,0);

            simulando = false;
            return new InformeZE(zonaEsteril.Contaminacion, manoDerecha.VecesContamino, manoIzquierda.VecesContamino);
        }

        /// <summary>
        /// Desconecta todo lo necesario para cerrar la aplicacion.
        /// </summary>
        public void Finalizar()
        {
            kinect.Desconectar();
            simulando = false;
        }

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
            bool cambioZE = false;
            var pos = joint.Position;
            if (zonaEsteril.EstaDentro(pos.X, pos.Y, pos.Z))
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
    }
}
