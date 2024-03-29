﻿using System;

namespace LumbApp.Conectores.ConectorFS
{
    public interface IConectorFS
    {
        bool GuardarObjectoComoArchivoDeTexto<T>(String path, T objectToBeSavedAsJson);

        T LevantarArchivoDeTextoComoObjeto<T>(String path);
    }
}
