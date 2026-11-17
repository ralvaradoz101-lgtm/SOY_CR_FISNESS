


using SoyCrFisness.Controllers;
using SoyCrFisness.Models;
using SoyCrFisness.Repositories;
using SoyCrFisness.Utils;
using SoyCrFisness.Views;

namespace SoyCrFisness
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            string carpetaDatos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            SeedDataGenerator.InicializarDatosSiNoExisten(carpetaDatos);

            var repoUsuarios = new JsonRepository<Usuario>(Path.Combine(carpetaDatos, "usuarios.json"));
            var repoEntrenadores = new JsonRepository<Entrenador>(Path.Combine(carpetaDatos, "entrenadores.json"));
            var repoRutinas = new JsonRepository<Rutina>(Path.Combine(carpetaDatos, "rutinas.json"));
            var repoAsistencias = new JsonRepository<Asistencia>(Path.Combine(carpetaDatos, "asistencias.json"));
            var repoInsignias = new JsonRepository<Insignia>(Path.Combine(carpetaDatos, "insignias.json"));
            var repoNotificaciones = new JsonRepository<Notificacion>(Path.Combine(carpetaDatos, "notificaciones.json"));

            var authController = new AuthController(repoUsuarios, repoEntrenadores);
            var gimnasioController = new GimnasioController(repoRutinas, repoAsistencias, repoInsignias, repoNotificaciones);

            Application.Run(new LoginForm(authController, gimnasioController));
        }
    }
}