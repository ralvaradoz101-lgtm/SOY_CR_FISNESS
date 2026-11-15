namespace SoyCrFisness.Models
{
    public class Notificacion
    {
        public string Id { get; set; } = string.Empty;
        public string DestinatarioId { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public bool EsAlertaNegativa { get; set; }
    }
}