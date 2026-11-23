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

        private readonly IRepository<Medicion> _repoMediciones;

        private readonly IRepository<Usuario> _repoUsuarios;



        public GimnasioController(
        IRepository<Rutina> repoRutinas,
        IRepository<Asistencia> repoAsistencias,
        IRepository<Insignia> repoInsignias,
        IRepository<Notificacion> repoNotificaciones,
        IRepository<Medicion> repoMediciones,
        IRepository<Usuario> repoUsuarios)
        {
                _repoRutinas = repoRutinas;
                _repoAsistencias = repoAsistencias;
                _repoInsignias = repoInsignias;
                _repoNotificaciones = repoNotificaciones;
                _repoMediciones = repoMediciones;
                _repoUsuarios = repoUsuarios;
        }


    public void RegistrarMedicion(Medicion nuevaMedicion)
        {
            var mediciones = _repoMediciones.ObtenerTodos();

            nuevaMedicion.Id = $"MED-{mediciones.Count + 1:000}";
            nuevaMedicion.FechaRegistro = DateTime.Now;

            mediciones.Add(nuevaMedicion);

            _repoMediciones.GuardarTodos(mediciones);
        }

     public List<Medicion> ObtenerMedicionesPorUsuario(string usuarioId)
    {
            return _repoMediciones.ObtenerTodos()
                .Where(m => m.UsuarioId.Equals(usuarioId, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => m.FechaRegistro)
                .ToList();
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


        public bool EliminarRutina(string rutinaId)
        {
            var rutinas = _repoRutinas.ObtenerTodos();

            var rutina = rutinas.FirstOrDefault(r => r.Id == rutinaId);

            if (rutina == null)
            {
                return false;
            }

            rutinas.Remove(rutina);
            _repoRutinas.GuardarTodos(rutinas);

            return true;
        }



        public bool EditarRutina(Rutina rutinaActualizada)
        {
            var rutinas = _repoRutinas.ObtenerTodos();

            var rutina = rutinas.FirstOrDefault(
                r => r.Id == rutinaActualizada.Id);

            if (rutina == null)
            {
                return false;
            }

            rutina.Nombre = rutinaActualizada.Nombre;
            rutina.Ejercicios = rutinaActualizada.Ejercicios;
            rutina.PesoCargaKg = rutinaActualizada.PesoCargaKg;
            rutina.FechaExpiracion = rutinaActualizada.FechaExpiracion;

            _repoRutinas.GuardarTodos(rutinas);

            return true;
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


        public List<Asistencia> ObtenerAsistenciasPorFechas(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return _repoAsistencias.ObtenerTodos()
                .Where(a => a.Fecha.Date >= fechaInicio.Date &&
                            a.Fecha.Date <= fechaFin.Date)
                .OrderByDescending(a => a.Fecha)
                .ToList();
        }



        public Dictionary<string, int> ObtenerRankingAsistencias(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            return ObtenerAsistenciasPorFechas(fechaInicio, fechaFin)
                .GroupBy(a => a.UsuarioId)
                .OrderByDescending(grupo => grupo.Count())
                .ToDictionary(
                    grupo => grupo.Key,
                    grupo => grupo.Count()
                );
        }



        public List<Usuario> ObtenerUsuarios()
        {
            return _repoUsuarios.ObtenerTodos();
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