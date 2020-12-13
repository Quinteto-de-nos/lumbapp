using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Conectores.ConectorSI;
using Moq;
using LumbApp.Expertos.ExpertoSI.Utils;

namespace UnitTestLumbapp.Experto_SI
{
    [TestClass]
    public class TestExpertoSI
    {

        /// <summary>
        /// Si el constructor de Experto SI se llama con null, debe tirar excepcion.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception),
            "Sensores no puede ser null. Necesito un conector a los sensores para crear un experto en sensores internos")]
        public void TestConstructorNull()
        {
            new ExpertoSI(null);
        }

        /// <summary>
        /// Si el constructor del Experto SI se llama con un conector, el resultado es OK.
        /// </summary>
        [TestMethod]
        public void TestConstructorOK()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            _ = new ExpertoSI(conn.Object);
        }

        /// <summary>
        /// Si en Inicializar pasa todo OK debe devolver true.
        /// </summary>
        [TestMethod]
        public void TestInicializarOK()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.Conectar()).Returns(true);
            conn.Setup(x => x.CheckearComunicacion()).Returns(true);
            ExpertoSI exp = new ExpertoSI(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(true, init);
        }

        /// <summary>
        /// Cuando se llama a Inicializar, si el Conectar() del conector tira una excepcion, debe capturarla y 
        /// retornar false.
        /// </summary>
        [TestMethod]
        public void TestInicializarConectarErr()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.Conectar()).Throws(new Exception());
            ExpertoSI exp = new ExpertoSI(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(false, init);
        }

        /// <summary>
        /// Cuando se llama a Inicializar, si el ChekearComunicacion() del conector devuelve false,
        /// la inicializacion debe retornar false.
        /// </summary>
        [TestMethod]
        public void TestInicializarCheckeoErr()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.CheckearComunicacion()).Returns(false);
            ExpertoSI exp = new ExpertoSI(conn.Object);

            bool init = exp.Inicializar();
            Assert.AreEqual(false, init);
        }

        /// <summary>
        /// Si IniciarSimulacion se llama antes de Inicializar, debe devolver false.
        /// </summary>
        [TestMethod]
        public void IniciarTooSoon()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();

            ExpertoSI exp = new ExpertoSI(conn.Object);

            bool init = exp.IniciarSimulacion();
            Assert.AreEqual(false, init);
        }

        /// <summary>
        /// Si IniciarSimulacion esta OK debe devolver true
        /// </summary>
        [TestMethod]
        public void IniciarOK()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.Conectar()).Returns(true);
            conn.Setup(x => x.CheckearComunicacion()).Returns(true);

            ExpertoSI exp = new ExpertoSI(conn.Object);
            exp.Inicializar();

            bool init = exp.IniciarSimulacion();
            Assert.AreEqual(true, init);
        }

        /// <summary>
        /// Si se llama a TerminarSimulacion antes de Inicializar debe devolver un reporte vacio
        /// </summary>
        [TestMethod]
        public void TerminarTooSoon1()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            ExpertoSI exp = new ExpertoSI(conn.Object);

            var informe = exp.TerminarSimulacion();

            Assert.AreEqual(0, informe.TejidoAdiposo);
            Assert.AreEqual(0, informe.L2);

            Assert.AreEqual(0, informe.L3Arriba);
            Assert.AreEqual(0, informe.L3Abajo);

            Assert.AreEqual(0, informe.L4ArribaIzquierda);
            Assert.AreEqual(0, informe.L4ArribaDerecha);
            Assert.AreEqual(0, informe.L4ArribaCentro);
            Assert.AreEqual(0, informe.L4Abajo);

            Assert.AreEqual(0, informe.L5);
            Assert.AreEqual(0, informe.Duramadre);
        }

        /// <summary>
        /// Si se llama a TerminarSimulacion antes de IniciarSimulacion, debe devolver un reporte
        /// vacio.
        /// </summary>
        [TestMethod]
        public void TerminarTooSoon2()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.CheckearComunicacion()).Returns(true);

            ExpertoSI exp = new ExpertoSI(conn.Object);
            exp.Inicializar();

            var informe = exp.TerminarSimulacion();

            Assert.AreEqual(0, informe.TejidoAdiposo);
            Assert.AreEqual(0, informe.L2);

            Assert.AreEqual(0, informe.L3Arriba);
            Assert.AreEqual(0, informe.L3Abajo);

            Assert.AreEqual(0, informe.L4ArribaIzquierda);
            Assert.AreEqual(0, informe.L4ArribaDerecha);
            Assert.AreEqual(0, informe.L4ArribaCentro);
            Assert.AreEqual(0, informe.L4Abajo);

            Assert.AreEqual(0, informe.L5);
            Assert.AreEqual(0, informe.Duramadre);
        }

        /// <summary>
        /// Si se llama a TerminarSimulacion todo OK, debe devolver un reporte con las cosas que ocurrieron.
        /// Si no ocurrio nada, el reporte estara vacio.
        /// </summary>
        [TestMethod]
        public void TerminarOkEnCeros()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.CheckearComunicacion()).Returns(true);

            ExpertoSI exp = new ExpertoSI(conn.Object);
            exp.Inicializar();
            exp.IniciarSimulacion();

            var informe = exp.TerminarSimulacion();

            Assert.AreEqual(0, informe.TejidoAdiposo);
            Assert.AreEqual(0, informe.L2);

            Assert.AreEqual(0, informe.L3Arriba);
            Assert.AreEqual(0, informe.L3Abajo);

            Assert.AreEqual(0, informe.L4ArribaIzquierda);
            Assert.AreEqual(0, informe.L4ArribaDerecha);
            Assert.AreEqual(0, informe.L4ArribaCentro);
            Assert.AreEqual(0, informe.L4Abajo);

            Assert.AreEqual(0, informe.L5);
            Assert.AreEqual(0, informe.Duramadre);
        }

        /// <summary>
        /// Si se llama a TerminarSimulacion todo OK, debe devolver un reporte con las cosas que ocurrieron.
        /// </summary>
        [TestMethod]
        public void TerminarOk()
        {
            Mock<IConectorSI> conn = new Mock<IConectorSI>();
            conn.Setup(x => x.Conectar()).Returns(true);
            conn.Setup(x => x.CheckearComunicacion()).Returns(true);

            ExpertoSI exp = new ExpertoSI(conn.Object);
            exp.Inicializar();
            exp.IniciarSimulacion();

            #region Simulacion de practica

            exp.GetTejidoAdiposo().Atravesar();//1 tej ad
            exp.GetTejidoAdiposo().Abandonar();
            exp.GetTejidoAdiposo().Atravesar();//2 tej ad
            exp.GetTejidoAdiposo().Abandonar();

            exp.GetL3().RozarSector(VertebraL3.Sectores.Arriba);//1 L3 arriba
            exp.GetL3().AbandonarSector(VertebraL3.Sectores.Arriba);

            exp.GetL3().RozarSector(VertebraL3.Sectores.Abajo);//1 L3 abajo
            exp.GetL3().AbandonarSector(VertebraL3.Sectores.Abajo);
            exp.GetL3().RozarSector(VertebraL3.Sectores.Abajo);//2 L3 abajo
            exp.GetL3().AbandonarSector(VertebraL3.Sectores.Abajo);

            exp.GetL4().RozarSector(VertebraL4.Sectores.ArribaDerecha);//1 L4 arriba der
            exp.GetL4().AbandonarSector(VertebraL4.Sectores.ArribaDerecha);
            exp.GetL4().RozarSector(VertebraL4.Sectores.ArribaDerecha);//2 L4 arriba der
            exp.GetL4().AbandonarSector(VertebraL4.Sectores.ArribaDerecha);
            exp.GetL4().RozarSector(VertebraL4.Sectores.ArribaDerecha);//3 L4 arriba der
            exp.GetL4().AbandonarSector(VertebraL4.Sectores.ArribaDerecha);

            exp.GetL4().RozarSector(VertebraL4.Sectores.ArribaCentro);//1 L4 arriba cen
            exp.GetL4().AbandonarSector(VertebraL4.Sectores.ArribaCentro);

            exp.GetDuramadre().Atravesar();//1 Duramadre
            exp.GetDuramadre().Abandonar();

            #endregion

            var informe = exp.TerminarSimulacion();
            conn.Verify(x => x.PausarSensado(), Times.Once);

            Assert.AreEqual(2, informe.TejidoAdiposo);
            Assert.AreEqual(0, informe.L2);

            Assert.AreEqual(1, informe.L3Arriba);
            Assert.AreEqual(2, informe.L3Abajo);

            Assert.AreEqual(0, informe.L4ArribaIzquierda);
            Assert.AreEqual(3, informe.L4ArribaDerecha);
            Assert.AreEqual(1, informe.L4ArribaCentro);
            Assert.AreEqual(0, informe.L4Abajo);

            Assert.AreEqual(0, informe.L5);
            Assert.AreEqual(1, informe.Duramadre);
        }
    }

}
