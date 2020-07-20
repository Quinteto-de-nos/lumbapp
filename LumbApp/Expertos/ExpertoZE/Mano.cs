using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoZE
{
    public class Mano
    {
        public enum Tracking { Trackeado, Perdido };
        public Tracking Track;

        public enum Estados
        {
            /// <summary>
            /// Inicial indica que, desde que comenzo la simulacion, la mano todavia no entro a la zona esteril.
            /// Es decir, nunca fue trackeada o se encuentra afuera.
            /// </summary>
            Inicial,

            /// <summary>
            /// Trabajando indica que la mano se encuentra dentro de la zona esteril y es la primera vez que ingresa.
            /// </summary>
            Trabajando,

            /// <summary>
            /// Fuera indica que la mano se encuentra fuera de la zona esteril, ya paso por la zona y no deberia volver 
            /// a entrar, si lo hace contaminara la zona.
            /// </summary>
            Fuera,

            /// <summary>
            /// Contaminando indica que la mano se encuentra dentro de la zona esteril y es al menos la segunda vez que
            /// entra, por lo que esta contaminando la zona.
            /// </summary>
            Contaminando
        };
        private Estados estado;
        public Estados Estado { get => estado; private set => estado = value; }


        public Mano()
        {
            Track = Tracking.Perdido;
            estado = Estados.Inicial;
        }

        /// <summary>
        /// Setea a la mano dentro de la zona esteril. Segun el estado anterior, puede quedar en Trabajando
        /// o Contaminando.
        /// No hay problema si se llama varias veces cuando ya esta adentro.
        /// Asume que si saben que entro es porque esta trackeada.
        /// </summary>
        /// <returns>Si hubo cambio de estado</returns>
        public bool Entrar()
        {
            Track = Tracking.Trackeado;

            switch (estado)
            {
                case Estados.Inicial:
                    estado = Estados.Trabajando;
                    return true;
                case Estados.Fuera:
                    estado = Estados.Contaminando;
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Setea a la mano fuera de la zona esteril. Si estaba en Inicial, lo mantiene, sino
        /// pasa a Fuera.
        /// No hay problema con llamarla cuando ya esta afuera.
        /// Asumen que si saben que salio es porque esta trackeada.
        /// </summary>
        /// <returns>Si hubo cambio de estado</returns>
        public bool Salir()
        {
            Track = Tracking.Trackeado;
            if (estado == Estados.Inicial || estado == Estados.Fuera)
                return false;
            estado = Estados.Fuera;
            return true;
        }
    }
}
