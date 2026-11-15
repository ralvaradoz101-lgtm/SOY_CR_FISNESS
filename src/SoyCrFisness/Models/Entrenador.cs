namespace SoyCrFisness.Models
{
    public class Entrenador : Persona
    {
        public string Especialidad { get; set; } = string.Empty;
        public string TipoEntrenador { get; set; } = string.Empty;

        public override string ObtenerRol()
        {
            return "Entrenador";
        }
    }
}