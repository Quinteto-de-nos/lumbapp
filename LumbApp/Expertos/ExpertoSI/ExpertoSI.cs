using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoSI {
    public class ExpertoSI : IExpertoSI {
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
        private bool _comunicacionCheckeada = false;

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

            return _comunicacionCheckeada;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IniciarSimulacion () {
            if (_comunicacionCheckeada) {
                simulando = true;
                sensoresInternos.ActivarSensado();
            }

            return simulando;
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
            if (!simulando)
                return new InformeSI(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            simulando = false;

            sensoresInternos.PausarSensado();

            InformeSI informe = new InformeSI(TejidoAdiposo.VecesAtravesada, L2.VecesRozada, L3.VecesArriba, L3.VecesAbajo,
                L4.VecesArribaIzquierda, L4.VecesArribaDerecha, L4.VecesArribaCentro, L4.VecesAbajo, L5.VecesRozada,
                Duramadre.VecesAtravesada, VecesCaminoCorrecto, VecesCaminoIncorrecto);

            Resetear();

            return informe;
        }

        public void Resetear () {
            TejidoAdiposo.Resetear();
            L2.Resetear();
            L3.Resetear();
            L4.Resetear();
            L5.Resetear();
            Duramadre.Resetear();
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
            if (simulando) {
                if (RealizarAcciones(datosSensados.datosSensados)) {
                    args = new CambioSIEventArgs(TejidoAdiposo, L2, L3, L4, L5, Duramadre);
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
            if (datosSensados.Count() == 12) {
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
            }

            return Cambio;
        }
    }
}
