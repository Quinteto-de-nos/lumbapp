namespace LumbApp.Expertos.ExpertoZE
{
    public class Mano
    {
        public enum Tracking { Trackeado, Perdido };
        public Tracking Track { get; private set; }

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
        public Estados Estado { get; private set; }

        public int VecesContamino { get; private set; }

        public Mano()
        {
            Track = Tracking.Perdido;
            Estado = Estados.Inicial;
            VecesContamino = 0;
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

            switch (Estado)
            {
                case Estados.Inicial:
                    Estado = Estados.Trabajando;
                    return true;
                case Estados.Fuera:
                    Estado = Estados.Contaminando;
                    VecesContamino++;
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
            if (Estado == Estados.Inicial || Estado == Estados.Fuera)
                return false;
            Estado = Estados.Fuera;
            return true;
        }

        /// <summary>
        /// Actualiza el track de la mano.
        /// </summary>
        /// <param name="trackeado">True si la mano esta trackeada en este momento</param>
        /// <returns>True si esta funcion realizo un cambio en Track</returns>
        public bool ActualizarTrack(bool trackeado)
        {
            var old = Track;
            if (trackeado)
                Track = Tracking.Trackeado;
            else Track = Tracking.Perdido;
            return old != Track;
        }
    }
}
