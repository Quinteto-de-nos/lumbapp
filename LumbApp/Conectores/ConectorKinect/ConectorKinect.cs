using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Conectores.ConectorKinect
{
    class ConectorKinect
    {
        private static KinectSensor _sensor;
        private static Skeleton[] _bodies = new Skeleton[6];

        public ConectorKinect() { }

        public void Inicializar()
        {
            _sensor = KinectSensor.KinectSensors.Where(s => s.Status == KinectStatus.Connected).FirstOrDefault();
            if (_sensor == null)
                throw new Exception("No encontre ninguna Kinect conectada");

            _sensor.ColorStream.Enable();
            _sensor.SkeletonStream.Enable();
            _sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;

            _sensor.AllFramesReady += Sensor_AllFramesReady;

            _sensor.Start();

        }

        void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e) { }


    }
}
