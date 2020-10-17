using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Conectores.ConectorKinect;
using Moq;
using System.Text.Json;

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
            _ = new ExpertoZE(null, null);
        }

        /// <summary>
        /// Si la calibracion de Experto ZE se llama con null, debe tirar excepcion.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception), "Datos de calibracion mal formados.")]
        public void TestConstructorCalNull()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            _ = new ExpertoZE(conn.Object, null);
        }

        /// <summary>
        /// Si el constructor se llama con un conector, OK.
        /// </summary>
        [TestMethod]
        public void TestConstructorOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            _ = new ExpertoZE(conn.Object, newCalibracion());
        }

        private Calibracion newCalibracion()
        {
            string json = "{\"zonaEsteril\":[{\"X\":-0.0531591699,\"Y\":-0.191040874,\"Z\":0.949000061},{\"X\":0.0752977654,\"Y\":-0.116173707,\"Z\":1.22900009},{\"X\":0.268609017,\"Y\":-0.144120455,\"Z\":1.11400008},{\"X\":0.140152082,\"Y\":-0.218987614,\"Z\":0.834000051},{\"X\":-0.0564637221,\"Y\":0.0991351306,\"Z\":0.872928083},{\"X\":0.0719932094,\"Y\":0.17400229,\"Z\":1.15292811},{\"X\":0.265304476,\"Y\":0.146055549,\"Z\":1.0379281},{\"X\":0.136847526,\"Y\":0.0711883903,\"Z\":0.757928073}]}";
            return JsonSerializer.Deserialize<Calibracion>(json);
        }

        /// <summary>
        /// Si en Inicializar pasa todo OK debe devolver true.
        /// </summary>
        [TestMethod]
        public void TestInicializarOK()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());

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
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());

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
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());
            Mock<IVideo> video = new Mock<IVideo>();

            bool init = exp.IniciarSimulacion(video.Object);
            Assert.AreEqual(false, init);
        }

        /// <summary>
        /// Si IniciarSimulacion esta OK debe devolver true
        /// </summary>
        [TestMethod]
        public void IniciarOK()
        {
            var videoMock = new Mock<IVideo>();
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());
            exp.Inicializar();

            bool init = exp.IniciarSimulacion(videoMock.Object);
            Assert.AreEqual(true, init);
        }

        /// <summary>
        /// Si se llama a TerminarSimulacion antes de Inicializar debe devolver un reporte vacio
        /// </summary>
        [TestMethod]
        public void TerminarTooSoon1()
        {
            Mock<IConectorKinect> conn = new Mock<IConectorKinect>();
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());

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
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());
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
            var videoMock = new Mock<IVideo>();
            ExpertoZE exp = new ExpertoZE(conn.Object, newCalibracion());
            exp.Inicializar();
            exp.IniciarSimulacion(videoMock.Object);

            var got = exp.TerminarSimulacion();

            Assert.AreEqual(0, got.Zona);
            Assert.AreEqual(0, got.ManoDerecha);
            Assert.AreEqual(0, got.ManoIzquierda);
        }
    }
}
