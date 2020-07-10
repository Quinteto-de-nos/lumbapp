using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Conectores.ConectorKinect
{
    class ConectorKinect
    {
        public KinectSensor _sensor;
        public Skeleton[] _bodies = new Skeleton[6];
        private AllFramesReadyEventArgs framesReadyEvent;

        public ConectorKinect() { }

        /// <summary>
        /// Inicializa la conexion con la Kinect. Si no encuentra una kinect, tira una excepcion
        /// </summary>
        /// Severity	Code	Description	Project	File	Line	Suppression State
        public void Conectar(EventHandler<AllFramesReadyEventArgs> sub)
        {
            _sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
            if (_sensor == null)
                throw new KinectNotFoundException();

            _sensor.ColorStream.Enable();
            _sensor.SkeletonStream.Enable();
            _sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;

            _sensor.AllFramesReady += Sensor_AllFramesReady;
            if (sub != null)
                _sensor.AllFramesReady += sub;

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

        void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e) {
            this.framesReadyEvent = e;
        }


    }

    class KinectNotFoundException: Exception
    {
        public KinectNotFoundException(): base("No encontre ninguna Kinect conectada") { }
    }
}
