namespace SoyCrFisness.Models
{
    public class Usuario : Persona
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public double PesoKg { get; set; }
        public double AlturaMetros { get; set; }
        public string Meta { get; set; } = string.Empty;
        public int VisitasSemanales { get; set; }

        public double CalcularImc()
        {
            if (AlturaMetros <= 0)
            {
                return 0;
            }

            return Math.Round(PesoKg / (AlturaMetros * AlturaMetros), 2);
        }

        public override string ObtenerRol()
        {
            return "Usuario";
        }
    }
}
