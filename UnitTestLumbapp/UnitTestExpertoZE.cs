using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using Moq;

namespace UnitTestLumbapp
{
    [TestClass]
    public class UnitTestExpertoZE
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
        public void TestEstadoMano()
        {
            Mano mano = new Mano();
            bool cambio;

            Assert.AreEqual(Mano.Estados.Inicial, mano.Estado);

            cambio = mano.Salir();
            Assert.AreEqual(Mano.Estados.Inicial, mano.Estado);
            Assert.AreEqual(false, cambio);

            cambio = mano.Entrar();
            Assert.AreEqual(Mano.Estados.Trabajando, mano.Estado);
            Assert.AreEqual(true, cambio);

            cambio = mano.Entrar();
            Assert.AreEqual(Mano.Estados.Trabajando, mano.Estado);
            Assert.AreEqual(false, cambio);

            cambio = mano.Salir();
            Assert.AreEqual(Mano.Estados.Fuera, mano.Estado);
            Assert.AreEqual(true, cambio);

            cambio = mano.Salir();
            Assert.AreEqual(Mano.Estados.Fuera, mano.Estado);
            Assert.AreEqual(false, cambio);

            cambio = mano.Entrar();
            Assert.AreEqual(Mano.Estados.Contaminando, mano.Estado);
            Assert.AreEqual(true, cambio);

            cambio = mano.Entrar();
            Assert.AreEqual(Mano.Estados.Contaminando, mano.Estado);
            Assert.AreEqual(false, cambio);

            cambio = mano.Salir();
            Assert.AreEqual(Mano.Estados.Fuera, mano.Estado);
            Assert.AreEqual(true, cambio);

            cambio = mano.Entrar();
            Assert.AreEqual(Mano.Estados.Contaminando, mano.Estado);
            Assert.AreEqual(true, cambio);
        }

        [TestMethod]
        public void TestTrackingMano()
        {
            Mano mano = new Mano();
            Assert.AreEqual(Mano.Tracking.Perdido, mano.Track);
            mano.Track = Mano.Tracking.Trackeado;
            Assert.AreEqual(Mano.Tracking.Trackeado, mano.Track);
            mano.Track = Mano.Tracking.Perdido;
            Assert.AreEqual(Mano.Tracking.Perdido, mano.Track);
        }

        [TestMethod]
        public void TestContaminacionZE()
        {
            ZonaEsteril ze = new ZonaEsteril();
            Assert.AreEqual(0, ze.Contaminacion);
            ze.Contaminar();
            Assert.AreEqual(1, ze.Contaminacion);
            ze.Contaminar();
            Assert.AreEqual(2, ze.Contaminacion);
            ze.Resetear();
            Assert.AreEqual(0, ze.Contaminacion);
        }

        [TestMethod]
        public void TestDentroZEFija()
        {
            ZonaEsteril ze = new ZonaEsteril();
            Assert.AreEqual(true, ze.EstaDentro(.09f, -.09f, 1.05f));
            Assert.AreEqual(false, ze.EstaDentro(2, 0, 0));
        }
    }
}
