using System;
using System.Threading.Tasks;

namespace LumbApp.Conectores.ConectorSI
{
    public class ConectorSIMock : IConectorSI
    {
        private bool sensando = false;
        private readonly bool shouldInit;

        public event EventHandler<DatosSensadosEventArgs> HayDatos;

        /// <summary>
        /// Crea el Conector Mockeado.
        /// </summary>
        /// <param name="shouldInit"> Recibe un booleano que de indica si debe iniciar bien o no.</param>
        public ConectorSIMock(bool shouldInit) { this.shouldInit = shouldInit; }
        public bool Conectar() { return shouldInit; }
        public void ActivarSensado()
        {
            sensando = shouldInit;
            //if (sensando)
            //    simulateAsync();
        }
        public void Desconectar() { }
        public void PausarSensado() { sensando = false; }

        public bool CheckearComunicacion()
        {
            Task.Delay(5000);
            return shouldInit;
        }

        private void sendEvent(String datosSensados)
        {
            var datosDelEvento = new DatosSensadosEventArgs(datosSensados);
            HayDatos?.Invoke(this, datosDelEvento);
        }

        /// <summary>
        /// simulateAsync es la funcion que simula recibir datos sensados y los envía al experto mediante sendEvent.
        /// El caso basico es: 
        ///     1) atraviesa tejido adiposo
        ///     2) roza vertebra L4 arriba centro
        ///     3) atravieza duramadre
        /// </summary>
        /// <returns></returns>
        private async Task simulateAsync()
        {
            //Arranca simulacion
            sensando = true;
            String datosSensados;

            //************ A partir de aca escribir toda la simulacion ****************

            //AUN NO INTRODUJO LA AGUJA
            await Task.Delay(1000);
            datosSensados = "111111111111";
            sendEvent(datosSensados);
            //AUN NO INTRODUJO LA AGUJA
            await Task.Delay(1000);
            datosSensados = "111111111111";
            sendEvent(datosSensados);

            //ATRAVIESA EL TEJIDO ADIPOSO
            await Task.Delay(1000);
            datosSensados = "110111111111";
            sendEvent(datosSensados);
            //SIGUE ATRAVESANDO EL TEJIDO ADIPOSO
            await Task.Delay(1000);
            datosSensados = "110111111111";
            sendEvent(datosSensados);
            //SIGUE ATRAVESANDO EL TEJIDO ADIPOSO
            await Task.Delay(1000);
            datosSensados = "110111111111";
            sendEvent(datosSensados);

            //ROZA L4 ARRIBA CENTRO
            await Task.Delay(1000);
            datosSensados = "110111110111";
            sendEvent(datosSensados);
            //SIGUE ROZANDO L4 ARRIBA CENTRO
            await Task.Delay(1000);
            datosSensados = "110111110111";
            sendEvent(datosSensados);
            //SIGUE ROZANDO L4 ARRIBA CENTRO
            await Task.Delay(1000);
            datosSensados = "110111110111";
            sendEvent(datosSensados);
            //SIGUE ROZANDO L4 ARRIBA CENTRO
            await Task.Delay(1000);
            datosSensados = "110111110111";
            sendEvent(datosSensados);

            //ATRAVIESA LA DURAMADRE
            await Task.Delay(1000);
            datosSensados = "110111110110";
            sendEvent(datosSensados);
            //SIGUE ATRAVESANDO LA DURAMADRE
            await Task.Delay(1000);
            datosSensados = "110111110110";
            sendEvent(datosSensados);
            //SIGUE ATRAVESANDO LA DURAMADRE
            await Task.Delay(1000);
            datosSensados = "110111110110";
            sendEvent(datosSensados);

            // ************** Fin seccion simulacion *************

            //Termina simulacion
            await Task.Delay(5000);
            Console.WriteLine("Finished mocked simulation");
            sensando = false;
        }
    }
}
