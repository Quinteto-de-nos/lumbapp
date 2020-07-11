using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoZE
{
    class ExpertoZE
    {
        private ConectorKinect kinect;

        /// <summary>
        /// Constructor de Expero en Zona Esteril.
        /// Este experto necesita una kinect para trabajar, que lo recibe por parametro. Tira una excepcion si recibe null.
        /// </summary>
        /// <param name="kinect">Conector a la kinect</param>
        public ExpertoZE(ConectorKinect kinect) 
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

        public bool IniciarSimulacion() { return false;  }
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
        private void allFramesReady(object sender, AllFramesReadyEventArgs e) { }
    }
}
