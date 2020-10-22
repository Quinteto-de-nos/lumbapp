using LumbApp.Conectores.ConectorKinect;
using LumbApp.Expertos.ExpertoZE;
using Microsoft.Kinect;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.Json;
using System.IO;

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

        private Brush rightHand;
        private Brush leftHand;
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
            leftHand = notTrackedJointBrush;
            rightHand = notTrackedJointBrush;

            try
            {
                conn = new ConectorKinect();
                expert = new ExpertoZE(conn);
                expert.Inicializar();

                conn.SubscribeFramesReady(Sensor_AllFramesReady);
            }
            catch (Exception except)
            {
                Console.WriteLine("No me pude conectar a la Kinect. Error: " + except.Message);
            }

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            expert.Finalizar();
        }

        private void Left_Down(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(camera);
            Point empty = new Point();
            if (screenPoint1 == empty)
                screenPoint1 = new Point(pos.X, pos.Y);
            else if (screenPoint2 == empty)
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
            Console.WriteLine("-Izquierda: " + e.ManoIzquierda.Track + " " + e.ManoIzquierda.Estado + " " + e.ManoIzquierda.VecesContamino);

            rightHand = setBrush(e.ManoDerecha);
            leftHand = setBrush(e.ManoIzquierda);
        }

        private SkeletonPoint mas(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint
            {
                X = a.X + b.X,
                Y = a.Y + b.Y,
                Z = a.Z + b.Z
            };
            return res;
        }

        private SkeletonPoint menos(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint
            {
                X = a.X - b.X,
                Y = a.Y - b.Y,
                Z = a.Z - b.Z
            };
            return res;
        }

        private SkeletonPoint cruz(SkeletonPoint a, SkeletonPoint b)
        {
            var res = new SkeletonPoint
            {
                X = a.Y * b.Z - a.Z * b.Y, //a2b3-a3b2
                Y = a.Z * b.X - a.X * b.Z, //a3b1-a1b3
                Z = a.X * b.Y - a.Y * b.X //a1b2-a2b1
            };
            return res;
        }

        private SkeletonPoint por(float a, SkeletonPoint b)
        {
            var res = new SkeletonPoint
            {
                X = a * b.X,
                Y = a * b.Y,
                Z = a * b.Z
            };
            return res;
        }

        private double modulo(SkeletonPoint a)
        {
            return Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2) + Math.Pow(a.Z, 2));
        }

        private double distToPlane(SkeletonPoint centro, SkeletonPoint right, SkeletonPoint left, SkeletonPoint test)
        {
            //Armo el plano
            var normal = cruz(menos(right, centro), menos(left, centro));
            var a = normal.X;
            var b = normal.Y;
            var c = normal.Z;
            var d = -a * centro.X - b * centro.Y - c * centro.Z;

            //Calculo la distancia
            return (a * test.X + b * test.Y + c * test.Z + d) / modulo(normal);
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

                //JSON
                var lista = new SkeletonPoint[] { s0, s1, s2, s3, s4, s5, s6, s7 };
                Calibracion cal = new Calibracion(lista);
                string json = JsonSerializer.Serialize(cal);
                Console.WriteLine(json);

                //Guardar
                File.WriteAllText("./zonaEsteril.json", json);

                //Expert
                Console.WriteLine("Regenerando experto");
                expert = new ExpertoZE(conn, cal);
                expert.CambioZE += CambioZE;
                expert.Inicializar();

                conn.SubscribeFramesReady(Sensor_AllFramesReady);
                expert.IniciarSimulacion(new Video("./test.mp4"));
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

                                Console.WriteLine("Calibrando");

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
                            drawJoint(body.Joints[JointType.HandRight], rightHand);
                            drawJoint(body.Joints[JointType.HandLeft], leftHand);
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

        private Brush setBrush(Mano data)
        {
            if (data.Track == Mano.Tracking.Perdido)
                return inferredJointBrush;
            if (data.Estado == Mano.Estados.Fuera || data.Estado == Mano.Estados.Inicial)
                return trackedJointBrush;
            return inZeBrush;
        }

        private void drawJoint(Joint joint, Brush b)
        {
            // 3D coordinates in meters
            SkeletonPoint skeletonPoint = joint.Position;
            ColorImagePoint colorPoint = SkeletonPointToScreen(skeletonPoint);

            // DRAWING...
            draw2DPoint(colorPoint, b);
        }

        private void draw2DPoint(ColorImagePoint colorPoint, Brush b)
        {
            // 2D coordinates in pixels
            Point point = new Point
            {
                X = colorPoint.X,
                Y = colorPoint.Y
            };

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
