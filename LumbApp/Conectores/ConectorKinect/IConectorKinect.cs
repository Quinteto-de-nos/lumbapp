using System;
using Microsoft.Kinect;

namespace LumbApp.Conectores.ConectorKinect
{
    public interface IConectorKinect
    {
        void Conectar();
        void Desconectar();
        void SubscribeFramesReady(EventHandler<AllFramesReadyEventArgs> subscriber);
    }
}
