using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI.Utils {
    public class Capa {
        public enum Estados {
            /// <summary>
            /// Inicial indica que, desde que comenzo la simulacion, la capa todavia no fue atravezada.
            /// Es decir, la aguja no la toco.
            /// </summary>
            Inicial,

            /// <summary>
            /// Atravezando indica que la capa esta siendo atravezada por la aguja en ese momento
            /// y es la primera vez que se atravieza.
            /// </summary>
            Atravezando,

            /// <summary>
            /// Abandonada indica que la capa no esta siento atravezada, pero ya fue atravezada por primera vez.
            /// Si se vuelve a atravezar, se considera un reintento.
            /// </summary>
            Abandonada,

            /// <summary>
            /// AtravezandoNuevamente indica que la capa esta siendo atravezada por la aguja en ese momento
            /// y es al menos la segunda vez que se atravieza.
            /// </summary>
            AtravezandoNuevamente
        };

        public Estados Estado { get; private set; }
        public int VecesAtravezada { get; private set; }

        public Capa () {
            Estado = Estados.Inicial;
            VecesAtravezada = 0;
        }

        /// <summary>
        /// Setea a la capa que esta siento atravezada. Segun el estado anterior, puede quedar en Atravezando
        /// o AtravezandoNuevamente.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool Atravezar () {
            switch (Estado) {
                case Estados.Inicial:
                    Estado = Estados.Atravezando;
                    return true;
                case Estados.Abandonada:
                    Estado = Estados.AtravezandoNuevamente;
                    VecesAtravezada++;
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Setea a la capa que esta siendo abandonada. Si estaba en Inicial, lo mantiene, sino
        /// pasa a Abandonada.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool Abandonar () {
            if (Estado == Estados.Inicial || Estado == Estados.Abandonada)
                return false;
            Estado = Estados.Abandonada;
            return true;
        }
    }
}
