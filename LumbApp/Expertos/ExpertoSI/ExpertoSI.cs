using LumbApp.Conectores.ConectorSI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoSI {
    public class ExpertoSI {
        private IConectorSI sensoresInternos;
        // RegistroEstado registroEstado;

        public ExpertoSI (IConectorSI sensoresInternos) {
            if (sensoresInternos == null)
                throw new Exception("Sensores no puede ser null. Necesito un conector a los sensores para crear un experto en sensores internos");
            this.sensoresInternos = sensoresInternos;
        }

    }
}
