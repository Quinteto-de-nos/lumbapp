using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Conectores.ConectorFS
{
    public interface IConectorFS
    {
        bool GuardarObjectoComoArchivoDeTexto<T>(String path, T objectToBeSavedAsJson);

        T LevantarArchivoDeTextoComoObjeto<T>(String path);
    }
}
