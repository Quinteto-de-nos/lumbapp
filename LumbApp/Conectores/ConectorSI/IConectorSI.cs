using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Conectores.ConectorSI {
    public interface IConectorSI {
        void Conectar ();
        bool ChekearSensado ();
        void ActivarSensado ();
    }
}
