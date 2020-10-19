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

namespace UnitTestLumbapp
{
    [TestClass]
    public class UnitTestOrquestador
    {
        /// <summary>
        /// Test que prueba el caso de intentar crear un orquestador sin una GUI, generando una exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Gui no puede ser null. Necesito un GUIController para crear un Orquestador.")]
        public void TestConstructorNull()
        {
            _ = new Orquestador(null, null);
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
            _ = new Orquestador(gui.Object, null);
        }

        [TestMethod]
        public void TestConstructorOK()
        {
            Mock<IGUIController> gui = new Mock<IGUIController>();
            _ = new Orquestador(gui.Object, mockedConectorFS());
        }

        [TestMethod]
        public async Task TestInicializarOk()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();
            gui.Verify(x => x.SolicitarDatosPracticante(It.IsAny<DatosPracticante>()), Times.Once) ;
        }

        [TestMethod]
        public async Task TestIniciarSimulaciónModoEvaluacion()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();

            var datos = new DatosPracticante()
            {
                Dni = 99999999,
                Nombre = "Maximo",
                Apellido = "Cozzetti",
                Email = "mcozzetti@alumno.unlam.edu.ar",
                FolderPath = "C:\\Cozzetti\\Desktop"
            };
            orq.SetDatosDeSimulacion(datos, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();

            expSI.Verify(x => x.IniciarSimulacion(), Times.Once);
            expZE.Verify(x => x.IniciarSimulacion(It.IsAny<IVideo>()), Times.Once);
            gui.Verify(x => x.IniciarSimulacionModoEvaluacion(), Times.Once);
        }

        [TestMethod]
        public async Task TestIniciarSimulaciónModoGuiado()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();
            var datos = new DatosPracticante()
            {
                Dni = 99999999,
                Nombre = "Maximo",
                Apellido = "Cozzetti",
                Email = "mcozzetti@alumno.unlam.edu.ar",
                FolderPath = "C:\\Cozzetti\\Desktop"
            };
            orq.SetDatosDeSimulacion(datos, LumbApp.Enums.ModoSimulacion.ModoGuiado);
            orq.IniciarSimulacion();

            expSI.Verify(x => x.IniciarSimulacion(), Times.Once);
            expZE.Verify(x => x.IniciarSimulacion(It.IsAny<IVideo>()), Times.Once);
            gui.Verify(x => x.IniciarSimulacionModoGuiado(), Times.Once);
        }

        [TestMethod]
        public async Task TestTerminarSimulacion()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IVideo> videoZE = new Mock<IVideo>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expSI.Setup(x => x.TerminarSimulacion()).Returns(new InformeSI(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12));
            expZE.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.TerminarSimulacion()).Returns(new InformeZE(0,0,0,videoZE.Object));

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();

            var datos = new DatosPracticante()
            {
                Dni = 99999999,
                Nombre = "Maximo",
                Apellido = "Cozzetti",
                Email = "mcozzetti@alumno.unlam.edu.ar",
                FolderPath = "C:\\Cozzetti\\Desktop"
            };
            orq.SetDatosDeSimulacion(datos, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();

            expSI.Verify(x => x.IniciarSimulacion(), Times.Once);
            expZE.Verify(x => x.IniciarSimulacion(It.IsAny<IVideo>()), Times.Once);
            gui.Verify(x => x.IniciarSimulacionModoEvaluacion(), Times.Once);

            await orq.TerminarSimulacion();
            expSI.Verify(x => x.TerminarSimulacion(), Times.Once);
            expZE.Verify(x => x.TerminarSimulacion(), Times.Once);
        }

        [TestMethod]
        public async Task TestInformeGenerico()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();

            DatosPracticante dp = new DatosPracticante
            {
                Dni = 39879304,
                Nombre = "Alexis",
                Apellido = "Aranda",
                FolderPath = ".\\folder"
            };

            orq.SetDatosDeSimulacion(dp, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();

            InformeSI informeSI = new InformeSI(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            Mock<IVideo> video = new Mock<IVideo>();
            InformeZE informeZE = new InformeZE(1, 2, 3, video.Object);
            expSI.Setup(x => x.TerminarSimulacion()).Returns(informeSI);
            expZE.Setup(x => x.TerminarSimulacion()).Returns(informeZE);

            await orq.TerminarSimulacion();
            gui.Verify(x => x.MostrarResultados(It.IsAny<Informe>()), Times.Once);
        }

        [TestMethod]
        public async Task TestInformeCorrectoAsync()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();
            Mock<IGUIController> gui = new Mock<IGUIController>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();

            DatosPracticante dp = new DatosPracticante
            {
                Dni = 39879304,
                Nombre = "Alexis",
                Apellido = "Aranda",
                FolderPath = ".\\folder"
            };

            orq.SetDatosDeSimulacion(dp, LumbApp.Enums.ModoSimulacion.ModoEvaluacion);
            orq.IniciarSimulacion();

            InformeSI informeSI = new InformeSI(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);
            InformeZE informeZE = new InformeZE(1, 2, 3, new Mock<IVideo>().Object);
            expSI.Setup(x => x.TerminarSimulacion()).Returns(informeSI);
            expZE.Setup(x => x.TerminarSimulacion()).Returns(informeZE);
            await Task.Delay(1000); //Para que el tiempo de simulacion no de 0 segundos

            await orq.TerminarSimulacion();

            var arg = new ArgumentCaptor<Informe>();
            gui.Verify(x => x.MostrarResultados(arg.Capture()), Times.Once);
            Informe informe = arg.Value;
            Assert.IsNotNull(informe);

            Assert.AreEqual(dp.Dni, informe.Dni);
            Assert.AreEqual(dp.Nombre, informe.Nombre);
            Assert.AreEqual(dp.Apellido, informe.Apellido);
            Assert.AreEqual(dp.FolderPath, informe.FolderPath);

            Assert.AreEqual(informeZE.ManoDerecha.ToString(), informe.DatosPractica["Contaminaciones por mano derecha"]);
            Assert.AreEqual(informeZE.ManoIzquierda.ToString(), informe.DatosPractica["Contaminaciones por mano izquierda"]);
            Assert.AreEqual(informeZE.Zona.ToString(), informe.DatosPractica["Contaminaciones totales"]);

            Assert.AreEqual(informeSI.TejidoAdiposo.ToString(), informe.DatosPractica["Punciones tejido adiposo"]);
            Assert.AreEqual(informeSI.L5.ToString(), informe.DatosPractica["Roces L5"]);
            Assert.AreEqual(informeSI.L4ArribaIzquierda.ToString(), informe.DatosPractica["Roces L4 arriba izquierda"]);
            Assert.AreEqual(informeSI.L4ArribaDerecha.ToString(), informe.DatosPractica["Roces L4 arriba derecha"]);
            Assert.AreEqual(informeSI.L4ArribaCentro.ToString(), informe.DatosPractica["Roces L4 arriba centro"]);
            Assert.AreEqual(informeSI.L4Abajo.ToString(), informe.DatosPractica["Roces L4 abajo"]);
            Assert.AreEqual(informeSI.L3Arriba.ToString(), informe.DatosPractica["Roces L3 arriba"]);
            Assert.AreEqual(informeSI.L3Abajo.ToString(), informe.DatosPractica["Roces L3 abajo"]);
            Assert.AreEqual(informeSI.L2.ToString(), informe.DatosPractica["Roces L2"]);
            Assert.AreEqual(informeSI.Duramadre.ToString(), informe.DatosPractica["Punciones duramadre"]);
            Assert.AreEqual(informeSI.CaminoIncorrecto.ToString(), informe.DatosPractica["Caminos incorrectos"]);
            Assert.AreEqual(informeSI.CaminoCorrecto.ToString(), informe.DatosPractica["Caminos correctos"]);

            Assert.AreNotEqual(TimeSpan.Zero.ToString(), informe.DatosPractica["Tiempo total"]);
            Assert.IsTrue(informe.PdfGenerado);
        }

        [TestMethod]
        public async Task TestInicializarSIErrAsyncInSI()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(false);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();
            gui.Verify(x => x.MostrarErrorDeConexion(It.IsAny<String>()), Times.Once);
        }

        [TestMethod]
        public async Task TestInicializarSIErrAsyncInZE()
        {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(false);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();
            gui.Verify(x => x.MostrarErrorDeConexion(It.IsAny<String>()), Times.Once);
        }

        [TestMethod]
        public async Task TestFinalizarOK () {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(true);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();
            
            Assert.AreEqual(orq.inicializacionOk, true);
            orq.Finalizar();
            expSI.Verify(x => x.Finalizar(), Times.Once);
            expZE.Verify(x => x.Finalizar(), Times.Once);
            Assert.AreEqual(orq.inicializacionOk, false);
        }

        [TestMethod]

        public void TestFinalizarTooSoon () {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(false);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            orq.Finalizar();
            expSI.Verify(x => x.Finalizar(), Times.Never);
            expZE.Verify(x => x.Finalizar(), Times.Never);
            Assert.AreEqual(orq.inicializacionOk, false);
        }

        [TestMethod]
        public async Task TestFinalizarInicializacionFallida () {
            Mock<IExpertoSI> expSI = new Mock<IExpertoSI>();
            Mock<IExpertoZE> expZE = new Mock<IExpertoZE>();

            expSI.Setup(x => x.Inicializar()).Returns(false);
            expZE.Setup(x => x.Inicializar()).Returns(true);

            Mock<IGUIController> gui = new Mock<IGUIController>();
            Orquestador orq = new Orquestador(gui.Object, mockedConectorFS());
            orq.SetExpertoSI(expSI.Object);
            orq.SetExpertoZE(expZE.Object);

            await orq.Inicializar();

            orq.Finalizar();
            expSI.Verify(x => x.Finalizar(), Times.Never);
            expZE.Verify(x => x.Finalizar(), Times.Never);
            Assert.AreEqual(orq.inicializacionOk, false);
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
