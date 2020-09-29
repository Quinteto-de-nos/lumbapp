using Accord.Video.FFMPEG;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Expertos.ExpertoZE
{
    class Video
    {
        private List<Bitmap> frames;

        internal Video()
        {
            frames = new List<Bitmap>();
        }

        internal void addFrame(ColorImageFrame frame)
        {
            frames.Add(ImageToBitmap(frame));
        }

        internal void save(string path)
        {
            VideoFileWriter writer = new VideoFileWriter();
            writer.Open(path, 640, 480, 30, VideoCodec.MPEG4);
            foreach (var frame in frames)
                writer.WriteVideoFrame(frame);
            writer.Close();
        }

        private Bitmap ImageToBitmap(ColorImageFrame Image)
        {
            byte[] pixeldata = new byte[Image.PixelDataLength];
            Image.CopyPixelDataTo(pixeldata);

            Bitmap bmap = new Bitmap(Image.Width, Image.Height, PixelFormat.Format32bppRgb);
            BitmapData bmapdata = bmap.LockBits(
                new Rectangle(0, 0, Image.Width, Image.Height),
                ImageLockMode.WriteOnly,
                bmap.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;

            Marshal.Copy(pixeldata, 0, ptr, Image.PixelDataLength);
            bmap.UnlockBits(bmapdata);

            return bmap;
        }
    }
}
