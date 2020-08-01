using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Conectores.ConectorKinect;
using Moq;
using System.Linq;
using System.Reflection;

namespace UnitTestLumbapp.Experto_ZE
{
    [TestClass]
    public class TestExpertoZE
    {
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Kinect no puede ser null. Necesito un conector a una kinect para crear un experto en zona esteril")]
        public void TestConstructorNull()
        {
            ExpertoZE exp = new ExpertoZE(null);
        }

        [TestMethod]
        public void TestConstructorOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);
        }

        [TestMethod]
        public void TestInicializarOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(true, init);
        }

        [TestMethod]
        public void TestInicializarConectarErr()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            conn.Setup(x => x.Conectar()).Throws(new KinectNotFoundException());
            ExpertoZE exp = new ExpertoZE(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(false, init);
        }

        [TestMethod]
        public void IniciarTooSoon()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);

            bool init = exp.IniciarSimulacion();
            Assert.AreEqual(false, init);
        }

        [TestMethod]
        public void IniciarOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);
            exp.Inicializar();

            bool init = exp.IniciarSimulacion();
            Assert.AreEqual(true, init);
        }

        [TestMethod]
        public void TerminarTooSoon1()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);

            var got = exp.TerminarSimulacion();

            Assert.AreEqual(0, got.Zona);
            Assert.AreEqual(0, got.ManoDerecha);
            Assert.AreEqual(0, got.ManoIzquierda);
        }

        [TestMethod]
        public void TerminarTooSoon2()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);
            exp.Inicializar();

            var got = exp.TerminarSimulacion();

            Assert.AreEqual(0, got.Zona);
            Assert.AreEqual(0, got.ManoDerecha);
            Assert.AreEqual(0, got.ManoIzquierda);
        }

        [TestMethod]
        public void TerminarNada()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);
            exp.Inicializar();
            exp.IniciarSimulacion();

            var got = exp.TerminarSimulacion();

            Assert.AreEqual(0, got.Zona);
            Assert.AreEqual(0, got.ManoDerecha);
            Assert.AreEqual(0, got.ManoIzquierda);
        }
    }
}
