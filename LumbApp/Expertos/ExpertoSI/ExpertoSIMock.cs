using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI {
    class ExpertoSIMock {
        private IConectorSI sensoresInternos;
        private bool simulando = false;
        private bool shouldInit;
        
        public Capa TejidoAdiposo = new Capa();
        public Vertebra L2 = new Vertebra();
        public VertebraL3 L3 = new VertebraL3();
        public VertebraL4 L4 = new VertebraL4();
        public Vertebra L5 = new Vertebra();
        public Capa Duramadre = new Capa();

        public int VecesCaminoCorrecto;
        public int VecesCaminoIncorrecto;

        public event EventHandler<CambioSIEventArgs> CambioSI;

        public ExpertoSIMock(bool shouldInit, IConectorSI conector) {

            this.shouldInit = shouldInit;
            VecesCaminoCorrecto = 0;
            VecesCaminoIncorrecto = 0;
        }
        public bool Inicializar () {

            //return sensoresInternos.ChekearSensado();
            return shouldInit;
        }
        public bool IniciarSimulacion () {
            simulando = true;
            sensoresInternos.ActivarSensado();

            if (shouldInit)
                simulateAsync();
            return shouldInit;
        }

        public InformeSI TerminarSimulacion () {

            if (!simulando)
                return new InformeSI(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            simulando = false;
            return new InformeSI(TejidoAdiposo.VecesAtravesada, L2.VecesRozada, L3.VecesArriba, L3.VecesAbajo, 
                L4.VecesArribaIzquierda, L4.VecesArribaDerecha, L4.VecesArribaCentro, L4.VecesAbajo, L5.VecesRozada,
                Duramadre.VecesAtravesada, VecesCaminoCorrecto, VecesCaminoIncorrecto) ;
        }
        public void Finalizar () { }

        private void sendEvent () {
            var args = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre);
            CambioSI.Invoke(this, args);
        }

        private async Task simulateAsync () {
            //Arranca simulacion
            simulando = true;
            
            //************ A partir de aca escribir toda la simulacion ****************

            //Ej: mano derecha se trackea a los 5 segundos
            await Task.Delay(5000);
            TejidoAdiposo.Atravesar();
            sendEvent();

            //Ej: entra a los 10 segundos
            await Task.Delay(5000);
            L4.RozarSector(VertebraL4.Sectores.ArribaCentro);
            sendEvent();

            //Ej: sale a los 20 segundos
            await Task.Delay(10000);
            Duramadre.Atravesar();
            sendEvent();

            // ************** Fin seccion simulacion *************

            //Termina simulacion
            await Task.Delay(5000);
            Console.WriteLine("Finished mocked simulation");
            simulando = false;
        }
    }
}
