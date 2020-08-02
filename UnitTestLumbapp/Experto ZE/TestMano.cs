using LumbApp.Expertos.ExpertoZE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLumbapp.Experto_ZE
{
    [TestClass]
    public class TestMano
    {
        /// <summary>
        /// La Mano debe cambiar de estado con Salir() y Entrar().
        /// Los estados son Inicial, Trabajando, Fuera y Contaminando.
        /// </summary>
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

        /// <summary>
        /// La Mano debe cambiar de tracking con ActualizarTrack.
        /// </summary>
        [TestMethod]
        public void TestTrackingMano()
        {
            Mano mano = new Mano();
            Assert.AreEqual(Mano.Tracking.Perdido, mano.Track);
            mano.ActualizarTrack(true);
            Assert.AreEqual(Mano.Tracking.Trackeado, mano.Track);
            mano.ActualizarTrack(false);
            Assert.AreEqual(Mano.Tracking.Perdido, mano.Track);
        }
    }
}
