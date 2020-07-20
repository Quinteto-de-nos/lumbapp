using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumbApp.Expertos.ExpertoZE
{
    class ZonaEsteril
    {
        private const float zeX = 0;
        private const float zeY = 0;
        private const float zeZ = 1;
        private const float delta = 0.1f;

        public ZonaEsteril() 
        {
        }

        public bool EstaDentro(float x, float y, float z)
        {
            return x < zeX + delta && x > zeX - delta
                && y < zeY + delta && y > zeY - delta
                && z < zeZ + delta && z > zeZ - delta;
        }
    }
}
