namespace SoyCrFisness.Models
{
    public class Rutina
    {
        public string Id { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
        public string EntrenadorId { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Ejercicios { get; set; } = string.Empty;
        public double PesoCargaKg { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}