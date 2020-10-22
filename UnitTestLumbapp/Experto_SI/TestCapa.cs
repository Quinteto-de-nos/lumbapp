using LumbApp.Expertos.ExpertoSI.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLumbapp.Experto_SI
{
    [TestClass]
    public class TestCapa
    {
        /// <summary>
        /// La Capa debe cambiar de estado con Atavesar() y Abandonar().
        /// Los estados son Inicial, Atravesando, Abanonada y AtravesandoNuevamente.
        /// </summary>
        [TestMethod]
        public void TestEstadoCapa()
        {
            Capa capa = new Capa();
            bool cambio;

            Assert.AreEqual(Capa.Estados.Inicial, capa.Estado);

            cambio = capa.Abandonar();
            Assert.AreEqual(Capa.Estados.Inicial, capa.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(0, capa.VecesAtravesada);

            cambio = capa.Atravesar();
            Assert.AreEqual(Capa.Estados.Atravesando, capa.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, capa.VecesAtravesada);

            cambio = capa.Atravesar();
            Assert.AreEqual(Capa.Estados.Atravesando, capa.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, capa.VecesAtravesada);

            cambio = capa.Abandonar();
            Assert.AreEqual(Capa.Estados.Abandonada, capa.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(1, capa.VecesAtravesada);

            cambio = capa.Abandonar();
            Assert.AreEqual(Capa.Estados.Abandonada, capa.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(1, capa.VecesAtravesada);

            cambio = capa.Atravesar();
            Assert.AreEqual(Capa.Estados.AtravesandoNuevamente, capa.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, capa.VecesAtravesada);

            cambio = capa.Atravesar();
            Assert.AreEqual(Capa.Estados.AtravesandoNuevamente, capa.Estado);
            Assert.AreEqual(false, cambio);
            Assert.AreEqual(2, capa.VecesAtravesada);

            cambio = capa.Abandonar();
            Assert.AreEqual(Capa.Estados.Abandonada, capa.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(2, capa.VecesAtravesada);

            cambio = capa.Atravesar();
            Assert.AreEqual(Capa.Estados.AtravesandoNuevamente, capa.Estado);
            Assert.AreEqual(true, cambio);
            Assert.AreEqual(3, capa.VecesAtravesada);
        }
    }
}
