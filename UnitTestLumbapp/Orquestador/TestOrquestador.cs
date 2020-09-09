using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Threading.Tasks;
using LumbApp.Orquestador;
using LumbApp.GUI;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;

namespace UnitTestLumbapp {
    [TestClass]
    public class TestOrquestador {
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Gui no puede ser null. Necesito un GUIController para crear un Orquestador.")]
        public void TestConstructorNull () {
            Orquestador orq = new Orquestador(null);
        }

        [TestMethod]
        public void TestInicializarOk() {
            Mock<ExpertoSI> expSI = new Mock<ExpertoSI>();
            Mock<ExpertoZE> expZE = new Mock<ExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<GUIController> gui = new Mock<GUIController>();
            Orquestador orq = new Orquestador(gui.Object);
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
        }

        [TestMethod]
        [ExpectedException(typeof(Exception),
            "No se pudo detectar correctamente los sensores internos.")]
        public void TestInicializarSIErrAsync () {
            Mock<ExpertoSI> expSI = new Mock<ExpertoSI>();
            Mock<ExpertoZE> expZE = new Mock<ExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(false);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<GUIController> gui = new Mock<GUIController>();
            Orquestador orq = new Orquestador(gui.Object);
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
        }
        
    }
}
