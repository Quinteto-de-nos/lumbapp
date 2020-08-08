using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using System;

namespace LumbApp.Expertos.ExpertoZE
{
    public class ExpertoZE
    {
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
            zonaEsteril = new ZonaEsteril();

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
            return true;
        }

        public bool IniciarSimulacion()
        {
            manoDerecha = new Mano();
            manoIzquierda = new Mano();
            zonaEsteril.Resetear();
            simulando = true;
            return true;
        }
        public void TerminarSimulacion()
        {
            simulando = false;
        }
        public void GetInforme() { }
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

        private void processSkeleton(Skeleton skeleton)
        {
            processHand(skeleton.Joints[JointType.HandRight].Position, manoDerecha);
            processHand(skeleton.Joints[JointType.HandLeft].Position, manoIzquierda);
        }

        private void processHand(SkeletonPoint pos, Mano mano)
        {
            if (zonaEsteril.EstaDentro(pos.X, pos.Y, pos.Z))
            {
                bool cambio = mano.Entrar();
                if (cambio && mano.Estado == Mano.Estados.Contaminando)
                    zonaEsteril.Contaminar();
            }
            else mano.Salir();
        }
    }
}
