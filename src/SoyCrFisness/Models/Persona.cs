namespace SoyCrFisness.Models
{
    public abstract class Persona
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public int Edad { get; set; }

        public abstract string ObtenerRol();
    }
}