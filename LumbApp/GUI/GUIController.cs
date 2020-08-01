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
    }
}
