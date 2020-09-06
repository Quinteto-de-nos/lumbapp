using Microsoft.Kinect;
using System;

namespace LumbApp.Expertos.ExpertoZE
{
    public class ZonaEsteril
    {
        private Calibracion cal;
        /// <summary>
        /// Esta matriz indica como crear cada cara. Cada numero es un indice para
        /// buscar los puntos. Ej: 0 indica el punto cal.zonaEsteril[0]
        /// Por cada cara guardamos 3 puntos en el orden que se deben pasar a
        /// distToPlane()
        /// </summary>
        private int[,] caras = {
                {0,4,1}, //Cara 1
                {1,5,2}, //Cara 2
                {2,6,3}, //Cara 3
                {0,3,4}, //Cara 4
                {4,7,5}, //Cara 5
                {0,1,3} //Cara 6
            };

        /// <summary>
        /// Cantidad de veces que fue contaminada, no importa por que mano.
        /// </summary>
        public int Contaminacion { get; private set; }

        #region Metodos ZE
        public ZonaEsteril(Calibracion cal)
        {
            if (cal == null || cal.zonaEsteril == null || cal.zonaEsteril.Length < 8)
                throw new Exception("Datos de calibracion mal formados.");
            this.cal = cal;
        }

        public void Contaminar()
        {
            Contaminacion++;
        }

        public void Resetear()
        {
            Contaminacion = 0;
        }

        public bool EstaDentro(SkeletonPoint pos)
        {
            // Recorro las 6 caras. Cada una se define por 3 puntos (esquinas de la ZE)
            // El array cal.zonaEsteril tiene los puntos para cada esquina de la ZE, de
            // s0 a s7 en orden. La matriz caras (6 caras x 3 puntos) tiene los indices
            // de los puntos que definen cada cara, listos para usar contra 
            // cal.zonaEsteril.
            bool result = true;
            for (int i = 0; i < 6; i++)
            {
                double dist = distToPlane(
                    cal.zonaEsteril[caras[i, 0]],
                    cal.zonaEsteril[caras[i, 1]],
                    cal.zonaEsteril[caras[i, 2]],
                    pos);
                result &= dist > 0;
            }

            return result;
        }

        #endregion

        #region Metodos algebraicos

        private double distToPlane(SkeletonPoint centro, SkeletonPoint right, SkeletonPoint left, SkeletonPoint test)
        {
            //Armo el plano
            var normal = cruz(menos(right, centro), menos(left, centro));
            var a = normal.X;
            var b = normal.Y;
            var c = normal.Z;
            var d = -a * centro.X - b * centro.Y - c * centro.Z;

            //Calculo la distancia
            return (a * test.X + b * test.Y + c * test.Z + d) / modulo(normal);
        }

        private SkeletonPoint cruz(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint();
            res.X = a.Y * b.Z - a.Z * b.Y; //a2b3-a3b2
            res.Y = a.Z * b.X - a.X * b.Z; //a3b1-a1b3
            res.Z = a.X * b.Y - a.Y * b.X; //a1b2-a2b1
            return res;
        }

        private SkeletonPoint menos(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint();
            res.X = a.X - b.X;
            res.Y = a.Y - b.Y;
            res.Z = a.Z - b.Z;
            return res;
        }

        private double modulo(SkeletonPoint a)
        {
            return Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2) + Math.Pow(a.Z, 2));
        }

        #endregion
    }
}
