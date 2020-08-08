using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoZE{
    public class CambioZEEventArgs : EventArgs{

        EstadoMano _estadoI;
        EstadoMano _estadoD;
        int _cantContaminadoI;
        int _cantContaminadoD;
        int _cantContaminadoTotal;
        bool _contaminada;

        public CambioZEEventArgs(EstadoMano estadoI, EstadoMano estadoD, int cantContaminadoI, int cantContaminadoD, int cantContaminadoTotal, bool contaminada)
        {
            _estadoI = estadoI;
            _estadoD = estadoD;
            _cantContaminadoI = cantContaminadoI;
            _cantContaminadoD = cantContaminadoD;
            _cantContaminadoTotal = cantContaminadoTotal;
            _contaminada = contaminada;
        }
    }
}
