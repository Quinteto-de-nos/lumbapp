using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoZE
{
    public class ZonaEsteril
    {
        private const float zeX = 0;
        private const float zeY = 0;
        private const float zeZ = 1;
        private const float delta = 0.1f;

        private int contaminacion;

        public int Contaminacion { get => contaminacion; }

        public ZonaEsteril() 
        {
            contaminacion = 0;
        }

        public bool EstaDentro(float x, float y, float z)
        {
            return x < zeX + delta && x > zeX - delta
                && y < zeY + delta && y > zeY - delta
                && z < zeZ + delta && z > zeZ - delta;
        }

        public void Contaminar() {
            contaminacion++;
            Console.WriteLine("MAMAA!! CONTAMINASTE TODA LA ZOOOOONE!!!\nYa te lo dije " + contaminacion + " veces.");
        }

        public void Resetear()
        {
            contaminacion = 0;
        }
    }
}
