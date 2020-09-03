using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoSI {
    public class RegistroEstado {
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
        public Vertebra L3;

        /// <summary>
        /// Arriba-Izquierda: posicion 6 - (Arduino posicion 4)
        /// Arriba-Derecha: posicion 7 - (Arduino posicion 5)
        /// Arriba-Centro: posicion 8 - (Arduino posicion 6)
        /// Abajo: posicion 9 - (Arduino posicion 7)
        /// </summary>
        public Vertebra L4;

        /// <summary>
        /// Vertebra L5: posicion 10 - (Arduino posicion 8)
        /// </summary>
        public Vertebra L5;

        /// <summary>
        /// Duramadre: posicion 11 - (Arduino posicion 9)
        /// </summary>
        public Capa Duramadre;

        public RegistroEstado () {
            this.TejidoAdiposo = new Capa();
            this.L2 = new Vertebra();
            this.L3 = new VertebraL3();
            this.L4 =  new VertebraL4();
            this.L5 = new Vertebra();
            this.Duramadre = new Capa();
        }
        
    }
}
