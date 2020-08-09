using LumbApp.Conectores.ConectorKinect;
using Microsoft.Kinect;
using System;
using System.Windows;
using Emgu.CV.Aruco;
using Emgu.CV.Util;
using Emgu.CV.UI;
using Emgu.CV;
using KinectCoordinateMapping;

namespace KinectCoordinateMapping
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ArucoWindow : Window
    {
        // Variables generales
        private ConectorKinect conn;

        // Variables de Aruco
        public static Dictionary.PredefinedDictionaryName DictName = Dictionary.PredefinedDictionaryName.Dict6X6_250;
        private Dictionary dict;
        private VectorOfVectorOfPointF corners;
        private VectorOfInt ids;
        private DetectorParameters parameters;
        private Emgu.CV.Structure.MCvScalar borderColor;

        // Metodos de Window
        public ArucoWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new ConectorKinect();
            conn.Conectar();
            conn.SubscribeFramesReady(Sensor_AllFramesReady);

            dict = new Dictionary(DictName);
            corners = new VectorOfVectorOfPointF();
            ids = new VectorOfInt();

            //parameters = DetectorParameters.GetDefault();

            parameters = new DetectorParameters();
            parameters.AdaptiveThreshWinSizeMin = 3;
            parameters.AdaptiveThreshWinSizeMax = 23;
            parameters.AdaptiveThreshWinSizeStep = 10;
            parameters.AdaptiveThreshConstant = 7;

            parameters.MinMarkerPerimeterRate = 0.03;
            parameters.MaxMarkerPerimeterRate = 4;

            parameters.PolygonalApproxAccuracyRate = 0.03; //Cuando agregue esta linea dejo de tirar excepciones raras

            parameters.MinCornerDistanceRate = 0.05;
            parameters.MinDistanceToBorder = 3;
            parameters.MinMarkerDistanceRate = 0.05;

            parameters.CornerRefinementMethod = DetectorParameters.RefinementMethod.None;
            parameters.CornerRefinementWinSize = 5;
            parameters.CornerRefinementMaxIterations = 30;
            parameters.CornerRefinementMinAccuracy = 0.1;

            parameters.MarkerBorderBits = 1;

            parameters.PerspectiveRemovePixelPerCell = 4;
            parameters.PerspectiveRemoveIgnoredMarginPerCell = 0.13;

            parameters.MaxErroneousBitsInBorderRate = 0.35;

            parameters.MinOtsuStdDev = 5;
            parameters.ErrorCorrectionRate = 0.6;

            parameters.AprilTagMinClusterPixels = 5;
            parameters.AprilTagMaxNmaxima = 10;
            parameters.AprilTagCriticalRad = (float)(10 * Math.PI / 180);
            parameters.AprilTagMaxLineFitMse = 10;
            parameters.AprilTagMinWhiteBlackDiff = 5;
            parameters.AprilTagDeglitch = 0;
            parameters.AprilTagQuadDecimate = 0;
            parameters.AprilTagQuadSigma = 0;

            borderColor = new Emgu.CV.Structure.MCvScalar(255, 0, 0);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            conn.Desconectar();
        }

        // Metodos de Kinect y draw
        void Sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            // Color
            using (var frame = e.OpenColorImageFrame())
            {
                if (frame != null)
                {
                    var bitmapSource = frame.ToBitmap();
                    //camera.Source = bitmapSource;

                    var currentImage = ColorExtensions.GetBitmapFromBitmapSource(bitmapSource);
                    var image = currentImage.ToImage<Emgu.CV.Structure.Bgr, byte>();

                    ArucoInvoke.DetectMarkers(image, dict, corners, ids, parameters);

                    ArucoInvoke.DrawDetectedMarkers(image, corners, ids, borderColor);
                    Console.WriteLine(ids.Size);
                    
                    camera.Source = ColorExtensions.Convert(image.ToBitmap());
                    

                    //ImageViewer.Show(image);
                }
            }
        }
        
        // Main
        [STAThread]
        public static void Main()
        {
            /*
            var img = new Emgu.CV.Mat();
            var dict = new Emgu.CV.Aruco.Dictionary(ArucoWindow.DictName);
            Emgu.CV.Aruco.ArucoInvoke.DrawMarker(dict, 1, 500, img);
            Emgu.CV.CvInvoke.Imwrite("marker1.png", img);
            */

            ArucoWindow window = new ArucoWindow();
            Application app = new Application();
            app.Run(window);
         
        }
    }

}
