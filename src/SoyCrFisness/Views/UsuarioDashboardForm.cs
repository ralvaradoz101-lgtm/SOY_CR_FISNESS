using SoyCrFisness.Controllers;
using SoyCrFisness.Models;

namespace SoyCrFisness.Views
{
    public class UsuarioDashboardForm : Form
    {
        private readonly Usuario _usuario;
        private readonly GimnasioController _gimnasioController;

        private readonly ComboBox _cmbClase = new() { Left = 20, Top = 105, Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly Button _btnRegistrarAsistencia = new() { Left = 210, Top = 104, Width = 170, Height = 27, Text = "Registrar Asistencia" };

        private readonly ListBox _lstRutinas = new() { Left = 20, Top = 170, Width = 360, Height = 110 };
        private readonly ListBox _lstInsignias = new() { Left = 400, Top = 170, Width = 360, Height = 110 };
        private readonly ListBox _lstNotificaciones = new() { Left = 20, Top = 315, Width = 740, Height = 130 };

        public UsuarioDashboardForm(Usuario usuario, GimnasioController gimnasioController)
        {
            _usuario = usuario;
            _gimnasioController = gimnasioController;

            ConfigurarVentana();
            CargarDatos();
        }

        private void ConfigurarVentana()
        {
            Text = $"SoyCrFisness - Panel de Socio ({_usuario.Nombre})";
            Width = 800;
            Height = 505;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var lblTitulo = new Label
            {
                Left = 20,
                Top = 15,
                Width = 740,
                Height = 25,
                Font = new Font(Font.FontFamily, 11, FontStyle.Bold),
                Text = $"Socio: {_usuario.Nombre} ({_usuario.Id}) | Meta: {_usuario.Meta}"
            };

            var lblMetricas = new Label
            {
                Left = 20,
                Top = 45,
                Width = 740,
                Height = 22,
                Text = $"Peso: {_usuario.PesoKg:F1} kg | Altura: {_usuario.AlturaMetros:F2} m | IMC Actual: {_usuario.CalcularImc():F2}"
            };

            var lblClase = new Label { Left = 20, Top = 82, Width = 250, Text = "Registrar visita o clase grupal:" };
            _cmbClase.Items.AddRange("Hora abierta", "Yoga", "Crossfit", "Zumba", "Spinning");
            _cmbClase.SelectedIndex = 0;
            _btnRegistrarAsistencia.Click += AlHacerClicRegistrarAsistencia;

            var lblRutinas = new Label { Left = 20, Top = 148, Width = 360, Text = "Mis Rutinas Asignadas:" };
            var lblInsignias = new Label { Left = 400, Top = 148, Width = 360, Text = "Mis Insignias (Logros y Vergüenza):" };
            var lblNotificaciones = new Label { Left = 20, Top = 293, Width = 740, Text = "Timeline de Notificaciones y Alertas:" };

            Controls.AddRange(
                lblTitulo, lblMetricas, lblClase, _cmbClase, _btnRegistrarAsistencia,
                lblRutinas, _lstRutinas, lblInsignias, _lstInsignias, lblNotificaciones, _lstNotificaciones);
        }

        private void CargarDatos()
        {
            _lstRutinas.Items.Clear();
            foreach (var r in _gimnasioController.ObtenerRutinasPorUsuario(_usuario.Id))
            {
                _lstRutinas.Items.Add($"[{r.Id}] {r.Nombre} - Carga: {r.PesoCargaKg} kg (Vence: {r.FechaExpiracion:dd/MM/yyyy})");
            }

            if (_lstRutinas.Items.Count == 0)
            {
                _lstRutinas.Items.Add("Sin rutinas asignadas todavía.");
            }

            _lstInsignias.Items.Clear();
            foreach (var ins in _gimnasioController.ObtenerInsigniasPorUsuario(_usuario.Id))
            {
                string tipo = ins.EsDeVerguenza ? "[VERGÜENZA]" : "[LOGRO]";
                _lstInsignias.Items.Add($"{tipo} {ins.Titulo}: {ins.Descripcion}");
            }

            if (_lstInsignias.Items.Count == 0)
            {
                _lstInsignias.Items.Add("Aún no tienes insignias registradas.");
            }

            _lstNotificaciones.Items.Clear();
            foreach (var not in _gimnasioController.ObtenerNotificaciones(_usuario.Id))
            {
                string marca = not.EsAlertaNegativa ? "[ALERTA]" : "[INFO]";
                _lstNotificaciones.Items.Add($"{not.Fecha:dd/MM HH:mm} {marca} {not.Mensaje}");
            }

            if (_lstNotificaciones.Items.Count == 0)
            {
                _lstNotificaciones.Items.Add("Sin notificaciones recientes.");
            }
        }

        private void AlHacerClicRegistrarAsistencia(object? sender, EventArgs e)
        {
            string clase = _cmbClase.SelectedItem?.ToString() ?? "Hora abierta";
            _gimnasioController.RegistrarAsistencia(_usuario.Id, clase);
            CargarDatos();
            MessageBox.Show($"Asistencia registrada en '{clase}'.", "Asistencia Confirmada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}