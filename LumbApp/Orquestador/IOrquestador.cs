using LumbApp.Enums;
using System.Threading.Tasks;

namespace LumbApp.Orquestador
{
    public interface IOrquestador
    {
        //void Start ();
        void SetDatosDeSimulacion(Models.DatosPracticante datosPracticante, ModoSimulacion modo);
        void IniciarSimulacion();
        Task Inicializar();
        Task TerminarSimulacion();
    }
}
