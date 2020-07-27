using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Conectores.ConectorKinect;
using Moq;

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
        public void TestDentroZEFija()
        {
            ZonaEsteril ze = new ZonaEsteril();
            Assert.AreEqual(true, ze.EstaDentro(.09f, -.09f, 1.05f));
            Assert.AreEqual(false, ze.EstaDentro(2, 0, 0));
        }
    }
}
