using Microsoft.Kinect;
using System;
using System.Linq;

namespace LumbApp.Conectores.ConectorKinect
{
    public class ConectorKinect : IConectorKinect
    {
        internal KinectSensor _sensor;

        public ConectorKinect() { }

        /// <summary>
        /// Inicializa la conexion con la Kinect. Si no encuentra una kinect, tira una excepcion
        /// </summary>
        public void Conectar()
        {
            _sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
            if (_sensor == null)
                throw new KinectNotFoundException();

            TransformSmoothParameters parameters = new TransformSmoothParameters();
            parameters.Smoothing = 0.75f;
            parameters.Correction = 0.3f;
            parameters.Prediction = 0.4f;
            parameters.JitterRadius = 0.05f;
            parameters.MaxDeviationRadius = 0.05f;

            _sensor.DepthStream.Enable();
            _sensor.ColorStream.Enable();
            _sensor.SkeletonStream.Enable(parameters);
            _sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;

            _sensor.Start();
        }

        /// <summary>
        /// Termina la conexion con la Kinect
        /// </summary>
        public void Desconectar()
        {
            if (_sensor != null)
                _sensor.Stop();
        }

        /// <summary>
        /// Subscribe un event handler al evento AllFramesReady de la Kinect.
        /// </summary>
        /// <param name="subscriber"></param>
        public void SubscribeFramesReady(EventHandler<AllFramesReadyEventArgs> subscriber)
        {
            if (_sensor == null)
                throw new KinectNotConnectedException();
            _sensor.AllFramesReady += subscriber;
        }
    }

    public class KinectNotFoundException : Exception
    {
        public KinectNotFoundException() : base("No encontre ninguna Kinect conectada") { }
    }
    public class KinectNotConnectedException : Exception
    {
        public KinectNotConnectedException() : base("No hay ninguna Kinect conectada. Proba Conectar() primero.") { }
    }
}
