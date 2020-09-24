using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoZE
{
    class ExpertoZEMock : IExpertoZE
    {
        public event EventHandler<CambioZEEventArgs> CambioZE;

        private bool shouldInit;
        private bool simulando;
        private ZonaEsteril zonaEsteril;
        private Mano manoDerecha = new Mano();
        private Mano manoIzquierda = new Mano();

        public ExpertoZEMock(bool shouldInit)
        {
            this.shouldInit = shouldInit;
        }

        public void Finalizar(){}

        public bool Inicializar()
        {
            return shouldInit;
        }

        public bool IniciarSimulacion()
        {
            SkeletonPoint[] points = {
                newPoint(0,0,0), newPoint(1,0,0), newPoint(1, 0, 1),  newPoint(0,0,1),
                newPoint(0,1,0), newPoint(1,1,0), newPoint(1, 1, 1),  newPoint(0,1,1)};
            Calibracion cal = new Calibracion(points);
            zonaEsteril = new ZonaEsteril(cal);
            manoDerecha = new Mano();
            manoIzquierda = new Mano();

            if (shouldInit)
                simulateAsync();
            return shouldInit;
        }

        private SkeletonPoint newPoint(float x, float y, float z)
        {
            SkeletonPoint p = new SkeletonPoint();
            p.X = x;
            p.Y = y;
            p.Z = z;
            return p;
        }

        public InformeZE TerminarSimulacion()
        {
            if (!simulando)
                return new InformeZE(0, 0, 0);

            simulando = false;
            return new InformeZE(zonaEsteril.Contaminacion, manoDerecha.VecesContamino, manoIzquierda.VecesContamino);
        }

        private async Task simulateAsync()
        {
            //Arranca simulacion
            simulando = true;

            //************ A partir de aca escribir toda la simulacion ****************

            //Ej: mano derecha se trackea a los 5 segundos
            await Task.Delay(5000);
            manoDerecha.ActualizarTrack(true);
            sendEvent();

            //Ej: entra a los 10 segundos
            await Task.Delay(5000);
            manoDerecha.Entrar();
            sendEvent();

            //Ej: sale a los 20 segundos
            await Task.Delay(10000);
            manoDerecha.Salir();
            sendEvent();

            //Ej: entra y contamina a los 30 segundos (sendEvent no marca el contaminando ahora)
            await Task.Delay(10000);
            manoDerecha.Entrar();
            zonaEsteril.Contaminar();
            var args = new CambioZEEventArgs(manoDerecha, manoIzquierda);
            args.VecesContaminado = zonaEsteril.Contaminacion;
            args.ContaminadoAhora = true;
            CambioZE.Invoke(this, args);

            // ************** Fin seccion simulacion *************

            //Termina simulacion
            await Task.Delay(10000);
            Console.WriteLine("Finished mocked simulation");
            simulando = false;
        }

        private void sendEvent()
        {
            var args = new CambioZEEventArgs(manoDerecha, manoIzquierda);
            args.VecesContaminado = zonaEsteril.Contaminacion;
            CambioZE.Invoke(this, args);
        }
    }
}
