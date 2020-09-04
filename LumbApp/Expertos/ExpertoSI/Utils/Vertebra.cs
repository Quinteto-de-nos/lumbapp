using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI.Utils {
    public class Vertebra {
        
        public enum Estados {
            /// <summary>
            /// Inicial indica que, desde que comenzo la simulacion, la vertebra todavia no fue rozada.
            /// Es decir, la aguja no la toco.
            /// </summary>
            Inicial,

            /// <summary>
            /// Rozando indica que la vertebra esta siendo tocada por la aguja en ese momento
            /// y es la primera vez que se toca.
            /// </summary>
            Rozando,

            /// <summary>
            /// Abandonada indica que la vertebra no esta siento tocada, pero ya fue tocada por primera vez.
            /// Si se vuelve a tocar, se considera un reintento.
            /// </summary>
            Abandonada,

            /// <summary>
            /// RozandoNuevamente indica que la vertebra esta siendo tocada por la aguja en ese momento
            /// y es al menos la segunda vez que se toca.
            /// </summary>
            RozandoNuevamente
        };

        public Estados Estado { get; private set; }
        public int VecesRozada { get; private set; }

        public Vertebra () {
            Estado = Estados.Inicial;
            VecesRozada = 0;
        }

        /// <summary>
        /// Sirve para vertebras que no estan sectorizadas.
        /// Setea a la vertebra que esta siendo rozada. Segun el estado anterior, puede quedar en Rozando
        /// o RozandoNuevamente.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool Rozar () {
            switch (Estado) {
                case Estados.Inicial:
                    Estado = Estados.Rozando;
                    if (VecesRozada == 0)
                        VecesRozada = 1;
                    return true;
                case Estados.Abandonada:
                    Estado = Estados.RozandoNuevamente;
                    VecesRozada++;
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Setea a la vertebra que esta siendo abandonada. Si estaba en Inicial, lo mantiene, sino
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
