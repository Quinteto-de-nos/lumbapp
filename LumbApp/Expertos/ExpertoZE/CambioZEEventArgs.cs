using System;

namespace LumbApp.Expertos.ExpertoZE
{
    public class CambioZEEventArgs : EventArgs
    {
        /// <summary>
        /// Contiene los datos relacionados a la mano derecha.
        /// </summary>
        public Mano ManoDerecha { get; }
        /// <summary>
        /// Contiene los datos relacionados a la mano izquierda.
        /// </summary>
        public Mano ManoIzquierda { get; }
        /// <summary>
        /// Cantidad de veces que fue contaminada la zona esteril, sin importar por que mano.
        /// </summary>
        public int VecesContaminado { get; internal set; }
        /// <summary>
        /// True si la zona esteril fue contaminada durante este evento.
        /// </summary>
        public bool ContaminadoAhora { get; internal set; }

        internal CambioZEEventArgs(Mano derecha, Mano izquierda)
        {
            ManoDerecha = derecha;
            ManoIzquierda = izquierda;
        }

        public CambioZEEventArgs()
        {
            ManoDerecha = new Mano();
            ManoIzquierda = new Mano();
        }

        //Clona el objeto con todos sus datos
        public CambioZEEventArgs Shallowcopy()
        {
            return (CambioZEEventArgs)this.MemberwiseClone();
        }
    }
}
