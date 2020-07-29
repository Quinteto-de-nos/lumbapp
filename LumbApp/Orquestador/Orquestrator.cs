﻿using GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Orquestador
{
    public class Orquestrator: IOrquestrator
    {
        public GUIController GUI { get; set; }
        public Orquestrator() { }

        public void Start()
        {
            GUI = new GUIController(this);
        }

    }
}