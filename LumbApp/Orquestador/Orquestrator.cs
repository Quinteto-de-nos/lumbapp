using GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace LumbApp.Orquestador
{
    public class Orquestrator: IOrquestrator
    {
        public GUIController GUI { get; set; }
        public Orquestrator() { }

        public void Start()
        {
            GUI = new GUIController(this);
            GUI.Inicializar();
           
            //GUI.SolicitarDatosPracticante();
        }


    }
    
}
