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
        /// <summary>
        /// provisorio
        /// </summary>
        [TestMethod]
        public void CrearOrquestadorOk()
        {
            String textToBeReturned = "{\"zonaEsteril\":[{\"X\":-0.0531591699,\"Y\":-0.191040874,\"Z\":0.949000061},{\"X\":0.0752977654,\"Y\":-0.116173707,\"Z\":1.22900009},{\"X\":0.268609017,\"Y\":-0.144120455,\"Z\":1.11400008},{\"X\":0.140152082,\"Y\":-0.218987614,\"Z\":0.834000051},{\"X\":-0.0564637221,\"Y\":0.0991351306,\"Z\":0.872928083},{\"X\":0.0719932094,\"Y\":0.17400229,\"Z\":1.15292811},{\"X\":0.265304476,\"Y\":0.146055549,\"Z\":1.0379281},{\"X\":0.136847526,\"Y\":0.0711883903,\"Z\":0.757928073}]}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Returns(textToBeReturned);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            Calibracion calibracion = conector.LevantarArchivoDeTextoComoObjeto<Calibracion>("C:\\file.txt");
            ExpertoZE expertoZE;
            ExpertoSI expertoSI;
            IConectorKinect conectorKinect;
            IConectorSI conectorSI;
            conectorKinect = new ConectorKinect();
            expertoZE = new ExpertoZE(conectorKinect, calibracion);
            conectorSI = new ConectorSI();
            expertoSI = new ExpertoSI(conectorSI);
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
            String textToBeReturned = "{\"zonaEsteril\":[{\"X\":-0.0531591699,\"Y\":-0.191040874,\"Z\":0.949000061},{\"X\":0.0752977654,\"Y\":-0.116173707,\"Z\":1.22900009},{\"X\":0.268609017,\"Y\":-0.144120455,\"Z\":1.11400008},{\"X\":0.140152082,\"Y\":-0.218987614,\"Z\":0.834000051},{\"X\":-0.0564637221,\"Y\":0.0991351306,\"Z\":0.872928083},{\"X\":0.0719932094,\"Y\":0.17400229,\"Z\":1.15292811},{\"X\":0.265304476,\"Y\":0.146055549,\"Z\":1.0379281},{\"X\":0.136847526,\"Y\":0.0711883903,\"Z\":0.757928073}]}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Returns(textToBeReturned);
            Mock<IGUIController> gui = new Mock<IGUIController>();
            IOrquestador orq = new Orquestador(gui.Object, new ConectorFS(filesystem.Object));
        }
    }
}
