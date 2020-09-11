using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using System.Threading.Tasks;
using LumbApp.Orquestador;
using LumbApp.GUI;
using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Conectores.ConectorSI;
using System.IO.Abstractions;
using LumbApp.Conectores.ConectorFS;

namespace UnitTestLumbapp {
    [TestClass]
    public class UnitTestOrquestador {
        /// <summary>
        /// Test que prueba el caso de intentar crear un orquestador sin una GUI, generando una exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Gui no puede ser null. Necesito un GUIController para crear un Orquestador.")]
        public void TestConstructorNull () {
            Orquestador orq = new Orquestador(null, null);
        }

        /// <summary>
        /// Test que prueba el caso de intentar crear un orquestador con GUI, pero incapaz de levantar un objeto calibración, generando una exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Error al tratar de cargar el archivo de calibracion.")]
        public void TestConstructorSinCalibracion()
        {
            Mock<IGUIController> gui = new Mock<IGUIController>();
            IOrquestador orq = new Orquestador(gui.Object, null);
        }

        [TestMethod]
        public void TestConstructorOK()
        {
            Mock<IGUIController> gui = new Mock<IGUIController>();
            IOrquestador orq = new Orquestador(gui.Object, mockedConectorFS());
        }

        [TestMethod]
        public void TestInicializarOk()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
            gui.Verify(x => x.SolicitarDatosPracticante(), Times.Once);
        }

        [TestMethod]
        public void TestInicializarSIErrAsyncInSI()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(false);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
            gui.Verify(x => x.MostrarErrorDeConexion(It.IsAny<String>()), Times.Once);
        }

        [TestMethod]
        public void TestInicializarSIErrAsyncInZE()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(false);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
            gui.Verify(x => x.MostrarErrorDeConexion(It.IsAny<String>()), Times.Once);
        }

        private ConectorFS mockedConectorFS()
        {
            String textToBeReturned = "{\"zonaEsteril\":[{\"X\":-0.0531591699,\"Y\":-0.191040874,\"Z\":0.949000061},{\"X\":0.0752977654,\"Y\":-0.116173707,\"Z\":1.22900009},{\"X\":0.268609017,\"Y\":-0.144120455,\"Z\":1.11400008},{\"X\":0.140152082,\"Y\":-0.218987614,\"Z\":0.834000051},{\"X\":-0.0564637221,\"Y\":0.0991351306,\"Z\":0.872928083},{\"X\":0.0719932094,\"Y\":0.17400229,\"Z\":1.15292811},{\"X\":0.265304476,\"Y\":0.146055549,\"Z\":1.0379281},{\"X\":0.136847526,\"Y\":0.0711883903,\"Z\":0.757928073}]}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Returns(textToBeReturned);
            return new ConectorFS(filesystem.Object);
        }
    }
}
