namespace SoyCrFisness.Models
{
    public class Rutina
    {
        public string Id { get; set; } = "";
        public string UsuarioId { get; set; } = "";
        public string EntrenadorId { get; set; } = "";
        public string Nombre { get; set; } = "";
        public string Ejercicios { get; set; } = "";
        public double PesoCargaKg { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
    }
}