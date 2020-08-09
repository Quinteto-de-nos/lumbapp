using LumbApp.Conectores.ConectorKinect;
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
        private List<Point> points2D;
        private List<DepthImagePoint> points3D;
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
            points2D = new List<Point>();
            points3D = new List<DepthImagePoint>();

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
            points2D.Add(pos);
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
        #endregion
        #region Metodos de Kinect y Draw
        void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            canvas.Children.Clear();

            // Color
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                    camera.Source = frame.ToBitmap();
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
            this.drawZE();

            // Calibration
            using (var frame = e.OpenDepthImageFrame())
            {
                if(frame != null)
                {
                    DepthImagePoint[] _depthPoint = new DepthImagePoint[640 * 480];
                    DepthImagePixel[] _depthPixels = new DepthImagePixel[640 * 480];
                    frame.CopyDepthImagePixelDataTo(_depthPixels);

                    conn._sensor.CoordinateMapper.MapColorFrameToDepthFrame(
                        ColorImageFormat.RgbResolution640x480Fps30,
                        DepthImageFormat.Resolution640x480Fps30,
                        _depthPixels,
                        _depthPoint
                    );

                    if(points2D.Count > 0)
                    {
                        foreach (var p in points2D)
                        {
                            Point point1 = p;
                            DepthImagePoint dpoint1 = _depthPoint[(int)point1.X * 640 + (int)point1.Y];
                            Console.WriteLine("El punto es " + dpoint1);
                            var colPoint = conn._sensor.CoordinateMapper.MapDepthPointToColorPoint(
                                DepthImageFormat.Resolution640x480Fps30,
                                dpoint1,
                                ColorImageFormat.InfraredResolution640x480Fps30);
                            draw2DPoint(colPoint, calBrush);
                        }
                    }
                    
                }
                
            }
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
