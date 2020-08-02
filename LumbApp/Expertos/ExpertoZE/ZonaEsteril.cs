namespace LumbApp.Expertos.ExpertoZE
{
    public class ZonaEsteril
    {
        private const float zeX = 0;
        private const float zeY = 0;
        private const float zeZ = 1;
        private const float delta = 0.1f;

        /// <summary>
        /// Cantidad de veces que fue contaminada, no importa por que mano.
        /// </summary>
        public int Contaminacion { get; private set; }

        public ZonaEsteril()
        {
            Contaminacion = 0;
        }

        public bool EstaDentro(float x, float y, float z)
        {
            return x < zeX + delta && x > zeX - delta
                && y < zeY + delta && y > zeY - delta
                && z < zeZ + delta && z > zeZ - delta;
        }

        public void Contaminar()
        {
            Contaminacion++;
        }

        public void Resetear()
        {
            Contaminacion = 0;
        }
    }
}
