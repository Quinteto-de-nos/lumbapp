using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Conectores.ConectorSI {
    class ConectorSI : IConectorSI
    {
        SerialPort mySerialPort;

        public ConectorSI () { }

        public void Conectar () {
            mySerialPort = new SerialPort("COM5");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
        }

        private static void DataReceivedHandler (
                        object sender,
                        SerialDataReceivedEventArgs e) {

            SerialPort sp = (SerialPort)sender;
            //string indata = sp.ReadExisting();
            //string indata = sp.ReadLine();
            // int data = sp.ReadByte();
            //Console.Write(indata);
            string datos = sp.ReadTo("$");

            //Console.WriteLine(datos);
            Console.WriteLine(datos[6]);
            //registroEstado = new RegistroEstado();

        }
    }
}
