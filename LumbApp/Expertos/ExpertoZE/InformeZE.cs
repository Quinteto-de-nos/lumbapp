namespace LumbApp.Expertos.ExpertoZE
{
    public class InformeZE
    {
        /// <summary>
        /// Total de cantidad de veces que se contamino la zona esteril
        /// </summary>
        public int Zona { get; private set; }
        /// <summary>
        /// Cantidad de veces que la mano izquierda contamino la zona esteril
        /// </summary>
        public int ManoIzquierda { get; private set; }
        /// <summary>
        /// Cantidad de veces que la mano derecha contamino la zona estril
        /// </summary>
        public int ManoDerecha { get; private set; }

        public InformeZE(int zona, int derecha, int izquierda)
        {
            Zona = zona;
            ManoDerecha = derecha;
            ManoIzquierda = izquierda;
        }

        public InformeZE() { }
    }
}
