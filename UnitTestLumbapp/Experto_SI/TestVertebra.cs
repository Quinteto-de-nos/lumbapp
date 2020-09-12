using LumbApp.Expertos.ExpertoSI.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestLumbapp.Experto_SI {
    [TestClass]
    public class TestVertebra {
        /// <summary>
        /// La Vertebra debe cambiar de estado con Rozar() y Abandonar().
        /// Los estados son Inicial, Rozando, Abandonada y RozandoNuevamente e incrementar el contador de veces rozada
        /// cuando hay un cambio de Inicial a Rozando o de Abandonada a RozandoNuevamente.
        /// </summary>
        [TestMethod]
        public void TestEstadosYVecesVertebra () {
            Vertebra vertebra = new Vertebra();
            bool cambio;

            Assert.AreEqual(Vertebra.Estados.Inicial, vertebra.Estado);

            cambio = vertebra.Abandonar();
            Assert.AreEqual(Vertebra.Estados.Inicial, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(0, vertebra.VecesRozada);

            cambio = vertebra.Rozar();
            Assert.AreEqual(Vertebra.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.Rozar();
            Assert.AreEqual(Vertebra.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.Abandonar();
            Assert.AreEqual(Vertebra.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.Abandonar();
            Assert.AreEqual(Vertebra.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.Rozar();
            Assert.AreEqual(Vertebra.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.Rozar();
            Assert.AreEqual(Vertebra.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.Abandonar();
            Assert.AreEqual(Vertebra.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.Rozar();
            Assert.AreEqual(Vertebra.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(3, vertebra.VecesRozada);

            vertebra.Resetear();
            Assert.AreEqual(Vertebra.Estados.Inicial, vertebra.Estado);
            Assert.AreEqual(0, vertebra.VecesRozada);
        }

        /// <summary>
        /// La VertebraL3 debe cambiar de estado con Rozar() y Abandonar().
        /// Los estados son Inicial, Rozando, Abandonada y RozandoNuevamente e incrementar el contador de veces rozada
        /// cuando hay un cambio de Inicial a Rozando o de Abandonada a RozandoNuevamente.
        /// </summary>
        [TestMethod]
        public void TestEstadosYVecesVertebraL3 () {
            VertebraL3 vertebra = new VertebraL3();
            bool cambio;

            Assert.AreEqual(VertebraL3.Estados.Inicial, vertebra.Estado);

            cambio = vertebra.AbandonarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(VertebraL3.Estados.Inicial, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(0, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(VertebraL3.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(VertebraL3.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(VertebraL3.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(VertebraL3.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(VertebraL3.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(VertebraL3.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(VertebraL3.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(VertebraL3.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(VertebraL3.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(3, vertebra.VecesRozada);

            vertebra.Resetear();
            Assert.AreEqual(VertebraL3.Estados.Inicial, vertebra.Estado);
            Assert.AreEqual(0, vertebra.VecesRozada);
        }

        [TestMethod]
        public void TestSectoresYVecesVertebraL3 () {
            VertebraL3 vertebra = new VertebraL3();

            Assert.AreEqual(VertebraL3.Estados.Inicial, vertebra.Estado);

            vertebra.AbandonarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(0, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.RozarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.RozarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.RozarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(1, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Abajo, vertebra.Sector);

            vertebra.RozarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(1, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Abajo, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL3.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArriba);
            Assert.AreEqual(1, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Abajo, vertebra.Sector);

            vertebra.RozarSector(VertebraL3.Sectores.Arriba);
            Assert.AreEqual(2, vertebra.VecesArriba);
            Assert.AreEqual(1, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);

            vertebra.Resetear();
            Assert.AreEqual(0, vertebra.VecesArriba);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL3.Sectores.Arriba, vertebra.Sector);
        }

        /// <summary>
        /// La VertebraL4 debe cambiar de estado con Rozar() y Abandonar().
        /// Los estados son Inicial, Rozando, Abandonada y RozandoNuevamente e incrementar el contador de veces rozada
        /// cuando hay un cambio de Inicial a Rozando o de Abandonada a RozandoNuevamente.
        /// </summary>
        [TestMethod]
        public void TestEstadosYVecesVertebraL4 () {
            VertebraL4 vertebra = new VertebraL4();
            bool cambio;

            Assert.AreEqual(VertebraL4.Estados.Inicial, vertebra.Estado);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(VertebraL4.Estados.Inicial, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(0, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(VertebraL4.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(VertebraL4.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.ArribaDerecha);
            Assert.AreEqual(VertebraL4.Estados.Rozando, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(VertebraL4.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(VertebraL4.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL4.Sectores.ArribaDerecha);
            Assert.AreEqual(VertebraL4.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            //Si L4 está siendo rozada en el sector ArribaDerecha y se roza también en el sector ArribaCentro,
            //se debe mantener el primer sector y no haber cambio de estado de la vertebra ni aumentar la cantidad de veces rozada.
            cambio = vertebra.RozarSector(VertebraL4.Sectores.ArribaCentro);
            Assert.AreEqual(VertebraL4.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.ArribaDerecha);
            Assert.AreEqual(VertebraL4.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL4.Sectores.ArribaCentro);
            Assert.AreEqual(VertebraL4.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(3, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.ArribaCentro);
            Assert.AreEqual(VertebraL4.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(3, vertebra.VecesRozada);

            cambio = vertebra.RozarSector(VertebraL4.Sectores.Abajo);
            Assert.AreEqual(VertebraL4.Estados.RozandoNuevamente, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(4, vertebra.VecesRozada);

            cambio = vertebra.AbandonarSector(VertebraL4.Sectores.Abajo);
            Assert.AreEqual(VertebraL4.Estados.Abandonada, vertebra.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(4, vertebra.VecesRozada);

            vertebra.Resetear();
            Assert.AreEqual(VertebraL4.Estados.Inicial, vertebra.Estado);
            Assert.AreEqual(0, vertebra.VecesRozada);
        }

        [TestMethod]
        public void TestSectoresYVecesVertebraL4 () {
            VertebraL4 vertebra = new VertebraL4();

            Assert.AreEqual(VertebraL4.Estados.Inicial, vertebra.Estado);

            vertebra.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaIzquierda, vertebra.Sector); // sacar


            vertebra.RozarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaIzquierda, vertebra.Sector);

            vertebra.RozarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaIzquierda, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.ArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaIzquierda, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaIzquierda, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.ArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaIzquierda, vertebra.Sector);

            vertebra.RozarSector(VertebraL4.Sectores.ArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaDerecha, vertebra.Sector);

            //Si L4 está siendo rozada en el sector ArribaDerecha y se roza también en el sector ArribaCentro,
            //se debe mantener el primer sector y no haber cambio de estado de la vertebra ni aumentar la cantidad de veces rozada.
            vertebra.RozarSector(VertebraL4.Sectores.ArribaCentro);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaDerecha, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.ArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaDerecha, vertebra.Sector);

            vertebra.RozarSector(VertebraL4.Sectores.ArribaCentro);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaCentro, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaCentro, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.ArribaCentro);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.ArribaCentro, vertebra.Sector);

            vertebra.RozarSector(VertebraL4.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaCentro);
            Assert.AreEqual(1, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.Abajo, vertebra.Sector);

            vertebra.AbandonarSector(VertebraL4.Sectores.Abajo);
            Assert.AreEqual(1, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(1, vertebra.VecesArribaDerecha);
            Assert.AreEqual(1, vertebra.VecesArribaCentro);
            Assert.AreEqual(1, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.Abajo, vertebra.Sector);

            vertebra.Resetear();
            Assert.AreEqual(0, vertebra.VecesArribaIzquierda);
            Assert.AreEqual(0, vertebra.VecesArribaDerecha);
            Assert.AreEqual(0, vertebra.VecesArribaCentro);
            Assert.AreEqual(0, vertebra.VecesAbajo);
            Assert.AreEqual(VertebraL4.Sectores.Abajo, vertebra.Sector);
        }
    }
}
