using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Threading.Tasks;
using LumbApp.Orquestador;
using LumbApp.GUI;

namespace UnitTestLumbapp {
    [TestClass]
    public class UnitTestOrquestador {
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Gui no puede ser null. Necesito un GUIController para crear un Orquestador.")]
        public void TestConstructorNull () {
            Orquestador orq = new Orquestador(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "No se pudo detectar correctamente los sensores internos.")]
        public void TestInicializarErrAsync () {
            //Mock<ExpertoSI> exp = new Mock<ExpertoSI>();

            //exp.Setup(x => x.Inicializar()).Returns(false);


            Mock<IOrquestador> orq = new Mock<IOrquestador>();
            //orq.Object.SetExpertoSI(exp.Object);
            orq.Setup(x => x.Inicializar()).Throws(new Exception("No se pudo detectar correctamente los sensores internos."));

            orq.Object.Inicializar();
        }
        
    }
}
