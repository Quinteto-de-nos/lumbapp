﻿using LumbApp.Enums;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Orquestador
{
    public interface IOrquestador
    {
        //void Start ();
        void SetDatosDeSimulacion(Models.DatosPracticante datosPracticante, ModoSimulacion modo);
        void IniciarSimulacion();
        Task Inicializar();
        Task TerminarSimulacion();
    }
}
