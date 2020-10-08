using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI.Utils {
    public class VertebraL3 : Vertebra {
        public  enum Sectores {

            [Display(Name = "Arriba")]
            Arriba,

            [Display(Name = "Abajo")]
            Abajo
        }

        public Sectores Sector { get; private set; }

        public int VecesArriba { get; private set; }
        public int VecesAbajo { get; private set; }

        public VertebraL3() : base() {
            VecesArriba = 0;
            VecesAbajo = 0;
        }
        /// <summary>
        /// Sirve para la vertebra L3 sectorizada.
        /// Setea al sector de la vertebra que esta siendo rozado.
        /// </summary>
        /// <returns>Si hubo cambio de estado retorna true.</returns>
        public bool RozarSector(Sectores sector) {
            
            if (Rozar()) {
                this.Sector = sector;

                switch (Sector) {
                    case Sectores.Arriba:
                        VecesArriba++;
                        break;
                    case Sectores.Abajo:
                        VecesAbajo++;
                        break;
                }
                return true;
            }

            return false;
        }

        public bool AbandonarSector (Sectores sector) {
            if (this.Sector == sector)
                return base.Abandonar();
            return false;
        }

        public new void Resetear () {
            VecesArriba = 0;
            VecesAbajo = 0;
            base.Resetear();
        }
    }
}
