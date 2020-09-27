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
using LumbApp.Models;

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
        public void TestIniciarSimulaciónModoEvaluacion()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
            gui.Verify(x => x.SolicitarDatosPracticante(), Times.Once);
            orq.SetDatosDeSimulacion(new Mock<LumbApp.Models.DatosPracticante>().Object, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();
            expSI.Verify(x => x.IniciarSimulacion(), Times.Once);
            expZE.Verify(x => x.IniciarSimulacion(), Times.Once);
            gui.Verify(x => x.IniciarSimulacionModoEvaluacion(), Times.Once);
        }

        [TestMethod]
        public void TestIniciarSimulaciónModoGuiado()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
            gui.Verify(x => x.SolicitarDatosPracticante(), Times.Once);
            orq.SetDatosDeSimulacion(new Mock<LumbApp.Models.DatosPracticante>().Object, LumbApp.Enums.ModoSimulacion.ModoGuiado);
            orq.IniciarSimulacion();
            expSI.Verify(x => x.IniciarSimulacion(), Times.Once);
            expZE.Verify(x => x.IniciarSimulacion(), Times.Once);
            gui.Verify(x => x.IniciarSimulacionModoGuiado(), Times.Once);
        }

        [TestMethod]
        public void TestTerminarSimulacion()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();
            gui.Verify(x => x.SolicitarDatosPracticante(), Times.Once);

            orq.SetDatosDeSimulacion(new Mock<LumbApp.Models.DatosPracticante>().Object, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();
            expSI.Verify(x => x.IniciarSimulacion(), Times.Once);
            expZE.Verify(x => x.IniciarSimulacion(), Times.Once);
            gui.Verify(x => x.IniciarSimulacionModoEvaluacion(), Times.Once);

            orq.TerminarSimulacion();
            expSI.Verify(x => x.TerminarSimulacion(), Times.Once);
            expZE.Verify(x => x.TerminarSimulacion(), Times.Once);
        }

        [TestMethod]
        public void TestInformeGenerico()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();

            DatosPracticante dp = new DatosPracticante();
            dp.Dni = 39879304;
            dp.Nombre = "Alexis";
            dp.Apellido = "Aranda";
            dp.FolderPath = ".\\folder";

            orq.SetDatosDeSimulacion(dp, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();

            InformeSI informeSI = new InformeSI(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            InformeZE informeZE = new InformeZE(1, 2, 3);
            expSI.Setup(x => x.TerminarSimulacion()).Returns(informeSI);
            expZE.Setup(x => x.TerminarSimulacion()).Returns(informeZE);

            orq.TerminarSimulacion();
            gui.Verify(x => x.MostrarResultados(It.IsAny<Informe>()), Times.Once);
        }

        [TestMethod]
        public void TestInformeCorrecto()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Inicializar();

            DatosPracticante dp = new DatosPracticante();
            dp.Dni = 39879304;
            dp.Nombre = "Alexis";
            dp.Apellido = "Aranda";
            dp.FolderPath = ".\\folder";

            orq.SetDatosDeSimulacion(dp, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();

            InformeSI informeSI = new InformeSI(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            InformeZE informeZE = new InformeZE(1, 2, 3);
            expSI.Setup(x => x.TerminarSimulacion()).Returns(informeSI);
            expZE.Setup(x => x.TerminarSimulacion()).Returns(informeZE);

            orq.TerminarSimulacion();
            var arg = new ArgumentCaptor<Informe>();
            gui.Verify(x => x.MostrarResultados(arg.Capture()), Times.Once);
            Informe informe = arg.Value;
            Assert.IsNotNull(informe);
            Assert.AreEqual(dp.Dni, informe.Dni);
            Assert.AreEqual(dp.Nombre, informe.Nombre);
            Assert.AreEqual(dp.Apellido, informe.Apellido);
            Assert.AreEqual(dp.FolderPath, informe.FolderPath);
            Assert.AreEqual(informeZE.ManoDerecha, informe.ManoDerecha);
            Assert.AreEqual(informeZE.ManoIzquierda, informe.ManoIzquierda);
            Assert.AreEqual(informeZE.Zona, informe.Zona);
            Assert.AreEqual(informeSI.TejidoAdiposo, informe.TejidoAdiposo);
            Assert.AreEqual(informeSI.L5, informe.L5);
            Assert.AreEqual(informeSI.L4ArribaIzquierda, informe.L4ArribaIzquierda);
            Assert.AreEqual(informeSI.L4ArribaDerecha, informe.L4ArribaDerecha);
            Assert.AreEqual(informeSI.L4ArribaCentro, informe.L4ArribaCentro);
            Assert.AreEqual(informeSI.L4Abajo, informe.L4Abajo);
            Assert.AreEqual(informeSI.L3Arriba, informe.L3Arriba);
            Assert.AreEqual(informeSI.L3Abajo, informe.L3Abajo);
            Assert.AreEqual(informeSI.L2, informe.L2);
            Assert.AreEqual(informeSI.Duramadre, informe.Duramadre);
            Assert.AreEqual(informeSI.CaminoIncorrecto, informe.caminoIncorrecto);
            Assert.AreEqual(informeSI.CaminoCorrecto, informe.caminoCorrecto);
            Assert.AreNotEqual(TimeSpan.Zero, informe.TiempoTotalDeEjecucion);
            Assert.IsTrue(informe.PdfGenerado);
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

    public class ArgumentCaptor<T>
    {
        public T Capture()
        {
            return It.Is<T>(t => SaveValue(t));
        }

        private bool SaveValue(T t)
        {
            Value = t;
            return true;
        }

        public T Value { get; private set; }
    }
}
