using LumbApp.Orquestador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.GUI {
    public class GUIController {
        private IOrquestador _orquestrator { get; set; }
        
        public GUIController () { }

        public GUIController (Orquestador.Orquestador orquestador) {
            _orquestrator = orquestador;
        }

        public void Inicializar () { }

        /// <summary>
        /// Lo llama el orquestador  para mostrar por que fallo la inicializacion de los sensores
        /// </summary>
        /// <param name="mensaje"></param>
        public void MostrarErrorDeConexion (string mensaje) {
            
        }

        public void SolicitarDatosPracticante () {

        }
    }
}
