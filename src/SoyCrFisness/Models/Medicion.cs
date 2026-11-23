using System;
using System.Collections.Generic;
using System.Text;


namespace SoyCrFisness.Models
{
    public class Medicion
    {
        public string Id { get; set; } = "";
        public string UsuarioId { get; set; } = "";

        public double PesoKg { get; set; }
        public double PorcentajeGrasa { get; set; }
        public double MasaMuscularKg { get; set; }

        public DateTime FechaRegistro { get; set; }
    }
}
