namespace SoyCrFisness.Models
{
    public class Asistencia
    {
        public string Id { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
        public string TipoClase { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}