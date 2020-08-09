using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoSI {
    public class ExpertoSI {
        private IConectorSI sensoresInternos;

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

            args = new CambioSIEventArgs();

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

        protected virtual void SiHayCambioSI (CambioSIEventArgs e) {
            EventHandler<CambioSIEventArgs> handler = CambioSI;
            if (handler != null) {
                handler(this, e);
            }
        }

        public event EventHandler<CambioSIEventArgs> CambioSI;

        public InformeSI TerminarSimulacion () {
            InformeSI informe = new InformeSI();
            return informe;
        }

        public RegistroEstado GetRegistroEstadoActual () {
            return registroEstadoActual;
        }
    }
}
