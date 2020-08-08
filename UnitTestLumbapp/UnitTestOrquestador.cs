using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LumbApp.Orquestador;
using Moq;
using LumbApp.Expertos.ExpertoSI;

namespace UnitTestLumbapp {
    [TestClass]
    public class UnitTestOrquestador {
        [TestMethod]
        public void TestInicializarOK () {
            Mock<Orquestador> orq = new Mock<Orquestador>();

            orq.Setup(x => x.Inicializar()).Returns(false);

            bool init = orq.Object.Inicializar();
            Assert.AreEqual(true, init);
        }

        [TestMethod]
        public void TestInicializarSIErr () {

            Mock<Orquestador> orq = new Mock<Orquestador>();

            orq.Setup(x => x.Inicializar()).Returns(false);

            bool init = orq.Object.Inicializar();
            Assert.AreEqual(false, init);
        }
    }
}
