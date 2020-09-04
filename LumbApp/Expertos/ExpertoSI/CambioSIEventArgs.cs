using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI {
    public class CambioSIEventArgs : EventArgs {
        /// <summary>
        /// Tejido-Adiposo: posicion 2 - (Arduino posicion 0)
        /// </summary>
        public Capa TejidoAdiposo;

        /// <summary>
        /// Vertebra L2: posicion 3 - (Arduino posicion 1)
        /// </summary>
        public Vertebra L2;

        /// <summary>
        /// Arriba: posicion 4 - (Arduino posicion 2)
        /// Abajo: posicion 5 - (Arduino posicion 3)
        /// </summary>
        public VertebraL3 L3;

        /// <summary>
        /// Arriba-Izquierda: posicion 6 - (Arduino posicion 4)
        /// Arriba-Derecha: posicion 7 - (Arduino posicion 5)
        /// Arriba-Centro: posicion 8 - (Arduino posicion 6)
        /// Abajo: posicion 9 - (Arduino posicion 7)
        /// </summary>
        public VertebraL4 L4;

        /// <summary>
        /// Vertebra L5: posicion 10 - (Arduino posicion 8)
        /// </summary>
        public Vertebra L5;

        /// <summary>
        /// Duramadre: posicion 11 - (Arduino posicion 9)
        /// </summary>
        public Capa Duramadre;

        public bool RozandoAhora { get; internal set; }
        public bool AtravezandoAhora { get; internal set; }

        public CambioSIEventArgs(Capa tejidoAdiposo, Vertebra L2, VertebraL3 L3, VertebraL4 L4, Vertebra L5, Capa Duramadre) {
            this.TejidoAdiposo = tejidoAdiposo;
            this.L2 = L2;
            this.L3 = L3;
            this.L4 = L4;
            this.L5 = L5;
            this.Duramadre = Duramadre;
        }
        
    }
}
