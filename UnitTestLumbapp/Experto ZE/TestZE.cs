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
        public void TestDentroZEFija()
        {
            ZonaEsteril ze = new ZonaEsteril();
            Assert.AreEqual(true, ze.EstaDentro(.09f, -.09f, 1.05f));
            Assert.AreEqual(false, ze.EstaDentro(2, 0, 0));
        }
    }
}
