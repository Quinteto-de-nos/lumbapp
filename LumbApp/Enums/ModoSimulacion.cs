using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LumbApp.Enums
{
    public class ModoSimulacionManager : BaseEnumManager<ModoSimulacion> { }

    /// <summary>
    /// Origenes de un Turno
    /// </summary>
    public enum ModoSimulacion
    {
        [Display(Name = "Modo Guiado")]
        ModoGuiado = 0,

        [Display(Name = "Modo Evaluación")]
        ModoEvaluacion = 1,
    }

}
