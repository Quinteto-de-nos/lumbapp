using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Orquestador {
    class Orquestador {
		private ExpertoZE expertoZE;
		private ExpertoSI expertoSI;

		/// <summary>
        /// Constructor del Orquestrador.
        /// Se encarga de construir los expertos y la GUI manejando para manejar la comunicación entre ellos.
        /// </summary>
        /// <param name="kinect">Conector a la kinect</param>
		public Orquestador () {
			IConectorKinect conZE = new ConectorKinect();
			expertoZE = new ExpertoZE(conZE);
			
			IConectorSI conSI = new ConectorSI();
			expertoSI = new ExpertoSI(conSI);

			inicializar();
			//Pedir a la GUI mostrar pantalla de ingreso de datos
			//	   o avisar que terminó de inicializar

			if (true /*GUI: hay evento de inicio*/) {
				iniciar();
			}
		}

		public bool inicializar () {
			//Pedir a la GUI mostrar msje "inicializando"

			//Inicializar Experto ZE
			//Si: Inicializar Experto ZE tuvo algún problema:
			//**Pedir a la GUI mostrar error de inicialización de ZE
			//**	  o avisar que hubo un error en la inicialización de la ZE
			//Si terminó bien, continuar...

			//Inicializar Experto SI
			//Si: Inicializar Experto SI tuvo algún problema:
			//**Pedir a la GUI mostrar error de inicialización de SI
			//**	  o avisar que hubo un error en la inicialización de los SI
			//Si terminó bien, continuar...

			return true;
		}

		public void iniciar () {
			//¿Habría que limpiar la info del anterior o lo hace cada experto al iniciar?

			//Iniciar Experto ZE
			//Iniciar Experto SI
			//Avisar a la GUI que comenzó la simulación


		}
	}
}
