﻿using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbApp.Conectores.ConectorSI {
    public class ConectorSI : IConectorSI
    {
        SerialPort mySerialPort;

        private string datosLeidos = null;

        public event EventHandler<DatosSensadosEventArgs> HayDatos;

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

        public void ActivarSensado () { }
        public void Desconectar () { }
        public void PausarSensado () { }

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

        public bool ChekearSensado () { //Se puede checkear que todo lo que se reciba sean 0 .... o 1 si usas eso del arduino que dijiste
            for (int i = 0; i < 5; i++) {
                if (datosLeidos == null || datosLeidos == "")
                    return false;
                Task.Delay(1000);
            }
            return true;
        }
    }
}
