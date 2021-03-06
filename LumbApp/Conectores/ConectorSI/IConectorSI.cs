﻿using System;

namespace LumbApp.Conectores.ConectorSI
{
    public interface IConectorSI
    {
        bool Conectar();
        bool CheckearComunicacion();
        void ActivarSensado();
        void PausarSensado();
        void Desconectar();
        //Es necesario declarar el evento en la interfaz del conector para que pueda ser registrado por el experto
        event EventHandler<DatosSensadosEventArgs> HayDatos;
    }
}
