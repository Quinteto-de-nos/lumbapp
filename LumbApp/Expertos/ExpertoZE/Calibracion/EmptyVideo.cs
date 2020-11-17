using Microsoft.Kinect;

namespace LumbApp.Expertos.ExpertoZE.test
{
    /// <summary>
    /// EmptyVideo es para la etapa de calibracion, para poder iniciar una simulacion
    /// y con eso testear los limites de la zona esteril pero sin guardar un video.
    /// Esto tambien evita un problema de que no encuentra la .dll de Accord (como
    /// realmente no hace falta en esta etapa, con EmptyVideo logramos sacarla)
    /// </summary>
    class EmptyVideo : IVideo
    {
        public EmptyVideo() { }
        public void addFrame(ColorImageFrame frame) { }

        public void Save() { }
    }
}
