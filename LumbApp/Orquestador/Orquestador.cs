﻿using LumbApp.Conectores.ConectorKinect;
using LumbApp.Conectores.ConectorSI;
using LumbApp.Enums;
using LumbApp.Expertos.ExpertoSI;
using LumbApp.Expertos.ExpertoZE;
using LumbApp.GUI;
using LumbApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Orquestador {
    public class Orquestador : IOrquestador {
		public GUIController GUI { get; set; }
		
		private ExpertoZE expertoZE;
		private ExpertoSI expertoSI;

		private Models.DatosPracticante datosPracticante;
		private ModoSimulacion modoSeleccionado;

		/// <summary>
		/// Constructor del Orquestrador.
		/// Se encarga de construir los expertos y la GUI manejando para manejar la comunicación entre ellos.
		/// </summary>
		public Orquestador(GUIController gui)
		{
			GUI = gui;

			expertoZE = new ExpertoZE(new ConectorKinect());

			expertoSI = new ExpertoSI(new ConectorSI());

			//Inicializar();
		}

		//public void Start () {
		//	GUI = new GUIController(this);
		//}
		public void SetDatosDeSimulacion(Models.DatosPracticante datosPracticante, ModoSimulacion modo)
		{
			this.datosPracticante = datosPracticante;
		
			this.modoSeleccionado = modo;
		}

		public void IniciarSimulacion () {

			expertoZE.IniciarSimulacion();

			expertoSI.IniciarSimulacion();
		}

		public async Task<bool> Inicializar() {
			//La GUI ya esta mostrando msje "inicializando"

			//Inicializar Experto ZE
			if (!expertoZE.Inicializar()) {
				expertoZE.CambioZE += CambioZE; 
				//Ver si hay que desuscribir en caso de error
				//Si: Inicializar Experto ZE tuvo algún problema:
				//**Pedir a la GUI mostrar error de inicialización de ZE
				//**	  o avisar que hubo un error en la inicialización de la ZE
				GUI.MostrarErrorDeConexion("Ocurrio un Error con la Kinect");
				return false;
			}	
			//Si terminó bien, continuar...

			if (!expertoSI.Inicializar())
            {
				expertoSI.CambioSI += CambioSI;
				GUI.MostrarErrorDeConexion("Ocurrio un Error con el Arduino");
				return false;
			}
			//Si: Inicializar Experto SI tuvo algún problema:
			//**Pedir a la GUI mostrar error de inicialización de SI
			//**	  o avisar que hubo un error en la inicialización de los SI

			//Pedir a la GUI mostrar pantalla de ingreso de datos a travez de un evento
			GUI.SolicitarDatosPracticante();

			return true;
		}

		public void TerminarSimulacion()//Funcion llamada por la GUI, devuelve void, respuesta por evento
        {
			//ExpertoZE.terminarSimulacion()
			//ExpertoSI.terminarSimulacion()
			InformeZE informeZE; //Llamado a funcion del experto
			InformeSI informeSI; //Llamado a funcion del experto
								 //Guardar informe en archivo
								 //Informar a GUI con informe con un evento, que pase si el informe se genero bien, y si se guardó  bien (bool, bool) 
		}

		private void CambioSI(object sender, CambioSIEventArgs e) {
			//comunicar los cambios a la GUI levantando un evento
			//Decidir que comunicamos dependiendo del modo
		}

		private void CambioZE(object sender, CambioZEEventArgs e)
		{
			//comunicar los cambios a la GUI levantando un evento
			//Decidir que comunicamos dependiendo del modo
		}

	}
}
