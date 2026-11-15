namespace SoyCrFisness.Models
{
    public class Insignia
    {
        public string Id { get; set; } = string.Empty;
        public string UsuarioId { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool EsDeVerguenza { get; set; }
        public DateTime FechaObtencion { get; set; }
    }
}