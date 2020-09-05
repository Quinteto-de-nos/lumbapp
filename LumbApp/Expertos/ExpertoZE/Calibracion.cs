using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoZE
{
    public class Calibracion
    {
        /// <summary>
        /// Puntos de la zona esteril. Deben ser los 8 puntos y estar en orden.
        /// </summary>
        public SkeletonPoint[] zonaEsteril { get; set; }
        public Calibracion() { }
        public Calibracion(SkeletonPoint[] points)
        {
            zonaEsteril = points;
        }
    }
}
