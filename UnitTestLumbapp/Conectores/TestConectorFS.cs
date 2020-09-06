using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Conectores.ConectorFS;
using Moq;
using System.IO.Abstractions;

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

    public class ExampleClass {
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
