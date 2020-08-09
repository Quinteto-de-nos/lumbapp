using LumbApp.Conectores.ConectorKinect;
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
		public GUIController GUIController { get; set; }

		private ExpertoZE expertoZE;
		private ExpertoSI expertoSI;

		private Models.DatosPracticante datosPracticante;
		private ModoSimulacion modoSeleccionado;

		/// <summary>
		/// Constructor del Orquestrador.
		/// Se encarga de construir los expertos y la GUI manejando para manejar la comunicación entre ellos.
		/// </summary>
		public Orquestador (GUIController gui) {
			if (gui == null)
				throw new Exception("Gui no puede ser null. Necesito un GUIController para crear un Orquestador.");
			
			GUIController = gui;

			expertoZE = new ExpertoZE(new ConectorKinect());

			expertoSI = new ExpertoSI(new ConectorSI());

		}

		public void SetDatosDeSimulacion(Models.DatosPracticante datosPracticante, ModoSimulacion modo)
		{
			this.datosPracticante = datosPracticante;
		
			this.modoSeleccionado = modo;
		}

		/// <summary>
		/// IniciarSimulacion: Informa a los expertos que deben inicar el sensado.
		/// 
		/// </summary>
		/// <param name="datosPracticante"> Datos ingresados del practicante </param>
		/// <param name="modo"> Modo de practica seleccionado (Guiado=0, Evaluacion=1) </param>
		public void IniciarSimulacion () {

			expertoZE.IniciarSimulacion();

			expertoSI.IniciarSimulacion();


			if(modoSeleccionado == ModoSimulacion.ModoGuiado)
				GUIController.IniciarSimulacionModoGuiado();
			else
				GUIController.IniciarSimulacionModoEvaluacion();
		}

		/// <summary>
		/// Inicializar: Se encarga de mandar a inicializar los expertos y pedir la pantalla de ingreso de datos.
		/// - Si algun experto no pudo inicializar correctamente, envía a la GUI un mensaje de error.
		/// - Sucede al abrir la aplicación. La GUI muestra un gif "checkeando sensores" y nos manda a inicializar.
		/// </summary>
		/// <returns></returns>
		public async Task Inicializar() {

			try {
				//INICIALIZAR EXPERTO ZE
				expertoZE.CambioZE += CambioZE; //suscripción al evento CambioZE
				if (!expertoZE.Inicializar()) {
					expertoZE.CambioZE -= CambioZE; //Ver si hay que desuscribir en caso de error
					throw new Exception("No se pudo detectar correctamente la kinect.");
				}

				//INICIALIZAR EXPERTO SI
				expertoSI.CambioSI += CambioSI; //suscripción al evento CambioSI
				if (!expertoSI.Inicializar()) {
					expertoSI.CambioSI -= CambioSI;
					throw new Exception("No se pudo detectar correctamente los sensores internos.");
				}

				//Mostrar pantalla de ingreso de datos
				GUIController.SolicitarDatosPracticante();

			} catch (Exception ex) {
				GUIController.MostrarErrorDeConexion(ex.Message);
			}
			
		}

		/// <summary>
		/// Terminar simulación: indica a los expertos que dejen de tomar estadísticas y devuelvan la informacion que obtuvo cada uno.
		/// - Con la informacion regida genera un informe general que se envia al final feedbacker y se guarda.
		/// - Si el informe general se genero y guardo bien, levanta un evento ue es atrapado por la GUI para decirle que todo salio bien.
		/// </summary>
		public void TerminarSimulacion() { //Funcion llamada por la GUI, devuelve void, respuesta por evento
			//ExpertoZE.terminarSimulacion()

			InformeZE informeZE; //Llamado a funcion del experto
			
			InformeSI informeSI = expertoSI.TerminarSimulacion();
			
			//Guardar informe en archivo
			//Informar a GUI con informe con un evento, que pase si el informe se genero bien, y si se guardó  bien (bool, bool) 
		}

		/// <summary>
		/// CambioSI: atrapa los eventos que indican un cambio en el sensado interno
		/// - En MODO GUIADO, informa los cambios a la GUI para que esta pueda mostrarlos
		/// - En MODO EVALUACION, ... (creo que no hace nada, ya que el informe lo arman los expertos)
		/// </summary>
		/// <param name="sender"> Remitente del evento </param>
		/// <param name="datosDelEvento"> Datos del cambio (capas, vertebras y correctitud del camino) </param>
		private void CambioSI(object sender, CambioSIEventArgs datosDelEvento) {
			//comunicar los cambios a la GUI levantando un evento
			//Decidir que comunicamos dependiendo del modo
		}

		private void CambioZE(object sender, CambioZEEventArgs e) {
			//comunicar los cambios a la GUI levantando un evento
			//Decidir que comunicamos dependiendo del modo
		}

        public ExpertoSI GetExpertoSI () {
			return expertoSI;
        }
		public void SetExpertoSI (ExpertoSI exp) {
			this.expertoSI = exp;
        }
    }
}
