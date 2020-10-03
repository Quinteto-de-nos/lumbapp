using Accord.Video.FFMPEG;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace LumbApp.Expertos.ExpertoZE
{
    public class Video : IVideo
    {
        //private List<BitmapData> frames;
        private const int width = 640;
        private const int height = 480;
        private VideoFileWriter writer;
        private Bitmap bitmap;

        internal Video()
        {
            bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
            writer = new VideoFileWriter();
            writer.Open("D:\\Leyluchy\\Documents\\LumbApp\\test.mp4", width, height, 30, VideoCodec.MPEG4);
        }

        /// <summary>
        /// Agrega un nuevo frame al video.
        /// </summary>
        /// <param name="frame">Frame de la camara de color de la kinect</param>
        internal void addFrame(ColorImageFrame frame)
        {
            ImageToBitmap(frame);
            writer.WriteVideoFrame(bitmap);
        }

        /// <summary>
        /// Procesa la lista de frames que tiene, encodea el video y lo guarda.
        /// </summary>
        /// <param name="path">Path completo, con nombre de archivo y extension mp4</param>
        public void Save(string path)
        {
            writer.Close();
            //Console.WriteLine("Video guardado en " + path);
        }

        private void ImageToBitmap(ColorImageFrame Image)
        {
            byte[] pixeldata = new byte[Image.PixelDataLength];
            Image.CopyPixelDataTo(pixeldata);

            BitmapData bmapdata = bitmap.LockBits(
                new Rectangle(0, 0, Image.Width, Image.Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat);
            
            IntPtr ptr = bmapdata.Scan0;

            Marshal.Copy(pixeldata, 0, ptr, Image.PixelDataLength);
            bitmap.UnlockBits(bmapdata);
        }
    }
}
