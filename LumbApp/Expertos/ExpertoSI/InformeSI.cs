using LumbApp.Expertos.ExpertoSI.Utils;

namespace LumbApp.Expertos.ExpertoSI {
    public class InformeSI {

        public int TejidoAdiposo { get; private set; }
        public int L2 { get; private set; }
        public int L3Arriba { get; private set; }
        public int L3Abajo { get; private set; }
        public int L4ArribaIzquierda { get; private set; }
        public int L4ArribaDerecha { get; private set; }
        public int L4ArribaCentro { get; private set; }
        public int L4Abajo { get; private set; }
        public int L5 { get; private set; }
        public int Duramadre { get; private set; }

        /// <summary>
        /// Cantidad de veces que fue por un camino que no debia.
        /// Ej.: Toco una vertebra incorrecta.
        /// </summary>
        public int CaminoCorrecto;
        /// <summary>
        /// Cantidad de veces que se logro llegar a la duramadre sin sensar ningun error.
        /// </summary>
        public int CaminoIncorrecto;
        
        public InformeSI(int TejidoAdiposo, int L2, int L3Arriba, int L3Abajo, int L4ArribaIzquierda, int L4ArribaDerecha, 
            int L4ArribaCentro, int L4Abajo, int L5, int Duramadre, int CaminoCorrecto, int CaminoIncorrecto) {

            this.TejidoAdiposo = TejidoAdiposo;
            this.L2 = L2;
            this.L3Arriba = L3Arriba;
            this.L3Abajo = L3Abajo;
            this.L4ArribaIzquierda = L4ArribaIzquierda;
            this.L4ArribaDerecha = L4ArribaDerecha;
            this.L4ArribaCentro = L4ArribaCentro;
            this.L4Abajo = L4Abajo;
            this.L5 = L5;
            this.Duramadre = Duramadre;
            this.CaminoCorrecto = CaminoCorrecto;
            this.CaminoIncorrecto = CaminoIncorrecto;
        }
    }
}