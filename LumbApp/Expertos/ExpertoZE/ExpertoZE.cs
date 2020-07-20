using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoZE
{
    public class ExpertoZE
    {
        private IConectorKinect kinect;
        private ZonaEsteril zonaEsteril;

        private Mano manoDerecha;
        private Mano manoIzquierda;

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

        /// <summary>
        /// Inicializa todo lo necesario y queda listo para aceptar simulaciones.
        /// </summary>
        /// <returns>True si se inicializo todo bien, false si algo fallo y no podra aceptar simulaciones</returns>
        public bool Inicializar() 
        {
            zonaEsteril = new ZonaEsteril();
            manoDerecha = new Mano();
            manoIzquierda = new Mano();

            try
            {
                kinect.Conectar();
                kinect.SubscribeFramesReady(allFramesReady);
            }
            catch
            {
                // TODO: log
                return false;
            }
            return true;
        }

        public bool IniciarSimulacion() {
            manoDerecha = new Mano();
            manoIzquierda = new Mano();
            return true;  
        }
        public void TerminarSimulacion() { }
        public void GetInforme() { }
        public void Finalizar() {
            kinect.Desconectar();
        }

        /// <summary>
        /// Esta funcion se llama cada vez que la kinect termina de calcular todas las frames.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allFramesReady(object sender, AllFramesReadyEventArgs e) {
            using (var frame = e.OpenSkeletonFrame())
            {
                if(frame != null)
                {
                    Skeleton[] allSkeletons = new Skeleton[6];
                    frame.CopySkeletonDataTo(allSkeletons);
                    foreach(Skeleton skeleton in allSkeletons)
                    {
                        if(skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            processSkeleton(skeleton);
                            break;
                        }
                    }
                }
            }
        }

        private void processSkeleton(Skeleton skeleton)
        {
            Console.WriteLine(skeleton.ClippedEdges);

            SkeletonPoint mano = skeleton.Joints[JointType.HandRight].Position;
            if (zonaEsteril.EstaDentro(mano.X, mano.Y, mano.Z))
                manoDerecha.Entrar();
            else manoDerecha.Salir();
        }
    }
}
