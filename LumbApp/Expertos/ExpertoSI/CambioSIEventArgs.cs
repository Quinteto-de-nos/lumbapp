﻿using LumbApp.Expertos.ExpertoSI.Utils;
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
        public Capa TejidoAdiposo { get; private set; }
        public bool TejidoAdiposoAtravezandoAhora { get; set; }

        /// <summary>
        /// Vertebra L2: posicion 3 - (Arduino posicion 1)
        /// </summary>
        public Vertebra L2 { get; private set; }
        public bool L2RozandoAhora { get; set; }

        /// <summary>
        /// Arriba: posicion 4 - (Arduino posicion 2)
        /// Abajo: posicion 5 - (Arduino posicion 3)
        /// </summary>
        public VertebraL3 L3 { get; private set; }
        public bool L3RozandoAhora { get; set; }

        /// <summary>
        /// Arriba-Izquierda: posicion 6 - (Arduino posicion 4)
        /// Arriba-Derecha: posicion 7 - (Arduino posicion 5)
        /// Arriba-Centro: posicion 8 - (Arduino posicion 6)
        /// Abajo: posicion 9 - (Arduino posicion 7)
        /// </summary>
        public VertebraL4 L4 { get; private set; }
        public bool L4RozandoAhora { get; set; }

        /// <summary>
        /// Vertebra L5: posicion 10 - (Arduino posicion 8)
        /// </summary>
        public Vertebra L5 { get; private set; }
        public bool L5RozandoAhora { get; set; }

        /// <summary>
        /// Duramadre: posicion 11 - (Arduino posicion 9)
        /// </summary>
        public Capa Duramadre { get; private set; }
        public bool DuramadreAtravezandoAhora { get; set; }

        public bool CaminoIncorrecto { get; private set; }

        public CambioSIEventArgs(Capa tejidoAdiposo, Vertebra L2, VertebraL3 L3, VertebraL4 L4, Vertebra L5, Capa Duramadre,
            bool AhoraTejidoAdiposo, bool AhoraL2, bool AhoraL3, bool AhoraL4, bool AhoraL5, bool AhoraDuramadre,
            bool CaminoIncorrecto) {

            this.TejidoAdiposo = tejidoAdiposo;
            this.L2 = L2;
            this.L3 = L3;
            this.L4 = L4;
            this.L5 = L5;
            this.Duramadre = Duramadre;

            this.TejidoAdiposoAtravezandoAhora = AhoraTejidoAdiposo;
            this.L2RozandoAhora = AhoraL2;
            this.L3RozandoAhora = AhoraL3;
            this.L4RozandoAhora = AhoraL4;
            this.L5RozandoAhora = AhoraL5;
            this.DuramadreAtravezandoAhora = AhoraDuramadre;

            this.CaminoIncorrecto = CaminoIncorrecto;
        }
        
    }
}
