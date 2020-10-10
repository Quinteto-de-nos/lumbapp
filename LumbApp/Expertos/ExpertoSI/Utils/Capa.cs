using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI.Utils
{
    public class Capa
    {
        public enum Estados
        {
            /// <summary>
            /// Inicial indica que, desde que comenzo la simulacion, la capa todavia no fue atravesada.
            /// Es decir, la aguja no la toco.
            /// </summary>
            Inicial,

            /// <summary>
            /// Atravesando indica que la capa esta siendo atravesada por la aguja en ese momento
            /// y es la primera vez que se atraviesa.
            /// </summary>
            Atravesando,

            /// <summary>
            /// Abandonada indica que la capa no esta siento atravesada, pero ya fue atravesada por primera vez.
            /// Si se vuelve a atravesar, se considera un reintento.
            /// </summary>
            Abandonada,

            /// <summary>
            /// AtravesandoNuevamente indica que la capa esta siendo atravesada por la aguja en ese momento
            /// y es al menos la segunda vez que se atraviesa.
            /// </summary>
            AtravesandoNuevamente
        };

        public Estados Estado { get; private set; }
        public int VecesAtravesada { get; private set; }

        public Capa()
        {
            Estado = Estados.Inicial;
            VecesAtravesada = 0;
        }

        /// <summary>
        /// Setea a la capa que esta siento atravesada. Segun el estado anterior, puede quedar en Atravesando
        /// o AtravesandoNuevamente.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool Atravesar()
        {
            switch (Estado)
            {
                case Estados.Inicial:
                    Estado = Estados.Atravesando;
                    VecesAtravesada++;
                    return true;
                case Estados.Abandonada:
                    Estado = Estados.AtravesandoNuevamente;
                    VecesAtravesada++;
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Setea a la capa que esta siendo abandonada. Si estaba en Inicial, lo mantiene, sino
        /// pasa a Abandonada.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool Abandonar()
        {
            if (Estado == Estados.Inicial || Estado == Estados.Abandonada)
                return false;
            Estado = Estados.Abandonada;
            return true;
        }

        public void Resetear()
        {
            Estado = Estados.Inicial;
            VecesAtravesada = 0;
        }
    }
}
