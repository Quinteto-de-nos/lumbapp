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
        public void TestInicializarOK () {
            Mock<GUIController> gui = new Mock<GUIController>();
            Mock<Orquestador> orq = new Mock<Orquestador>(gui);

            orq.Setup(x => x.Inicializar());

            orq.Object.Inicializar();           
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "No se pudo detectar correctamente los sensores internos.")]
        public void TestInicializarErrAsync () {
            //Mock<ExpertoSI> exp = new Mock<ExpertoSI>();

            //exp.Setup(x => x.Inicializar()).Returns(false);


            Mock<Orquestador> orq = new Mock<Orquestador>();
            //orq.Object.SetExpertoSI(exp.Object);
            orq.Setup(x => x.Inicializar()).Throws(new Exception("No se pudo detectar correctamente los sensores internos."));

            orq.Object.Inicializar();
        }
        
    }
}
