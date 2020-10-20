using System;
using System.IO.Abstractions;
using System.Text.Json;

namespace LumbApp.Conectores.ConectorFS
{
    public class ConectorFS : IConectorFS
    {
        private readonly IFileSystem _fileSystem;
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ConectorFS() : this(new FileSystem()) { }

        /// <summary>
        /// Constructor para testing
        /// </summary>
        /// <param name="fileSystem"></param>
        public ConectorFS(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        /// <summary>
        /// Método que guarda un objeto en formato Json en un archivo de texto
        /// </summary>
        /// <typeparam name="T">Clase del objeto a guardar</typeparam>
        /// <param name="path">Ruta para guardar el objeto</param>
        /// <param name="objectToBeSavedAsJson">Objeto a guardar</param>
        /// <returns>true si se guardó correctamente, false en caso contrario</returns>
        public bool GuardarObjectoComoArchivoDeTexto<T>(string path, T objectToBeSavedAsJson)
        {
            String stringToBeSaved = JsonSerializer.Serialize<T>(objectToBeSavedAsJson);
            Console.WriteLine("FS: Objeto a ser salvado: " + stringToBeSaved);
            try
            {
                _fileSystem.File.WriteAllText(path, stringToBeSaved);
                Console.WriteLine("FS: Archivo salvado correctamente en: " + path);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("FS: Exception: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Método que levanta un documento en formato Json y devuelve dicho objeto
        /// </summary>
        /// <typeparam name="T">Clase del objeto devuelto</typeparam>
        /// <param name="path">Ruta del archivo a levantar</param>
        /// <returns>El objeto deserializado</returns>
        public T LevantarArchivoDeTextoComoObjeto<T>(string path)
        {
            Console.WriteLine("FS: Levantando archivo de: " + path);

            string jsonStringToRead = _fileSystem.File.ReadAllText(path);
            Console.WriteLine("FS: Archivo levantado correctamente: " + jsonStringToRead);
            return JsonSerializer.Deserialize<T>(jsonStringToRead);
        }
    }
}
