using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI.Utils {
    public class VertebraL4 : Vertebra {
        public enum Sectores {
            ArribaIzquierda,
            ArribaDerecha,
            ArribaCentro,
            Abajo
        }

        public Sectores Sector { get; private set; }

        public int VecesArribaIzquierda { get; private set; }
        public int VecesArribaDerecha { get; private set; }
        public int VecesArribaCentro { get; private set; }
        public int VecesAbajo { get; private set; }

        public VertebraL4 () : base() {
            VecesArribaIzquierda = 0;
            VecesArribaDerecha = 0;
            VecesArribaCentro = 0;
            VecesAbajo = 0;
        }

        /// <summary>
        /// Sirve para la vertebra L4 sectorizada.
        /// Setea al sector de la vertebra que esta siendo rozado.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool RozarSector (Sectores sector) {
            this.Sector = sector;

            switch (Sector) {
                case Sectores.ArribaIzquierda:
                    VecesArribaIzquierda++;
                    break;
                case Sectores.ArribaDerecha:
                    VecesArribaDerecha++;
                    break;
                case Sectores.ArribaCentro:
                    VecesArribaCentro++;
                    break;
                case Sectores.Abajo:
                    VecesAbajo++;
                    break;
            }

            return base.Rozar();
        }

        public bool AbandonarSector (Sectores sector) {
            if (this.Sector == sector)
                return base.Abandonar();
            return false;
        }
    }
}
