using SoyCrFisness.Models;
using SoyCrFisness.Repositories;

namespace SoyCrFisness.Controllers
{
    public class GimnasioController
    {
        private readonly IRepository<Rutina> _repoRutinas;
        private readonly IRepository<Asistencia> _repoAsistencias;
        private readonly IRepository<Insignia> _repoInsignias;
        private readonly IRepository<Notificacion> _repoNotificaciones;

        public GimnasioController(
            IRepository<Rutina> repoRutinas,
            IRepository<Asistencia> repoAsistencias,
            IRepository<Insignia> repoInsignias,
            IRepository<Notificacion> repoNotificaciones)
        {
            _repoRutinas = repoRutinas;
            _repoAsistencias = repoAsistencias;
            _repoInsignias = repoInsignias;
            _repoNotificaciones = repoNotificaciones;
        }

        public List<Rutina> ObtenerRutinasPorUsuario(string usuarioId)
        {
            return _repoRutinas.ObtenerTodos()
                .FindAll(r => r.UsuarioId.Equals(usuarioId, StringComparison.OrdinalIgnoreCase));
        }

        public void AsignarRutina(Rutina nuevaRutina)
        {
            var rutinas = _repoRutinas.ObtenerTodos();
            nuevaRutina.Id = $"RUT-{rutinas.Count + 1:00}";
            rutinas.Add(nuevaRutina);
            _repoRutinas.GuardarTodos(rutinas);

            AgregarNotificacion(
                nuevaRutina.UsuarioId,
                $"Se te asignó la rutina '{nuevaRutina.Nombre}' hasta el {nuevaRutina.FechaExpiracion:dd/MM/yyyy}.",
                false);
        }

        public void RegistrarAsistencia(string usuarioId, string tipoClase)
        {
            var asistencias = _repoAsistencias.ObtenerTodos();
            asistencias.Add(new Asistencia
            {
                Id = $"ASI-{asistencias.Count + 1:00}",
                UsuarioId = usuarioId,
                TipoClase = tipoClase,
                Fecha = DateTime.Now
            });
            _repoAsistencias.GuardarTodos(asistencias);

            EvaluarInsigniaPorAsistencia(usuarioId, asistencias);
        }

        public List<Asistencia> ObtenerAsistenciasPorUsuario(string usuarioId)
        {
            return _repoAsistencias.ObtenerTodos()
                .FindAll(a => a.UsuarioId.Equals(usuarioId, StringComparison.OrdinalIgnoreCase));
        }

        public List<Insignia> ObtenerInsigniasPorUsuario(string usuarioId)
        {
            return _repoInsignias.ObtenerTodos()
                .FindAll(i => i.UsuarioId.Equals(usuarioId, StringComparison.OrdinalIgnoreCase));
        }

        public void OtorgarInsignia(string usuarioId, string titulo, string descripcion, bool esDeVerguenza)
        {
            var insignias = _repoInsignias.ObtenerTodos();
            insignias.Add(new Insignia
            {
                Id = $"INS-{insignias.Count + 1:00}",
                UsuarioId = usuarioId,
                Titulo = titulo,
                Descripcion = descripcion,
                EsDeVerguenza = esDeVerguenza,
                FechaObtencion = DateTime.Now
            });
            _repoInsignias.GuardarTodos(insignias);

            string prefijo = esDeVerguenza ? "Alerta de vergüenza:" : "¡Nuevo logro!";
            AgregarNotificacion(usuarioId, $"{prefijo} {titulo} - {descripcion}", esDeVerguenza);
        }

        public List<Notificacion> ObtenerNotificaciones(string destinatarioId)
        {
            return _repoNotificaciones.ObtenerTodos()
                .FindAll(n => n.DestinatarioId.Equals(destinatarioId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(n => n.Fecha)
                .ToList();
        }

        public void AgregarNotificacion(string destinatarioId, string mensaje, bool esAlertaNegativa)
        {
            var notificaciones = _repoNotificaciones.ObtenerTodos();
            notificaciones.Add(new Notificacion
            {
                Id = $"NOT-{notificaciones.Count + 1:00}",
                DestinatarioId = destinatarioId,
                Mensaje = mensaje,
                Fecha = DateTime.Now,
                EsAlertaNegativa = esAlertaNegativa
            });
            _repoNotificaciones.GuardarTodos(notificaciones);
        }

        private void EvaluarInsigniaPorAsistencia(string usuarioId, List<Asistencia> todas)
        {
            int conteo = todas.Count(a => a.UsuarioId.Equals(usuarioId, StringComparison.OrdinalIgnoreCase));
            if (conteo == 3)
            {
                OtorgarInsignia(usuarioId, "Racha Rojinegra", "Completó 3 asistencias registradas", false);
            }
        }
    }
}