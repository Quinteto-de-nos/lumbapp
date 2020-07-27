using LumbApp.Orquestador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp
{
    class StartUp
    {

        [STAThread]
        public static void Main()
        {
            Orquestrator Orquestrador = new Orquestrator();
            Orquestrador.Start();
        }
    }
}
