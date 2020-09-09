using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI {
    public class ExpertoSIMock {
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

        private CambioSIEventArgs args;

        public event EventHandler<CambioSIEventArgs> CambioSI;

        public ExpertoSIMock (bool shouldInit, IConectorSI conector) {
            this.shouldInit = shouldInit;
            sensoresInternos = new ConectorSIMock(shouldInit);
            VecesCaminoCorrecto = 0;
            VecesCaminoIncorrecto = 0;
        }
        public bool Inicializar () {
            sensoresInternos.HayDatos += HayDatosNuevos;
            sensoresInternos.Conectar();

            return sensoresInternos.CheckearComunicacion();
        }

        protected virtual void HayCambioSI (CambioSIEventArgs datosCambioSI) {
            EventHandler<CambioSIEventArgs> handler = CambioSI;
            if (handler != null) {
                handler(this, datosCambioSI);
            }
        }

        public void HayDatosNuevos (object sender, DatosSensadosEventArgs datosNuevos) {
            if (simulando) {
                if (RealizarAcciones(datosNuevos.datosSensados)) {
                    args = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre);
                    HayCambioSI(args);
                }
            }
        }

        public bool IniciarSimulacion () {
            simulando = true;
            sensoresInternos.ActivarSensado();
            /*
            if (shouldInit) {
                //simulateAsync();
            }*/
            return shouldInit;
        }

        public InformeSI TerminarSimulacion () {

            if (!simulando)
                return new InformeSI(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            simulando = false;
            return new InformeSI(TejidoAdiposo.VecesAtravesada, L2.VecesRozada, L3.VecesArriba, L3.VecesAbajo,
                L4.VecesArribaIzquierda, L4.VecesArribaDerecha, L4.VecesArribaCentro, L4.VecesAbajo, L5.VecesRozada,
                Duramadre.VecesAtravesada, VecesCaminoCorrecto, VecesCaminoIncorrecto);
        }
        public void Finalizar () { }

        private void sendEvent () {
            var datosCambioSI = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre);
            EventHandler<CambioSIEventArgs> handler = CambioSI;
            if (handler != null) {
                handler(this, datosCambioSI);
            }
        }

        /// <summary>
        /// simulateAsync es la funcion que simula cambios de estado en vertebras y capas.
        /// Para usarla se debe deshabilitar la funcion simulateAsync de ConectorSIMock.
        /// El caso basico es: 
        ///     1) atraviesa tejido adiposo
        ///     2) roza vertebra L4 arriba centro
        ///     3) atravieza duramadre
        /// </summary>
        /// <returns></returns>
        private async Task simulateAsync () {
            //Arranca simulacion
            simulando = true; 

            //************ A partir de aca escribir toda la simulacion ****************

            //Ej: Atraviesa el tejido adiposo a los 5 segundos
            await Task.Delay(5000);
            TejidoAdiposo.Atravesar();
            sendEvent();

            //Ej: Roza L4-arriba-centro a los 10 segundos
            await Task.Delay(5000);
            L4.RozarSector(VertebraL4.Sectores.ArribaCentro);
            sendEvent();

            //Ej: Atraviesa la duramadre a los 20 segundos
            await Task.Delay(10000);
            Duramadre.Atravesar();
            sendEvent();

            // ************** Fin seccion simulacion *************

            //Termina simulacion
            await Task.Delay(5000);
            Console.WriteLine("Finished mocked simulation");
            simulando = false;
        }

        public bool RealizarAcciones (string datosSensados) {
            bool Cambio = false;

            if (datosSensados[2] == '0') {
                if (TejidoAdiposo.Atravesar())
                    Cambio = true;
            } else {
                if (TejidoAdiposo.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[3] == '0') {
                if (L2.Rozar())
                    Cambio = true;
            } else {
                if (L2.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[4] == '0') {
                if (L3.RozarSector(VertebraL3.Sectores.Arriba))
                    Cambio = true;
            } else {
                if (L3.AbandonarSector(VertebraL3.Sectores.Arriba))
                    Cambio = true;
            }

            if (datosSensados[5] == '0') {
                if (L3.RozarSector(VertebraL3.Sectores.Abajo))
                    Cambio = true;
            } else {
                if (L3.AbandonarSector(VertebraL3.Sectores.Abajo))
                    Cambio = true;
            }

            if (datosSensados[6] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaIzquierda))
                    Cambio = true;
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda))
                    Cambio = true;
            }

            if (datosSensados[7] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaDerecha))
                    Cambio = true;
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaDerecha))
                    Cambio = true;
            }

            if (datosSensados[8] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaCentro))
                    Cambio = true;
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaCentro))
                    Cambio = true;
            }

            if (datosSensados[9] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.Abajo))
                    Cambio = true;
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.Abajo))
                    Cambio = true;
            }

            if (datosSensados[10] == '0') {
                if (L5.Rozar())
                    Cambio = true;
            } else {
                if (L5.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[11] == '0') {
                if (Duramadre.Atravesar())
                    Cambio = true;
            } else {
                if (Duramadre.Abandonar())
                    Cambio = true;
            }

            return Cambio;
        }
    }
}
