using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Orquestador {
    public class Orquestador : IOrquestador {
		//public GUIController GUI { get; set; }

		private ExpertoZE expertoZE;
		private ExpertoSI expertoSI;

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

		public void Start() {
			//GUI = new GUIController(this);
		}

		public bool Inicializar () {
			//Pedir a la GUI mostrar msje "inicializando"

			//Inicializar Experto ZE
			if (!expertoZE.Inicializar())
				//Si: Inicializar Experto ZE tuvo algún problema:
				//**Pedir a la GUI mostrar error de inicialización de ZE
				//**	  o avisar que hubo un error en la inicialización de la ZE
				return false;
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

		private void IniciarSimulacion () {/// acá hay que poner el nombre del evento que la GUI dispara al iniciar la simulacion
			Iniciar();

			bool run = true;

			while (run) { 
				
			}
			
		}

		public void Iniciar () {

			expertoZE.CambioZE += CambioZE;
			expertoZE.IniciarSimulacion();

			expertoSI.CambioSI += CambioSI;
			expertoSI.IniciarSimulacion();
			//Avisar a la GUI que comenzó la simulación
		}

		public void TerminarSimulacion()//Funcion llamada por la GUI
        {
			InformeZE informeZE; //Llamado a funcion del experto
			InformeSI informeSI; //Llamado a funcion del experto
			//Informar a GUI con informe
			//Guardar informe en archivo
        }

		private void CambioSI(object sender, CambioSIEventArgs e) {
			//comunicar los cambios a la GUI
		}

		private void CambioZE(object sender, CambioZEEventArgs e)
		{
			//comunicar los cambios a la GUI
		}

	}
}
