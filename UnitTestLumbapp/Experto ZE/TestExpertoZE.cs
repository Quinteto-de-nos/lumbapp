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
        /// <summary>
        /// Si el constructor de Experto ZE se llama con null, debe tirar excepcion.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Kinect no puede ser null. Necesito un conector a una kinect para crear un experto en zona esteril")]
        public void TestConstructorNull()
        {
            ExpertoZE exp = new ExpertoZE(null);
        }

        /// <summary>
        /// Si el constructor se llama con un conector, OK.
        /// </summary>
        [TestMethod]
        public void TestConstructorOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);
        }

        /// <summary>
        /// Si en Inicializar pasa todo OK debe devolver true.
        /// </summary>
        [TestMethod]
        public void TestInicializarOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(true, init);
        }

        /// <summary>
        /// Cuando se llama a Inicializar, si el Conectar() del conector tira excepcion, debe atajarla
        /// y devolver false.
        /// </summary>
        [TestMethod]
        public void TestInicializarConectarErr()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            conn.Setup(x => x.Conectar()).Throws(new KinectNotFoundException());
            ExpertoZE exp = new ExpertoZE(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(false, init);
        }

        /// <summary>
        /// Si IniciarSimulacion se llama antes de Inicializar, debe devolver false.
        /// </summary>
        [TestMethod]
        public void IniciarTooSoon()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);

            bool init = exp.IniciarSimulacion();
            Assert.AreEqual(false, init);
        }

        /// <summary>
        /// Si IniciarSimulacion esta OK debe devolver true
        /// </summary>
        [TestMethod]
        public void IniciarOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object);
            exp.Inicializar();

            bool init = exp.IniciarSimulacion();
            Assert.AreEqual(true, init);
        }

        /// <summary>
        /// Si se llama a TerminarSimulacion antes de Inicializar debe devolver un reporte vacio
        /// </summary>
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

        /// <summary>
        /// Si se llama a TerminarSimulacion antes de IniciarSimulacion, debe devolver un reporte
        /// vacio.
        /// </summary>
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

        /// <summary>
        /// Si se llama a TerminarSimulacion todo OK, debe devolver un reporte con las cosas que ocurrieron.
        /// Si no ocurrio nada, el reporte estara vacio.
        /// </summary>
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