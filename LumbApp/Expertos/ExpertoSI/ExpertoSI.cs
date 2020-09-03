using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoSI {
    public class ExpertoSI {
        private IConectorSI sensoresInternos;

        public Capa TejidoAdiposo = new Capa();
        public Vertebra L2 = new Vertebra();
        public VertebraL3 L3 = new VertebraL3();
        public VertebraL4 L4 = new VertebraL4();
        public Vertebra L5 = new Vertebra();
        public Capa Duramadre = new Capa();

        public int VecesCaminoCorrecto;
        public int VecesCaminoIncorrecto;

        protected RegistroEstado registroEstadoActual;
        protected RegistroEstado registroEstadoAnterior;

        private bool simulando = false;

        CambioSIEventArgs args;

        public ExpertoSI (IConectorSI sensoresInternos) {
            if (sensoresInternos == null)
                throw new Exception("Sensores no puede ser null. Necesito un conector a los sensores para crear un experto en sensores internos");
            this.sensoresInternos = sensoresInternos;
        }

        public bool Inicializar () {
            registroEstadoActual = new RegistroEstado();
            registroEstadoAnterior = new RegistroEstado();

            //args = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre);

            //sensoresInternos.HayDatos += HayDatosNuevos; //suscripción al evento HayDatos
            sensoresInternos.Conectar();

            if (!sensoresInternos.ChekearSensado())
                return false;
            
            return true;
        }

        public bool IniciarSimulacion () {
            return true;
        }

        public void Finalizar () {

        }

        protected virtual void HayCambioSI (CambioSIEventArgs e) {
            EventHandler<CambioSIEventArgs> handler = CambioSI;
            if (handler != null) {
                handler(this, e);
            }
        }

        public event EventHandler<CambioSIEventArgs> CambioSI;

        public InformeSI TerminarSimulacion () {
            InformeSI informe = new InformeSI(TejidoAdiposo.VecesAtravesada, L2.VecesRozada, L3.VecesArriba, L3.VecesAbajo,
                L4.VecesArribaIzquierda, L4.VecesArribaDerecha, L4.VecesArribaCentro, L4.VecesAbajo, L5.VecesRozada,
                Duramadre.VecesAtravesada, VecesCaminoCorrecto, VecesCaminoIncorrecto);
            return informe;
        }

        public RegistroEstado GetRegistroEstadoActual () {
            return registroEstadoActual;
        }



       /// <summary>
       /// Acciona cada capa y vertebra de acuerdo a los datos sensados recibidos.
       /// Cada capa o vertebra se encarga de actualizar su estado segun corresponda.
       /// </summary>
       /// <param name="datosSensados"> string que contiene los datos sensados. </param>
       /// <returns> Retorna true si hubo un cambio de estado en al menos alguna vertebra o capa. </returns>
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
                if (L3.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[5] == '0') {
                if (L3.RozarSector(VertebraL3.Sectores.Abajo))
                    Cambio = true;
            } else {
                if (L3.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[6] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaIzquierda))
                    Cambio = true;
            } else {
                if (L4.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[7] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaDerecha))
                    Cambio = true;
            } else {
                if (L4.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[8] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaCentro))
                    Cambio = true;
            } else {
                if (L4.Abandonar())
                    Cambio = true;
            }

            if (datosSensados[9] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.Abajo))
                    Cambio = true;
            } else {
                if (L4.Abandonar())
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
