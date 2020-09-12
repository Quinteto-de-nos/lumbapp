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

        private bool AhoraTejidoAdiposo = false;
        private bool AhoraL2 = false;
        private bool AhoraL3 = false;
        private bool AhoraL4 = false;
        private bool AhoraL5 = false;
        private bool AhoraDuramadre = false;

        private int VecesCaminoCorrecto = 0;
        private int VecesCaminoIncorrecto = 0;

        private bool _simulando = false;
        private bool _comunicacionCheckeada = false;

        private bool CaminoIncorrecto = false;

        CambioSIEventArgs args;

        public ExpertoSI (IConectorSI sensoresInternos) {
            if (sensoresInternos == null)
                throw new Exception("Sensores no puede ser null. Necesito un conector a los sensores para crear un experto en sensores internos");
            this.sensoresInternos = sensoresInternos;
        }

        /// <summary>
        /// Método que inicializa las variables, incluyendo el conector.
        /// </summary>
        /// <returns> Si todo sale bien, devuelve true sino devuelve false. </returns>
        public bool Inicializar () {
            sensoresInternos.HayDatos += HayDatosNuevos; //suscripción al evento HayDatos
            try {
                if (sensoresInternos.Conectar())
                    _comunicacionCheckeada = sensoresInternos.CheckearComunicacion();
            } catch {
                return false;
            }
            _comunicacionCheckeada = sensoresInternos.CheckearComunicacion();
            
            return _comunicacionCheckeada;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IniciarSimulacion () {
            if (_comunicacionCheckeada) {
                _simulando = true;
                sensoresInternos.ActivarSensado();
            }

            return _simulando;
        }

        /// <summary>
        /// Finaliza el sensado.
        /// </summary>
        public void Finalizar () {
            if (_comunicacionCheckeada) {
                _comunicacionCheckeada = false;
                sensoresInternos.Desconectar();
            }
        }

        public InformeSI TerminarSimulacion () {
            if (!_simulando)
                return new InformeSI(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            _simulando = false;

            sensoresInternos.PausarSensado();

            InformeSI informe = new InformeSI(TejidoAdiposo.VecesAtravesada, L2.VecesRozada, L3.VecesArriba, L3.VecesAbajo,
                L4.VecesArribaIzquierda, L4.VecesArribaDerecha, L4.VecesArribaCentro, L4.VecesAbajo, L5.VecesRozada,
                Duramadre.VecesAtravesada, VecesCaminoCorrecto, VecesCaminoIncorrecto);

            Resetear();

            return informe;
        }

        /// <summary>
        /// Vuelve Capas, Vertebras y estadisticas al estado inicial.
        /// </summary>
        public void Resetear () {
            TejidoAdiposo.Resetear();
            L2.Resetear();
            L3.Resetear();
            L4.Resetear();
            L5.Resetear();
            Duramadre.Resetear();

            VecesCaminoCorrecto = 0;
            VecesCaminoIncorrecto = 0;
        }

        /// <summary>
        /// Evento que le indica al orquestador cuando se produce un cambio en el estado de algun elemento sensado.
        /// </summary>
        public event EventHandler<CambioSIEventArgs> CambioSI;

        /// <summary>
        /// Método que levanta el  evento CambioSI.
        /// </summary>
        /// <param name="datosCambioSI">
        /// Recibe los datos del Regsitro de Estado actual y el anterior a traves de la clase CambioSIEventArgs.
        /// </param>
        protected virtual void HayCambioSI (CambioSIEventArgs datosCambioSI) {
            EventHandler<CambioSIEventArgs> handler = CambioSI;
            if (handler != null) {
                handler(this, datosCambioSI);
            }
        }

        /// <summary>
        /// Método que recibe los nuevos datos que le pasó el conector.
        /// Acá se analiza la información para detectar si hubo algún cambio en el sensado.
        ///  - Si hubo cambio, guarda los datos del cambio y llama a la función SiHayCambio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="datosSensados"></param>
        public void HayDatosNuevos (object sender, DatosSensadosEventArgs datosSensados) {
            if (_simulando) {
                if (RealizarAcciones(datosSensados.datosSensados)) {
                    args = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre, 
                        AhoraTejidoAdiposo, AhoraL2, AhoraL3, AhoraL4, AhoraL5, AhoraDuramadre,
                        CaminoIncorrecto);

                    AhoraTejidoAdiposo = false;
                    AhoraL2 = false;
                    AhoraL3 = false;
                    AhoraL4 = false;
                    AhoraL5 = false;
                    AhoraDuramadre = false;

                    HayCambioSI(args);
                }
            }
        }

        /// <summary>
        /// Acciona cada capa y vertebra de acuerdo a los datos sensados recibidos.
        /// Cada capa o vertebra se encarga de actualizar su estado segun corresponda.
        /// </summary>
        /// <param name="datosSensados"> string que contiene los datos sensados. </param>
        /// <returns> Retorna true si hubo un cambio de estado en al menos alguna vertebra o capa. </returns>
        public bool RealizarAcciones (string datosSensados) {
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

            //Camino INCORRECTO - L4 ARRIBA IZQUIERDA ---------------------------- NO SE SI ES INCORRECTO
            if (datosSensados[6] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaIzquierda)) {
                    AhoraL4 = true;
                    CaminoIncorrecto = true; ///NO SE SI CONSIDERARLO UN ERROR
                }
            } else {
                if (L4.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda))
                    AhoraL4 = true;
            }
            //Camino INCORRECTO - L4 ARRIBA DERECHA ---------------------------- NO SE SI ES INCORRECTO
            if (datosSensados[7] == '0') {
                if (L4.RozarSector(VertebraL4.Sectores.ArribaDerecha)) {
                    AhoraL4 = true;
                    CaminoIncorrecto = true; ///NO SE SI CONSIDERARLO UN ERROR
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

            //Camino CORRECTO - TEJIDO ADIPOSO
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
