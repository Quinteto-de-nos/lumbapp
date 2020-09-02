using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI {
    public class CambioSIEventArgs : EventArgs {
        public Capa TejidoAdiposo; //posicion 2 - (Arduino posicion 0)

        public Vertebra L2; //posicion 3 - (Arduino posicion 1)

        public Vertebra L3Arriba; //posicion 4 - (Arduino posicion 2)
        public Vertebra L3Abajo; //posicion 5 - (Arduino posicion 3)

        public Vertebra L4ArribaIzq; //posicion 6 - (Arduino posicion 4)
        public Vertebra L4ArribaDer; //posicion 7 - (Arduino posicion 5)
        public Vertebra L4ArribaCentro; //posicion 8 - (Arduino posicion 6)
        public Vertebra L4Abajo; //posicion 9 - (Arduino posicion 7)

        public Vertebra L5; //posicion 10 - (Arduino posicion 8)

        public Capa Duramadre; //posicion 11 - (Arduino posicion 9)

        public bool RozandoAhora { get; internal set; }
        public bool AtravezandoAhora { get; internal set; }

        public CambioSIEventArgs(RegistroEstado re) {
            this.reActual = re;
        }
        
    }
}
