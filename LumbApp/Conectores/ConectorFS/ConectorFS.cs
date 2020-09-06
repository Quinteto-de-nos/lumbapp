using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LumbApp.Conectores.ConectorFS
{
    class ConectorFS : IConectorFS
    {
        public bool GuardarObjectoComoArchivoDeTexto<T>(string path, T objectToBeSavedAsJson)
        {
            String jsonStringToBeSaved = JsonSerializer.Serialize<T>(objectToBeSavedAsJson);
        }

        public T LevantarArchivoDeTextoComoObjeto<T>(string path)
        {
            throw new NotImplementedException();
        }
    }
}
