using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Conectores.ConectorSI {
    public class ConectorSIMock : IConectorSI {
        private bool sensando = false;
        private bool shouldInit;

        /// <summary>
        /// Crea el Conector Mockeado.
        /// </summary>
        /// <param name="shouldInit"> Recibe un booleano que de indica si debe iniciar bien o no.</param>
        public ConectorSIMock(bool shouldInit) { this.shouldInit = shouldInit; }
        public void Conectar () {}
        public void ActivarSensado () { sensando = true; }
        public void Desconectar () {}
        public void PausarSensado () { sensando = false; }

        public bool ChekearSensado () {
            for (int i = 0; i < 5; i++) {
                Task.Delay(1000);
            }
            return shouldInit;
        }

        private async Task simulateAsync () {
            //Arranca simulacion

            //************ A partir de aca escribir toda la simulacion ****************

        }
    }
}
