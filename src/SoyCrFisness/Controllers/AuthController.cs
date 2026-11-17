using SoyCrFisness.Models;
using SoyCrFisness.Repositories;

namespace SoyCrFisness.Controllers
{
    public class AuthController
    {
        private readonly IRepository<Usuario> _repoUsuarios;
        private readonly IRepository<Entrenador> _repoEntrenadores;

        public AuthController(IRepository<Usuario> repoUsuarios, IRepository<Entrenador> repoEntrenadores)
        {
            _repoUsuarios = repoUsuarios;
            _repoEntrenadores = repoEntrenadores;
        }

        public Usuario? IniciarSesionUsuario(string identificador)
        {
            string limpio = identificador.Trim().ToLowerInvariant();
            return _repoUsuarios.ObtenerTodos()
                .Find(u => u.NombreUsuario.Equals(limpio, StringComparison.OrdinalIgnoreCase)
                        || u.Id.Equals(limpio, StringComparison.OrdinalIgnoreCase));
        }

        public Entrenador? IniciarSesionEntrenador(string identificador)
        {
            string limpio = identificador.Trim();
            return _repoEntrenadores.ObtenerTodos()
                .Find(e => e.Id.Equals(limpio, StringComparison.OrdinalIgnoreCase)
                        || e.Correo.Equals(limpio, StringComparison.OrdinalIgnoreCase));
        }

        public bool RegistrarUsuario(Usuario nuevoUsuario)
        {
            var usuarios = _repoUsuarios.ObtenerTodos();
            bool existe = usuarios.Exists(u =>
                u.NombreUsuario.Equals(nuevoUsuario.NombreUsuario, StringComparison.OrdinalIgnoreCase));

            if (existe)
            {
                return false;
            }

            nuevoUsuario.Id = $"USR-{usuarios.Count + 1:00}";
            usuarios.Add(nuevoUsuario);
            _repoUsuarios.GuardarTodos(usuarios);
            return true;
        }

        public List<Usuario> ObtenerTodosLosUsuarios()
        {
            return _repoUsuarios.ObtenerTodos();
        }
    }
}