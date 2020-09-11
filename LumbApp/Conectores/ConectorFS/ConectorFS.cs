using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
            Console.WriteLine("Objeto a ser salvado: " + stringToBeSaved);
            try {
                _fileSystem.File.WriteAllText(path, stringToBeSaved);
                Console.WriteLine("Archivo salvado correctamente en: "+path);
                return true;
            }catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
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
            Console.WriteLine("Levantando archivo de: " + path);
            try
            {
                String jsonStringToRead = _fileSystem.File.ReadAllText(path);
                Console.WriteLine("Archivo levantado correctamente: " + jsonStringToRead);
                return JsonSerializer.Deserialize<T>(jsonStringToRead);
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return default;
            }
        }
    }
}
