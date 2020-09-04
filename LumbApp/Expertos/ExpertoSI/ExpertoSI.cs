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

        private bool simulando = false;

        CambioSIEventArgs args;

        public ExpertoSI (IConectorSI sensoresInternos) {
            if (sensoresInternos == null)
                throw new Exception("Sensores no puede ser null. Necesito un conector a los sensores para crear un experto en sensores internos");
            this.sensoresInternos = sensoresInternos;
        }

        public bool Inicializar () {
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

    }
}
