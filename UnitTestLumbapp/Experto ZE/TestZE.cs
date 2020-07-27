using LumbApp.Expertos.ExpertoZE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLumbapp.Experto_ZE
{
    [TestClass]
    public class TestZE
    {
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
        public void TestTrackingMano()
        {
            Mano mano = new Mano();
            Assert.AreEqual(Mano.Tracking.Perdido, mano.Track);
            mano.Track = Mano.Tracking.Trackeado;
            Assert.AreEqual(Mano.Tracking.Trackeado, mano.Track);
            mano.Track = Mano.Tracking.Perdido;
            Assert.AreEqual(Mano.Tracking.Perdido, mano.Track);
        }
    }
}
