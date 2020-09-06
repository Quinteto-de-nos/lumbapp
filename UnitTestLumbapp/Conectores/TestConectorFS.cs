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
