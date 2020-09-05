﻿using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables de joints
        internal Skeleton[] _bodies = new Skeleton[6];

        private readonly Brush trackedJointBrush = Brushes.Blue;
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Brush notTrackedJointBrush = Brushes.Red;
        #endregion

        #region Variables ZE
        private readonly Brush zeBrush = Brushes.Aqua;
        private readonly Brush inZeBrush = Brushes.Orange;

        private SkeletonPoint s0;
        private SkeletonPoint s1;
        private SkeletonPoint s2;
        private SkeletonPoint s3;
        private SkeletonPoint s4;
        private SkeletonPoint s5;
        private SkeletonPoint s6;
        private SkeletonPoint s7;
        #endregion

        #region Variables calibracion
        private bool calcular;
        private readonly Brush calBrush2 = Brushes.Pink;
        private Point screenPoint1;
        private Point screenPoint2;
        private Point screenPoint3;
        #endregion

        #region Variables generales
        private ConectorKinect conn;
        private ExpertoZE expert;
        #endregion

        #region Metodos de Window
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new ConectorKinect();
            expert = new ExpertoZE(conn);
            expert.CambioZE += CambioZE;
            expert.Inicializar();

            conn.SubscribeFramesReady(Sensor_AllFramesReady);
            expert.IniciarSimulacion();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            expert.Finalizar();
        }

        private void Left_Down(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(camera);
            Point empty = new Point();
            if(screenPoint1 == empty)
                screenPoint1 = new Point(pos.X, pos.Y);
            else if(screenPoint2 == empty)
                screenPoint2 = new Point(pos.X, pos.Y);
            else if (screenPoint3 == empty)
            {
                screenPoint3 = new Point(pos.X, pos.Y);
                calcular = true;
            }
           
            Console.WriteLine("Click en " + pos);
        }
        #endregion

        #region Metodos de ZE
        void CambioZE(object sender, CambioZEEventArgs e)
        {
            Console.WriteLine("Cambio en ZE:");
            Console.WriteLine("-Contaminacion: " + e.ContaminadoAhora + " " + e.VecesContaminado);
            Console.WriteLine("-Derecha: " + e.ManoDerecha.Track + " " + e.ManoDerecha.Estado + " " + e.ManoDerecha.VecesContamino);
            Console.WriteLine("-Derecha: " + e.ManoIzquierda.Track + " " + e.ManoIzquierda.Estado + " " + e.ManoIzquierda.VecesContamino);
        }

        private bool isInZE(SkeletonPoint pos)
        {
            return distToPlane(s1,s2,s5, pos) > 0;
        }

        private SkeletonPoint mas(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint();
            res.X = a.X + b.X;
            res.Y = a.Y + b.Y;
            res.Z = a.Z + b.Z;
            return res;
        }

        private SkeletonPoint menos(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint();
            res.X = a.X - b.X;
            res.Y = a.Y - b.Y;
            res.Z = a.Z - b.Z;
            return res;
        }

        private SkeletonPoint cruz(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint();
            res.X = a.Y*b.Z - a.Z*b.Y; //a2b3-a3b2
            res.Y = a.Z*b.X - a.X*b.Z; //a3b1-a1b3
            res.Z = a.X*b.Y - a.Y*b.X; //a1b2-a2b1
            return res;
        }

        private SkeletonPoint por(float a, SkeletonPoint b)
        {
            var res = new SkeletonPoint();
            res.X = a * b.X;
            res.Y = a * b.Y;
            res.Z = a * b.Z;
            return res;
        }

        private double modulo(SkeletonPoint a)
        {
            return Math.Sqrt(Math.Pow(a.X,2) + Math.Pow(a.Y, 2) + Math.Pow(a.Z, 2));
        }

        private double distToPlane(SkeletonPoint p0, SkeletonPoint p1, SkeletonPoint p2, SkeletonPoint test)
        {
            //Armo el plano
            var normal = cruz(menos(p1, p0), menos(p2, p0));
            var a = normal.X;
            var b = normal.Y;
            var c = normal.Z;
            var d = -a * p0.X - b * p0.Y - c * p0.Z;

            //Calculo la distancia
            return (a * test.X + b * test.Y + c * test.Z + d) / modulo(normal);
        }

        private SkeletonPoint toWorld(SkeletonPoint sp)
        {
            //Resto s0
            /*
            var aux = new SkeletonPoint();
            aux.X = sp.X - s0.X;
            aux.Y = sp.Y - s0.Y;
            aux.Z = sp.Z - s0.Z;
            */
            //Normal
            var sn = cruz(menos(s1, s0), menos(s3, s0));


            //Matriz de transformacion
            var aux1 = menos(s1, s0);
            var aux2 = menos(sn, s0);
            var aux3 = menos(s3, s0);

            float[,] mat = new float[,]{
                {aux1.X, aux1.Y, aux1.Z},
                {aux2.X, aux2.Y, aux2.Z},
                {aux3.X, aux3.Y, aux3.Z}
            };

            // Calcular
            var auxSP = menos(sp, s0);
            var wp = new SkeletonPoint();
            wp.X = auxSP.X * mat[0,0] + auxSP.Y * mat[1,0] + auxSP.Z * mat[2,0];
            wp.Y = auxSP.X * mat[0, 1] + auxSP.Y * mat[1, 1] + auxSP.Z * mat[2, 1];
            wp.Z = auxSP.X * mat[0, 2] + auxSP.Y * mat[1, 2] + auxSP.Z * mat[2, 2];

            Console.WriteLine("ToWorld sp: " + sp.X + " " + sp.Y + " " + sp.Z);
            Console.WriteLine("ToWorld s0: " + s0.X + " " + s0.Y + " " + s0.Z);
            Console.WriteLine("ToWorld aux: " + auxSP.X + " " + auxSP.Y + " " + auxSP.Z);
            Console.WriteLine("ToWorld wp: " + wp.X + " " + wp.Y + " " + wp.Z);
            Console.WriteLine();
            return wp;
        }

        private void calcularZE(SkeletonPoint[] skeletonPoints)
        {
            // Calcular ZE
            var empty = new SkeletonPoint();
            if (s0 != empty && s1 != empty && s3 != empty)
            {
                //Calculo 4ta esquina
                //p4 = p2 - p3 + p1
                s2 = menos(mas(s1, s3), s0);
                ColorImagePoint colorPoint = SkeletonPointToScreen(s2);
                draw2DPoint(colorPoint, zeBrush);

                //Puntos de arriba
                var sn = cruz(menos(s1, s0), menos(s3, s0));
                float altura = (float)(0.3 / modulo(sn));
                var aux = por(altura, sn);
                s4 = mas(s0, aux);
                s5 = mas(s1, aux);
                s6 = mas(s2, aux);
                s7 = mas(s3, aux);

                //Log
                logPoint(s0, "s0");
                logPoint(s1, "s1");
                logPoint(s2, "s2");
                logPoint(s3, "s3");
                logPoint(s4, "s4");
                logPoint(s5, "s5");
                logPoint(s6, "s6");
                logPoint(s7, "s7");
            }
        }

        private SkeletonPoint screenPointToSkeleton(Point p, SkeletonPoint[] skeletonPoints)
        {
            return skeletonPoints[640 * (int)p.Y + (int)p.X];
        }

        private void logPoint(SkeletonPoint p, string name)
        {
            Console.WriteLine("Kinect point" + name + " [" + p.X + "," + p.Y + "," + p.Z + "]");
        }
        #endregion

        #region Metodos de Kinect y Draw
        void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            canvas.Children.Clear();

            // Color
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    camera.Source = frame.ToBitmap();

                    if (calcular)
                    {
                        //Calibration
                        using (var frameDepth = e.OpenDepthImageFrame())
                        {
                            if (frameDepth != null)
                            {
                                //Init
                                DepthImagePixel[] depthData = new DepthImagePixel[frameDepth.PixelDataLength];
                                frameDepth.CopyDepthImagePixelDataTo(depthData);
                                SkeletonPoint[] skeletonPoints = new SkeletonPoint[frameDepth.PixelDataLength];

                                // Mapeo color a skeleton
                                conn._sensor.CoordinateMapper.MapColorFrameToSkeletonFrame(
                                    frame.Format,
                                    frameDepth.Format,
                                    depthData,
                                    skeletonPoints);

                                Console.WriteLine("FRAME");

                                // Recorrer puntos marcados
                                s3 = screenPointToSkeleton(screenPoint1, skeletonPoints);
                                s0 = screenPointToSkeleton(screenPoint2, skeletonPoints);
                                s1 = screenPointToSkeleton(screenPoint3, skeletonPoints);

                                calcularZE(skeletonPoints);
                                calcular = false;
                            }
                        }
                    }
                    
                }  
            }

            // Body
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    frame.CopySkeletonDataTo(_bodies);

                    foreach (var body in _bodies)
                    {
                        if (body.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            // Console.WriteLine(body.ClippedEdges);
                            drawJoint(body.Joints[JointType.HandRight]);
                            drawJoint(body.Joints[JointType.HandLeft]);
                        }
                    }
                }
            }

            // Calibracion
            drawPoint(calBrush2, screenPoint1);
            drawPoint(calBrush2, screenPoint2);
            drawPoint(calBrush2, screenPoint3);

            //ZE
            drawZE();
        }

        private SkeletonPoint drawMarkedPoint(Point screenPoint1, SkeletonPoint[] skeletonPoints)
        {
            if (screenPoint1 != null)
            {
                SkeletonPoint sp = skeletonPoints[640 * (int)screenPoint1.Y + (int)screenPoint1.X];
                //Console.WriteLine("Point [" + screenPoint1.X + "," + screenPoint1.Y + "] - 3D point [" + sp.X + "," + sp.Y + "," + sp.Z + "]");

                if (KinectSensor.IsKnownPoint(sp))
                {
                    ColorImagePoint colorPoint = SkeletonPointToScreen(sp);
                    draw2DPoint(colorPoint, calBrush2);
                    return sp;
                }
                return new SkeletonPoint();
            }
            return new SkeletonPoint();
        }

        private void drawJoint(Joint joint)
        {
            // 3D coordinates in meters
            SkeletonPoint skeletonPoint = joint.Position;

            Brush b;
            if (joint.TrackingState == JointTrackingState.Tracked)
            {
                if (isInZE(joint.Position))
                    b = inZeBrush;
                else b = trackedJointBrush;
            }
            else if (joint.TrackingState == JointTrackingState.Inferred)
                b = inferredJointBrush;
            else b = notTrackedJointBrush;

            ColorImagePoint colorPoint = SkeletonPointToScreen(skeletonPoint);



            // DRAWING...
            draw2DPoint(colorPoint, b);
        }

        private void draw2DPoint(ColorImagePoint colorPoint, Brush b)
        {
            // 2D coordinates in pixels
            Point point = new Point();
            point.X = colorPoint.X;
            point.Y = colorPoint.Y;

            drawPoint(b, point);
        }

        private void drawPoint(Brush b, Point point)
        {
            Ellipse ellipse = new Ellipse
            {
                Fill = b,
                Width = 20,
                Height = 20
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        private ColorImagePoint SkeletonPointToScreen(SkeletonPoint skeletonPoint)
        {
            // Skeleton-to-Color mapping
            return conn._sensor.CoordinateMapper.MapSkeletonPointToColorPoint(skeletonPoint, ColorImageFormat.RgbResolution640x480Fps30);
        }

        private void drawZE()
        {
            draw2DPoint(SkeletonPointToScreen(s0), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s1), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s2), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s3), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s4), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s5), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s6), zeBrush);
            draw2DPoint(SkeletonPointToScreen(s7), zeBrush);
        }
        #endregion

        #region Main
        [STAThread]
        public static void Main()
        {
            MainWindow window = new MainWindow();
            Application app = new Application();
            app.Run(window);
        }
        #endregion
    }

}
