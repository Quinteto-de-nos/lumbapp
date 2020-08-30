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

        private const float zeX = 0;
        private const float zeY = 0;
        private const float zeZ = 1;
        private const float delta = 0.1f;
        #endregion

        #region Variables calibracion
        private readonly Brush calBrush = Brushes.DeepPink;
        private readonly Brush calBrush2 = Brushes.Pink;
        private Point screenPoint1;
        private Point screenPoint2;
        private SkeletonPoint basePoint;
        private List<SkeletonPoint> worldPoints;
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
            worldPoints = new List<SkeletonPoint>();

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
            return pos.X < zeX + delta && pos.X > zeX - delta
                && pos.Y < zeY + delta && pos.Y > zeY - delta
                && pos.Z < zeZ + delta && pos.Z > zeZ - delta;
        }

        private SkeletonPoint toWorld(SkeletonPoint sp)
        {
            var wp = new SkeletonPoint();
            wp.X = sp.X - basePoint.X;
            wp.Y = sp.Y - basePoint.Y;
            wp.Z = sp.Z - basePoint.Z;
            return wp;
        }

        private SkeletonPoint toSkeleton(SkeletonPoint wp)
        {
            var sp = new SkeletonPoint();
            sp.X = wp.X + basePoint.X;
            sp.Y = wp.Y + basePoint.Y;
            sp.Z = wp.Z + basePoint.Z;
            return sp;
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
                            basePoint = drawMarkedPoint(screenPoint1, skeletonPoints);
                            var point2 = drawMarkedPoint(screenPoint2, skeletonPoints);

                            // Calcular ZE
                            var empty = new SkeletonPoint();
                            if(basePoint != empty && point2 != empty)
                            {
                                var wb = toWorld(basePoint);
                                var w2 = toWorld(point2);
                                Console.WriteLine("Kinect point [" + basePoint.X + "," + basePoint.Y + "," + basePoint.Z + "] - World point [" + wb.X + "," + wb.Y + "," + wb.Z + "]");
                                Console.WriteLine("Kinect point [" + point2.X + "," + point2.Y + "," + point2.Z + "] - World point [" + w2.X + "," + w2.Y + "," + w2.Z + "]");

                                var p = new SkeletonPoint();
                                p.X = toWorld(point2).X;
                                var p2 = toSkeleton(p);
                                ColorImagePoint colorPoint = SkeletonPointToScreen(p2);
                                draw2DPoint(colorPoint, zeBrush);
                                Console.WriteLine("Kinect point [" + p2.X + "," + p2.Y + "," + p2.Z + "] - World point [" + p.X + "," + p.Y + "," + p.Z + "]");
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

            // ZE
           // this.drawZE();
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

            SkeletonPoint center = getPoint(zeX, zeY, zeZ);

            //up-bottom (Y) left-right (X) front-back (Z)
            SkeletonPoint ulf = getPoint(zeX + delta, zeY + delta, zeZ - delta);
            SkeletonPoint urf = getPoint(zeX - delta, zeY + delta, zeZ - delta);
            SkeletonPoint ulb = getPoint(zeX + delta, zeY + delta, zeZ + delta);
            SkeletonPoint urb = getPoint(zeX - delta, zeY + delta, zeZ + delta);
            SkeletonPoint blf = getPoint(zeX + delta, zeY - delta, zeZ - delta);
            SkeletonPoint brf = getPoint(zeX - delta, zeY - delta, zeZ - delta);
            SkeletonPoint blb = getPoint(zeX + delta, zeY - delta, zeZ + delta);
            SkeletonPoint brb = getPoint(zeX - delta, zeY - delta, zeZ + delta);

            draw2DPoint(this.SkeletonPointToScreen(center), zeBrush);

            draw2DPoint(this.SkeletonPointToScreen(center), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(ulf), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(urf), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(ulb), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(urb), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(blf), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(brf), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(blb), zeBrush);
            draw2DPoint(this.SkeletonPointToScreen(brb), zeBrush);


        }
        private SkeletonPoint getPoint(float x, float y, float z)
        {
            SkeletonPoint point = new SkeletonPoint();
            point.X = x;
            point.Y = y;
            point.Z = z;
            return point;
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
