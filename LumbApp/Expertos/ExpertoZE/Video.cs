﻿using Accord.Video.FFMPEG;
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
        VideoFileWriter writer;

        internal Video()
        {
            //frames = new List<BitmapData>();
            writer = new VideoFileWriter();
            writer.Open("D:\\Leyluchy\\Documents\\LumbApp\\test.mp4", 640, 480, 30, VideoCodec.MPEG4);
        }

        /// <summary>
        /// Agrega un nuevo frame al video.
        /// </summary>
        /// <param name="frame">Frame de la camara de color de la kinect</param>
        internal void addFrame(ColorImageFrame frame)
        {
            writer.WriteVideoFrame(ImageToBitmap(frame));
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
