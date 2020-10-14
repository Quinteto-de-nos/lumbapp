using Microsoft.Kinect;

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
