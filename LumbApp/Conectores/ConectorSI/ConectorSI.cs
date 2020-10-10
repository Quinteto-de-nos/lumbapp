using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Documents;

namespace LumbApp.Conectores.ConectorSI
{
    public class ConectorSI : IConectorSI
    {
        SerialPort mySerialPort;
        private bool sensando = false;

        private string datosLeidos = null;

        public ConectorSI()
        {
            sensando = false;
            datosLeidos = null;
        }

        public bool Conectar()
        {
            try
            {
                mySerialPort = new SerialPort(DetectarArduinoPort())
                {
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    RtsEnable = true
                };

                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                mySerialPort.Open();

                mySerialPort.Open();

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        private string DetectarArduinoPort()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();

                    if (desc.Contains("Dispositivo"))
                    {
                        return deviceId;
                    }
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        public void ActivarSensado() { sensando = true; }
        public void Desconectar() { mySerialPort.Close(); }
        public void PausarSensado() { sensando = false; }

        /// <summary>
        /// Evento que le indica al experto que llegaron datos al conector
        /// </summary>
        public event EventHandler<DatosSensadosEventArgs> HayDatos;

        /// <summary>
        /// Método que levanta el evento HayDatos
        /// </summary>
        /// <param name="datosDelEvento">
        /// Recibe los datos de cada puerto a traves de  la clase DatosSensadosEventArgs.
        /// </param>
        protected virtual void SiHayDatos(DatosSensadosEventArgs datosDelEvento)
        {
            HayDatos?.Invoke(this, datosDelEvento);
        }

        public virtual void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            if (sensando)
            {
                SerialPort sp = (SerialPort)sender;

                datosLeidos = sp.ReadTo("$");

                if (datosLeidos.Length == 12)
                {
                    //Cada vez que el conector recibe datos del arduino en medio de una simulación, llama al método SiHayDatos
                    DatosSensadosEventArgs args = new DatosSensadosEventArgs(datosLeidos);
                    SiHayDatos(args);
                }
            }

        }

        public bool CheckearComunicacion()
        { //Se puede checkear que todo lo que se reciba sean 0 .... o 1 si usas eso del arduino que dijiste
            String datos;

            try
            {
                for (int i = 0; i < 50; i++)
                {

                    mySerialPort.WriteLine("p");

                    datos = mySerialPort.ReadTo("$");

                    if (datos.Contains("1") || datos.Contains("0"))
                    {
                        return true;
                    }

                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
