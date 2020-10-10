using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Conectores.ConectorFS;
using Moq;
using System.IO.Abstractions;
using LumbApp.Expertos.ExpertoZE;

namespace UnitTestLumbapp.Conectores
{
    [TestClass]
    public class TestConectorFS
    {
        /// <summary>
        /// Test que verifica el guardado de un objeto genericamente
        /// </summary>
        [TestMethod]
        public void TestGuardadoDeObjeto()
        {
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.WriteAllText(It.IsAny<String>(), It.IsAny<String>())).Verifiable();
            ExampleClass example = new ExampleClass(4586, "Hello world", true);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            bool isOk = conector.GuardarObjectoComoArchivoDeTexto<ExampleClass>("C:\\file.txt", example);
            filesystem.Verify(x => x.File.WriteAllText(It.IsAny<String>(), It.IsAny<String>()), Times.Once);
            Assert.IsTrue(isOk);
        }

        /// <summary>
        /// Test que verifica el guardado de un objeto en especifico
        /// </summary>
        [TestMethod]
        public void TestGuardadoDeObjetoReal()
        {
            String textToBeReturned = "{\"_intExample\":123,\"_stringExample\":\"example\",\"_boolExample\":true}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.WriteAllText(It.IsAny<String>(), textToBeReturned)).Verifiable();
            ExampleClass example = new ExampleClass(123, "example", true);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            bool isOk = conector.GuardarObjectoComoArchivoDeTexto<ExampleClass>("C:\\file.txt", example);
            filesystem.Verify(x => x.File.WriteAllText(It.IsAny<String>(), It.IsAny<String>()), Times.Once);
            Assert.IsTrue(isOk);
        }

        /// <summary>
        /// Test que verifica el guardado de un objeto cuando no se puede generar el Json o no se puede guardar
        /// </summary>
        [TestMethod]
        public void TestGuardadoDeObjetoLanzaException()
        {
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.WriteAllText(It.IsAny<String>(), It.IsAny<String>())).Throws(new Exception());
            ExampleClass example = new ExampleClass(4586, "Hello world", true);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            bool isOk = conector.GuardarObjectoComoArchivoDeTexto<ExampleClass>("C:\\file.txt", example);
            Assert.IsFalse(isOk);
        }

        /// <summary>
        /// Test que verifica el levantado de un objeto genericamente
        /// </summary>
        [TestMethod]
        public void TestLevantadoDeObjeto()
        {
            String textToBeReturned = "{}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Returns(textToBeReturned);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            ExampleClass example = conector.LevantarArchivoDeTextoComoObjeto<ExampleClass>("C:\\file.txt");
            Assert.IsNotNull(example);
            Assert.AreEqual(0, example._intExample);
            Assert.IsNull(example._stringExample);
            Assert.AreEqual(false, example._boolExample);
        }

        /// <summary>
        /// Test que verifica el levantado de un objeto en especifico
        /// </summary>
        [TestMethod]
        public void TestLevantadoDeObjetoReal()
        {
            String textToBeReturned = "{\"_intExample\":123, \"_stringExample\":\"example\",\"_boolExample\":true}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Returns(textToBeReturned);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            ExampleClass example = conector.LevantarArchivoDeTextoComoObjeto<ExampleClass>("C:\\file.txt");
            Assert.IsNotNull(example);
            Assert.AreEqual(123, example._intExample);
            Assert.AreEqual("example", example._stringExample);
            Assert.AreEqual(true, example._boolExample);
        }

        /// <summary>
        /// Test que verifica el levantado de un objeto Calibración
        /// </summary>
        [TestMethod]
        public void TestLevantadoDeObjetoCalibracion()
        {
            String textToBeReturned = "{\"zonaEsteril\":[{\"X\":-0.0531591699,\"Y\":-0.191040874,\"Z\":0.949000061},{\"X\":0.0752977654,\"Y\":-0.116173707,\"Z\":1.22900009},{\"X\":0.268609017,\"Y\":-0.144120455,\"Z\":1.11400008},{\"X\":0.140152082,\"Y\":-0.218987614,\"Z\":0.834000051},{\"X\":-0.0564637221,\"Y\":0.0991351306,\"Z\":0.872928083},{\"X\":0.0719932094,\"Y\":0.17400229,\"Z\":1.15292811},{\"X\":0.265304476,\"Y\":0.146055549,\"Z\":1.0379281},{\"X\":0.136847526,\"Y\":0.0711883903,\"Z\":0.757928073}]}";
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Returns(textToBeReturned);
            ConectorFS conector = new ConectorFS(filesystem.Object);
            Calibracion calibracion = conector.LevantarArchivoDeTextoComoObjeto<Calibracion>("C:\\file.txt");
            Assert.IsNotNull(calibracion);
            Assert.IsNotNull(calibracion.zonaEsteril);
            Assert.AreEqual(calibracion.zonaEsteril.Length, 8);
            Assert.AreEqual(calibracion.zonaEsteril[0].X, (float)-0.0531591699);
            Assert.AreEqual(calibracion.zonaEsteril[0].Y, (float)-0.191040874);
            Assert.AreEqual(calibracion.zonaEsteril[0].Z, (float)0.949000061);
        }

        /// <summary>
        /// Test que verifica el levantado de un objeto cuando falla la deserialización o el levantado de archivo
        /// </summary>
        [TestMethod]
        public void TestLevantadoDeObjetoLanzaException()
        {
            Mock<IFileSystem> filesystem = new Mock<IFileSystem>();
            filesystem.Setup(x => x.File.ReadAllText(It.IsAny<String>())).Throws(new Exception());
            ConectorFS conector = new ConectorFS(filesystem.Object);
            ExampleClass example = conector.LevantarArchivoDeTextoComoObjeto<ExampleClass>("C:\\file.txt");
            Assert.IsNull(example);
        }
    }

    public class ExampleClass
    {
        public int _intExample { get; set; }
        public String _stringExample { get; set; }
        public bool _boolExample { get; set; }

        public ExampleClass() { }

        public ExampleClass(int intExample, String stringExample, bool boolExample)
        {
            _intExample = intExample;
            _stringExample = stringExample;
            _boolExample = boolExample;
        }
    }
}
