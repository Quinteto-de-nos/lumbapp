using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Orquestador {
    public class Orquestador : IOrquestador {
		public GUIController GUI { get; set; }

		private ExpertoZE expertoZE;
		private ExpertoSI expertoSI;

		private DatosPracticante datosPracticante;

		/// <summary>
		/// Constructor del Orquestrador.
		/// Se encarga de construir los expertos y la GUI manejando para manejar la comunicación entre ellos.
		/// </summary>
		public Orquestador () {
			IConectorKinect conZE = new ConectorKinect();
			expertoZE = new ExpertoZE(conZE);
			
			IConectorSI conSI = new ConectorSI();
			expertoSI = new ExpertoSI(conSI);

			Inicializar();
		}

		public void Start () {
			GUI = new GUIController(this);
		}

		public void IniciarSimulacion (DatosPracticante datosPracticante) {
			this.datosPracticante = datosPracticante;

			expertoZE.IniciarSimulacion();

			expertoSI.CambioSI += CambioSI;
			expertoSI.IniciarSimulacion();
			//Avisar a la GUI que comenzó la simulación
		}

		public bool Inicializar () {
			//Pedir a la GUI mostrar msje "inicializando"

			//Inicializar Experto ZE
			//Si: Inicializar Experto ZE tuvo algún problema:
			//**Pedir a la GUI mostrar error de inicialización de ZE
			//**	  o avisar que hubo un error en la inicialización de la ZE
			//Si terminó bien, continuar...

			if (!expertoSI.Inicializar())
				return false;
				//Si: Inicializar Experto SI tuvo algún problema:
				//**Pedir a la GUI mostrar error de inicialización de SI
				//**	  o avisar que hubo un error en la inicialización de los SI

			//Pedir a la GUI mostrar pantalla de ingreso de datos
			//	   o avisar que terminó de inicializar

			return true;
		}

		private void CambioSI (object sender, CambioSIEventArgs e) {
			//comunicar los cambios a la GUI
		}

	}
}
