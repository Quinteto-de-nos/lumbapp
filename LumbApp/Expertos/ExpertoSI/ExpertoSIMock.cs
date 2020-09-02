using LumbApp.Conectores.ConectorSI;
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
        private bool primeraVezPunzado;

        RegistroEstado re = new RegistroEstado(false, false, false, false, false, false, false, false, false, false);

        public event EventHandler<CambioSIEventArgs> CambioSI;

        public ExpertoSIMock(bool shouldInit) {
            this.shouldInit = shouldInit;
        }
        public bool Inicializar () {
            //return sensoresInternos.ChekearSensado();
            return shouldInit;
        }
        public bool IniciarSimulacion () {
            simulando = true;
            sensoresInternos.ActivarSensado();
            primeraVezPunzado = true;

            if (shouldInit)
                simulateAsync();
            return shouldInit;
        }

        public InformeSI TerminarSimulacion () {

            if (!simulando)
                return new InformeZE(0, 0, 0);

            simulando = false;
            return new InformeZE(zonaEsteril.Contaminacion, manoDerecha.VecesContamino, manoIzquierda.VecesContamino);
        }
        public void Finalizar () { }

        private void sendEvent () {
            var args = new CambioSIEventArgs(re);
            CambioSI.Invoke(this, args);
        }

        private async Task simulateAsync () {
            //Arranca simulacion
            simulando = true;
            
            //************ A partir de aca escribir toda la simulacion ****************

            //Ej: mano derecha se trackea a los 5 segundos
            await Task.Delay(5000);
            re.TejidoAdiposo = true;
            sendEvent();

            //Ej: entra a los 10 segundos
            await Task.Delay(5000);
            re.
            sendEvent();

            //Ej: sale a los 20 segundos
            await Task.Delay(10000);
            manoDerecha.Salir();
            sendEvent();

            //Ej: entra y contamina a los 30 segundos (sendEvent no marca el contaminando ahora)
            await Task.Delay(10000);
            manoDerecha.Entrar();
            zonaEsteril.Contaminar();
            var args = new CambioZEEventArgs(manoDerecha, manoIzquierda);
            args.VecesContaminado = zonaEsteril.Contaminacion;
            args.ContaminadoAhora = true;
            CambioZE.Invoke(this, args);

            // ************** Fin seccion simulacion *************

            //Termina simulacion
            await Task.Delay(10000);
            Console.WriteLine("Finished mocked simulation");
            simulando = false;
        }
    }
}
