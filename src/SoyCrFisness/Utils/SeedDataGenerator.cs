using SoyCrFisness.Models;
using SoyCrFisness.Repositories;

namespace SoyCrFisness.Utils
{
    public static class SeedDataGenerator
    {
        public static void InicializarDatosSiNoExisten(string carpetaDatos)
        {
            var repoUsuarios = new JsonRepository<Usuario>(Path.Combine(carpetaDatos, "usuarios.json"));
            if (repoUsuarios.ObtenerTodos().Count > 0)
            {
                return;
            }

            var entrenadores = CrearEntrenadores();
            var usuarios = CrearUsuarios();
            var rutinas = CrearRutinas();
            var asistencias = CrearAsistencias();
            var insignias = CrearInsignias();
            var notificaciones = CrearNotificaciones();



            new JsonRepository<Entrenador>(Path.Combine(carpetaDatos, "entrenadores.json")).GuardarTodos(entrenadores);
            repoUsuarios.GuardarTodos(usuarios);
            new JsonRepository<Rutina>(Path.Combine(carpetaDatos, "rutinas.json")).GuardarTodos(rutinas);
            new JsonRepository<Asistencia>(Path.Combine(carpetaDatos, "asistencias.json")).GuardarTodos(asistencias);
            new JsonRepository<Insignia>(Path.Combine(carpetaDatos, "insignias.json")).GuardarTodos(insignias);
            new JsonRepository<Notificacion>(Path.Combine(carpetaDatos, "notificaciones.json")).GuardarTodos(notificaciones);
        }




        private static List<Entrenador> CrearEntrenadores()
        {
            return
            [
                new() { Id = "ENT-01", Nombre = "Óscar Ramírez", Correo = "oramirez@lda.cr", Edad = 61, Especialidad = "Táctica y Fuerza", TipoEntrenador = "Piso" },
                new() { Id = "ENT-02", Nombre = "Wilmer López", Correo = "wlopez@lda.cr", Edad = 55, Especialidad = "Resistencia y Cardio", TipoEntrenador = "Grupal" },
                new() { Id = "ENT-03", Nombre = "Bryan Ruiz", Correo = "bruiz@lda.cr", Edad = 41, Especialidad = "Movilidad y Core", TipoEntrenador = "Grupal" },
                new() { Id = "ENT-04", Nombre = "Javier Delgado", Correo = "jdelgado@lda.cr", Edad = 58, Especialidad = "Hipertrofia", TipoEntrenador = "Piso" },
                new() { Id = "ENT-05", Nombre = "Shirley Cruz", Correo = "scruz@lda.cr", Edad = 41, Especialidad = "Crossfit y Funcional", TipoEntrenador = "Grupal" }
            ];
        }


        private static List<Usuario> CrearUsuarios()
        {
            string[] hombres =
            [
                "Washington Ortega", "Bayron Mora", "Santiago van der Putten", "Alexis Gamboa", "Guillermo Villalobos",
                "Daniel Chacón", "Yeison Molina", "Giancarlo González", "Ronald Matarrita", "John Paul Ruiz",
                "Fernando Piñar", "Roan Wilson", "Aarón Salazar", "Rashir Parkins", "Celso Borges",
                "Creichel Pérez", "Juanpi Añor", "Anthony Hernández", "Luis Díaz", "Ronaldo Cisneros"
            ];






            string[] mujeres =
            [
                "Noelia Bermúdez", "Dayanna Pérez", "Melissa Vargas", "Lixy Rodríguez", "Fabiola Villalobos",
                "Gabriela Guillén", "Gabriella Cuevas", "María Paula Coto", "Marilenis Oporta", "Stephannie Blanco",
                "Valery Sandoval", "Keylin Gómez", "Alexandra Pinell", "Emilie Valenciano", "Kenia Rangel",
                "Mariela Campos", "Viviana Chinchilla", "Sofía Varela", "Natalia Mills", "Ányela Mesén"
            ];





            string[] metas = ["Ganar masa muscular", "Resistencia", "Potencia física", "Salud general"];
            var lista = new List<Usuario>();

            for (int i = 0; i < hombres.Length; i++)
            {
                lista.Add(new Usuario
                {
                    Id = $"USR-{i + 1:00}",
                    Nombre = hombres[i],
                    NombreUsuario = $"lda_m{i + 1:00}",
                    Correo = $"{hombres[i].Split(' ')[0].ToLowerInvariant()}{i + 1}@lda.cr",
                    Edad = 21 + (i % 17),
                    PesoKg = 72.0 + (i % 14),
                    AlturaMetros = 1.73 + ((i % 6) * 0.03),
                    Meta = metas[i % metas.Length],
                    VisitasSemanales = (i % 5) + 1
                });
            }

            for (int i = 0; i < mujeres.Length; i++)
            {
                int numero = i + 21;
                lista.Add(new Usuario
                {
                    Id = $"USR-{numero:00}",
                    Nombre = mujeres[i],
                    NombreUsuario = $"lda_f{i + 1:00}",
                    Correo = $"{mujeres[i].Split(' ')[0].ToLowerInvariant()}{numero}@lda.cr",
                    Edad = 20 + (i % 15),
                    PesoKg = 56.0 + (i % 11),
                    AlturaMetros = 1.60 + ((i % 5) * 0.03),
                    Meta = metas[i % metas.Length],
                    VisitasSemanales = (i % 5) + 1
                });
            }

            return lista;
        }

        private static List<Rutina> CrearRutinas()
        {
            var lista = new List<Rutina>();

            string[] nombres =
            [
                "Piernas",
        "Pecho",
        "Espalda",
        "Cardio",
        "Cuerpo completo"
            ];

            string[] ejercicios =
            [
                "Sentadillas 4x12, Prensa 3x12",
        "Press banca 4x10, Flexiones 3x12",
        "Remo 4x12, Jalones 3x10",
        "Caminadora 20 minutos, Bicicleta 15 minutos",
        "Sentadillas 3x12, Flexiones 3x10, Plancha 3x30s"
            ];

            int numeroId = 1;

            for (int usuario = 1; usuario <= 40; usuario++)
            {
                for (int rutina = 0; rutina < 5; rutina++)
                {
                    lista.Add(new Rutina
                    {
                        Id = $"RUT-{numeroId:000}",
                        UsuarioId = $"USR-{usuario:00}",
                        EntrenadorId = $"ENT-{rutina + 1:00}",
                        Nombre = nombres[rutina],
                        Ejercicios = ejercicios[rutina],
                        PesoCargaKg = 20 + (rutina * 5),
                        FechaAsignacion = DateTime.Now.AddDays(-15),
                        FechaExpiracion = DateTime.Now.AddDays(15)
                    });

                    numeroId++;
                }
            }

            return lista;
        }

        private static List<Asistencia> CrearAsistencias()
        {
            var lista = new List<Asistencia>();

            string[] clases =
            [
                "Yoga",
        "Crossfit",
        "Zumba",
        "Spinning",
        "Hora abierta"
            ];

            int numeroId = 1;

            for (int usuario = 1; usuario <= 40; usuario++)
            {
                for (int visita = 0; visita < 2; visita++)
                {
                    lista.Add(new Asistencia
                    {
                        Id = $"ASI-{numeroId:000}",
                        UsuarioId = $"USR-{usuario:00}",
                        TipoClase = clases[(usuario + visita - 1) % clases.Length],
                        Fecha = DateTime.Now.AddDays(-(usuario + visita))
                    });

                    numeroId++;
                }
            }

            return lista;
        }
        private static List<Insignia> CrearInsignias()
        {
            var lista = new List<Insignia>();

            for (int usuario = 1; usuario <= 40; usuario++)
            {
                bool esDeVerguenza = usuario % 4 == 0;

                lista.Add(new Insignia
                {
                    Id = $"INS-{usuario:00}",
                    UsuarioId = $"USR-{usuario:00}",
                    Titulo = esDeVerguenza
                        ? "Modo Sofá"
                        : "Constancia de Hierro",
                    Descripcion = esDeVerguenza
                        ? "Insignia negativa de demostración"
                        : "Insignia positiva de demostración",
                    EsDeVerguenza = esDeVerguenza,
                    FechaObtencion = DateTime.Now.AddDays(-usuario)
                });
            }

            return lista;
        }
        private static List<Notificacion> CrearNotificaciones()
        {
            return
            [
                new() { Id = "NOT-01", DestinatarioId = "USR-01", Mensaje = "Tu rutina vence en 2 días. Solicita actualización.", Fecha = DateTime.Now.AddHours(-5), EsAlertaNegativa = true },
                new() { Id = "NOT-02", DestinatarioId = "USR-15", Mensaje = "¡Ganaste la insignia Capitán de Hierro!", Fecha = DateTime.Now.AddDays(-2), EsAlertaNegativa = false },
                new() { Id = "NOT-03", DestinatarioId = "ENT-01", Mensaje = "El usuario USR-01 tiene su rutina próxima a vencer.", Fecha = DateTime.Now.AddHours(-3), EsAlertaNegativa = true },
                new() { Id = "NOT-04", DestinatarioId = "USR-03", Mensaje = "Alerta: Llevas pocas visitas esta semana.", Fecha = DateTime.Now.AddDays(-1), EsAlertaNegativa = true },
                new() { Id = "NOT-05", DestinatarioId = "ENT-02", Mensaje = "Clase de resistencia grupal llena para mañana.", Fecha = DateTime.Now.AddHours(-1), EsAlertaNegativa = false }
            ];
        }
    }
}