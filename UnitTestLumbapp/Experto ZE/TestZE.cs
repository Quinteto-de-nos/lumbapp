using LumbApp.Expertos.ExpertoZE;
using Microsoft.Kinect;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLumbapp.Experto_ZE
{
    [TestClass]
    public class TestZE
    {
        /// <summary>
        /// La Zona Esteril debe contar cuantas veces de contamino y resetear el contador
        /// con Resetear.
        /// </summary>
        [TestMethod]
        public void TestContaminacionZE()
        {
            SkeletonPoint[] points = {
                newPoint(0,0,0), newPoint(1,0,0), newPoint(1, 0, 1),  newPoint(0,0,1),
                newPoint(0,1,0), newPoint(1,1,0), newPoint(1, 1, 1),  newPoint(0,1,1)};
            Calibracion cal = new Calibracion(points);
            ZonaEsteril ze = new ZonaEsteril(cal);

            Assert.AreEqual(0, ze.Contaminacion);
            ze.Contaminar();
            Assert.AreEqual(1, ze.Contaminacion);
            ze.Contaminar();
            Assert.AreEqual(2, ze.Contaminacion);
            ze.Resetear();
            Assert.AreEqual(0, ze.Contaminacion);
        }

        /// <summary>
        /// Version 1: Zona Esteril con limites fijos.
        /// EstaDentro debe dar true si la posicion esta dentro de la zona delimitada.
        /// </summary>
        [TestMethod]
        public void TestDentroZEFija()
        {
            SkeletonPoint[] points = { 
                newPoint(0,0,0), newPoint(0,0,1), newPoint(1, 0, 1),  newPoint(1,0,0),
                newPoint(0,1,0), newPoint(0,1,1), newPoint(1, 1, 1),  newPoint(1,1,0)};
            Calibracion cal = new Calibracion(points);
            ZonaEsteril ze = new ZonaEsteril(cal);

            Assert.AreEqual(true, ze.EstaDentro(newPoint(0.1f, 0.5f, 0.9f)));
            Assert.AreEqual(false, ze.EstaDentro(newPoint(1.2f, 0.5f, 0.9f)));
        }

        private SkeletonPoint newPoint(float x, float y, float z)
        {
            SkeletonPoint p = new SkeletonPoint();
            p.X = x;
            p.Y = y;
            p.Z = z;
            return p;
        }
    }
}
