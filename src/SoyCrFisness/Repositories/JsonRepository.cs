using System.Text.Json;

namespace SoyCrFisness.Repositories
{
    public class JsonRepository<T> : IRepository<T> where T : class
    {
        private readonly string _rutaArchivo;
        private static readonly JsonSerializerOptions _opcionesJson = new()
        {
            WriteIndented = true
        };

        public JsonRepository(string rutaArchivo)
        {
            _rutaArchivo = rutaArchivo;
        }

        public List<T> ObtenerTodos()
        {
            if (!File.Exists(_rutaArchivo))
            {
                return [];
            }

            string contenido = File.ReadAllText(_rutaArchivo);
            if (string.IsNullOrWhiteSpace(contenido))
            {
                return [];
            }

            return JsonSerializer.Deserialize<List<T>>(contenido, _opcionesJson) ?? [];
        }

        public void GuardarTodos(List<T> elementos)
        {
            string? carpeta = Path.GetDirectoryName(_rutaArchivo);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            string contenido = JsonSerializer.Serialize(elementos, _opcionesJson);
            File.WriteAllText(_rutaArchivo, contenido);
        }
    }
}