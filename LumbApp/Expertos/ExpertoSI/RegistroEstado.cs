﻿using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoSI {
    public class RegistroEstado {
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

        public RegistroEstado () {
            this.TejidoAdiposo = new Capa();
            this.L2 = new Vertebra();
            this.L3Arriba = new Vertebra();
            this.L3Abajo = new Vertebra();
            this.L4ArribaIzq = new Vertebra();
            this.L4ArribaDer = new Vertebra();
            this.L4ArribaCentro = new Vertebra();
            this.L4Abajo = new Vertebra();
            this.L5 = new Vertebra();
            this.Duramadre = new Capa();
        }
        
    }
}
