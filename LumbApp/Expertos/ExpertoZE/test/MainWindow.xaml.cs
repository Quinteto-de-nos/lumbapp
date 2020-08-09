using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using KinectCoordinateMapping;
using Emgu.CV.CvEnum;
using Emgu.CV.UI;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Variables de joints
        internal Skeleton[] _bodies = new Skeleton[6];

        private readonly Brush trackedJointBrush = Brushes.Blue;
        private readonly Brush inferredJointBrush = Brushes.Yellow;
        private readonly Brush notTrackedJointBrush = Brushes.Red;

        // Variables de ZE
        private readonly Brush zeBrush = Brushes.Aqua;
        private readonly Brush inZeBrush = Brushes.Orange;
        private const float zeX = 0;
        private const float zeY = 0;
        private const float zeZ = 1;
        private const float delta = 0.1f;

        // Variables generales
        private ConectorKinect conn;
        private ExpertoZE expert;

        // Variables de Aruco

        // Metodos de Window
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

        // Metodos de ZE
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

        // Metodos de Kinect y draw
        void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            // Color
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    processColor(frame);
                }
            }

            // Body
            using (var frame = e.OpenSkeletonFrame())
            {
                if (frame != null)
                {
                    canvas.Children.Clear();

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

            this.drawZE();
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
        
        // Aruco
        private void processColor(ColorImageFrame frame)
        {
            var bitmapSource = frame.ToBitmap();
            camera.Source = bitmapSource;
            
            var currentImage = ColorExtensions.GetBitmapFromBitmapSource(bitmapSource);
            var image = currentImage.ToImage<Emgu.CV.Structure.Bgr, byte>();
            
            //Emgu.CV.Aruco.Dictionary.PredefinedDictionaryName name = new Emgu.CV.Aruco.Dictionary.PredefinedDictionaryName();
            //Emgu.CV.Aruco.Dictionary dict = new Emgu.CV.Aruco.Dictionary(name);
            //Emgu.CV.Util.VectorOfVectorOfPointF corners = new Emgu.CV.Util.VectorOfVectorOfPointF();
            //Emgu.CV.Util.VectorOfInt ids = new Emgu.CV.Util.VectorOfInt();
            //Emgu.CV.Aruco.DetectorParameters parameters = Emgu.CV.Aruco.DetectorParameters.GetDefault();
            
            //Emgu.CV.Aruco.ArucoInvoke.DetectMarkers(image, dict, corners, ids, parameters);

            /*
            var borderColor = new Emgu.CV.Structure.MCvScalar(1, 1, 1);
            Emgu.CV.Aruco.ArucoInvoke.DrawDetectedMarkers(image, corners, ids, borderColor);
            camera.Source = ColorExtensions.Convert(image.ToBitmap());
            */

            ImageViewer.Show(image);

        }

        // Main
        [STAThread]
        public static void Main()
        {
            
            MainWindow window = new MainWindow();
            Application app = new Application();
            app.Run(window);
         
        }
    }

}
