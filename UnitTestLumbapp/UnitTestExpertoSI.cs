using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Conectores.ConectorSI;
using Moq;

namespace UnitTestLumbapp 
{
    [TestClass]
    public class UnitTestExpertoSI {
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Sensores no puede ser null. Necesito un conector a los sensores para crear un experto en sensores internos")]
        public void TestConstructorNull ()
        {
            ExpertoSI exp = new ExpertoSI(null);
        }

        [TestMethod]
        public void TestConstructorOK () {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            ExpertoSI exp = new ExpertoSI(conn.Object);
        }
    }

}
