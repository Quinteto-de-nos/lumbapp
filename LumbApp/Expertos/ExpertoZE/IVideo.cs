using Microsoft.Kinect;

namespace LumbApp.Expertos.ExpertoZE
{
    public interface IVideo
    {
        void addFrame(ColorImageFrame frame);
        void Save();
    }
}
