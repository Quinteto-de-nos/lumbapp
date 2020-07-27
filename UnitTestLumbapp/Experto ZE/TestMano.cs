using LumbApp.Expertos.ExpertoZE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLumbapp.Experto_ZE
{
    class TestMano
    {
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
    }
}
