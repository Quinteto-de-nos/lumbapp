using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoSI {
    public class ExpertoSIMock : IExpertoSI{
        private IConectorSI sensoresInternos;
        private bool simulando = false;
        private bool shouldInit;

        public Capa TejidoAdiposo = new Capa();
        public Vertebra L2 = new Vertebra();
        public VertebraL3 L3 = new VertebraL3();
        public VertebraL4 L4 = new VertebraL4();
        public Vertebra L5 = new Vertebra();
        public Capa Duramadre = new Capa();

        private bool AhoraTejidoAdiposo = false;
        private bool AhoraL2 = false;
        private bool AhoraL3 = false;
        private bool AhoraL4 = false;
        private bool AhoraL5 = false;
        private bool AhoraDuramadre = false;

        private bool CaminoIncorrecto = false;

        public int VecesCaminoCorrecto = 0;
        public int VecesCaminoIncorrecto = 0;

        private CambioSIEventArgs args;

        public event EventHandler<CambioSIEventArgs> CambioSI;

        public ExpertoSIMock (bool shouldInit) {
            this.shouldInit = shouldInit;
            sensoresInternos = new ConectorSIMock(shouldInit);
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
                    args = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre,
                        AhoraTejidoAdiposo, AhoraL2, AhoraL3, AhoraL4, AhoraL5, AhoraDuramadre, 
                        CaminoIncorrecto);

                    ResetearAhora();

                    HayCambioSI(args);
                }
            }
        }

        public void ResetearAhora () {
            AhoraTejidoAdiposo = false;
            AhoraL2 = false;
            AhoraL3 = false;
            AhoraL4 = false;
            AhoraL5 = false;
            AhoraDuramadre = false;
        }

        public bool IniciarSimulacion () {
            simulando = true;
            sensoresInternos.ActivarSensado();

            if (shouldInit)
            {
                simulateAsync();
            }
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
            var datosCambioSI = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre, 
                AhoraTejidoAdiposo, AhoraL2, AhoraL3, AhoraL4, AhoraL5, AhoraDuramadre,
                CaminoIncorrecto);

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

            await Task.Delay(5000);
            AhoraL4 = true;
            L4.RozarSector(VertebraL4.Sectores.Abajo);
            sendEvent();

            await Task.Delay(5000);
            AhoraL4 = false;
            AhoraL3 = true;
            L3.RozarSector(VertebraL3.Sectores.Abajo);
            sendEvent();

            await Task.Delay(5000);
            L4.AbandonarSector(VertebraL4.Sectores.Abajo);
            AhoraL3 = false;
            AhoraL4 = true;
            L4.RozarSector(VertebraL4.Sectores.ArribaIzquierda);
            sendEvent();

            await Task.Delay(5000);
            AhoraL4 = false;
            AhoraL3 = true;
            L3.AbandonarSector(VertebraL3.Sectores.Abajo);
            sendEvent();

            //Ej: Roza L4-arriba-centro a los 10 segundos
            await Task.Delay(5000);
            L4.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            AhoraL3 = false;
            AhoraL4 = true;
            L4.RozarSector(VertebraL4.Sectores.ArribaCentro);
            sendEvent();

            //Ej: Atraviesa la duramadre a los 20 segundos
            await Task.Delay(10000);
            AhoraL4 = false;
            Duramadre.Atravesar();
            sendEvent();

            await Task.Delay(10000);
            Duramadre.Abandonar();
            sendEvent();

            await Task.Delay(5000);
            L4.Abandonar();
            sendEvent();

            await Task.Delay(5000);
            TejidoAdiposo.Abandonar();
            sendEvent();

            // ************** Fin seccion simulacion *************

            //Termina simulacion
            await Task.Delay(5000);
            Console.WriteLine("Finished mocked simulation");
            simulando = false;
        }

        /// <summary>
        /// Acciona cada capa y vertebra de acuerdo a los datos sensados recibidos.
        /// Cada capa o vertebra se encarga de actualizar su estado segun corresponda.
        /// </summary>
        /// <param name="datosSensados"> string que contiene los datos sensados. </param>
        /// <returns> Retorna true si hubo un cambio de estado en al menos alguna vertebra o capa. </returns>
        private bool RealizarAcciones (string datosSensados) {
            bool Cambio = false;
            bool CaminoCorrecto = false;
            CaminoIncorrecto = false;

            //Camino CORRECTO - TEJIDO ADIPOSO
            if (datosSensados[2] == '0') {
                if (TejidoAdiposo.Atravesar()) {
                    AhoraTejidoAdiposo = true;
                    CaminoCorrecto = true;
                }
            } else {
                if (TejidoAdiposo.Abandonar())
                    AhoraTejidoAdiposo = true;
            }

            //Camino INCORRECTO - L2
            if (datosSensados[3] == '0') {
                if (L2.Rozar()) {
                    AhoraL2 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L2.Abandonar())
                    AhoraL2 = true;
            }

            //Camino INCORRECTO - L3 ARRIBA
            if (datosSensados[4] == '0') {
                if (L3.RozarSector(VertebraL3.Sectores.Arriba)) {
                    AhoraL3 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L3.AbandonarSector(VertebraL3.Sectores.Arriba))
                    AhoraL3 = true;
            }
            //Camino INCORRECTO - L3 ABAJO
            if (datosSensados[5] == '0') {
                if (L3.RozarSector(VertebraL3.Sectores.Abajo)) {
                    AhoraL3 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L3.AbandonarSector(VertebraL3.Sectores.Abajo))
                    AhoraL3 = true;
            }

            //Camino INCORRECTO - L4 ARRIBA IZQUIERDA
            if (datosSensados[6] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaIzquierda)) {
                    AhoraL4 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda))
                    AhoraL4 = true;
            }
            //Camino INCORRECTO - L4 ARRIBA DERECHA 
            if (datosSensados[7] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaDerecha)) {
                    AhoraL4 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaDerecha))
                    AhoraL4 = true;
            }
            //Camino CORRECTO - L4 ARRIBA CENTRO
            if (datosSensados[8] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaCentro)) {
                    AhoraL4 = true;
                    CaminoCorrecto = true;
                }
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaCentro))
                    AhoraL4 = true;
            }
            //Camino INCORRECTO - L4 ABAJO
            if (datosSensados[9] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.Abajo)) {
                    AhoraL4 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.Abajo))
                    AhoraL4 = true;
            }

            //Camino INCORRECTO - L5
            if (datosSensados[10] == '0') {
                if (L5.Rozar()) {
                    AhoraL5 = true;
                    CaminoIncorrecto = true;
                }
            } else {
                if (L5.Abandonar())
                    AhoraL5 = true;
            }

            //Camino CORRECTO - DURAMADRE
            if (datosSensados[11] == '0') {
                if (Duramadre.Atravesar()) {
                    AhoraDuramadre = true;
                    CaminoCorrecto = true;
                }
            } else {
                if (Duramadre.Abandonar())
                    AhoraDuramadre = true;
            }

            if (AhoraTejidoAdiposo || AhoraL2 || AhoraL3 || AhoraL4 || AhoraL5 || AhoraDuramadre)
                Cambio = true;

            if (CaminoIncorrecto)
                VecesCaminoIncorrecto++;
            else if (CaminoCorrecto && (TejidoAdiposo.Estado == Capa.Estados.AtravesandoNuevamente ||
                L4.Estado == VertebraL4.Estados.RozandoNuevamente ||
                Duramadre.Estado == Capa.Estados.AtravesandoNuevamente || Duramadre.Estado == Capa.Estados.Atravesando)) {

                VecesCaminoCorrecto++;
            }

            return Cambio;
        }
    }
}
