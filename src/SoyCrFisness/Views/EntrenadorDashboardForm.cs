using SoyCrFisness.Controllers;
using SoyCrFisness.Models;

namespace SoyCrFisness.Views
{
    public class EntrenadorDashboardForm : Form
    {
        private readonly Entrenador _entrenador;
        private readonly AuthController _authController;
        private readonly GimnasioController _gimnasioController;

        private readonly ComboBox _cmbSocios = new() { Left = 20, Top = 75, Width = 350, DropDownStyle = ComboBoxStyle.DropDownList };

        private readonly TextBox _txtNombreRutina = new() { Left = 20, Top = 140, Width = 220, PlaceholderText = "Ej: Fuerza Explosiva" };
        private readonly TextBox _txtEjercicios = new() { Left = 20, Top = 172, Width = 350, PlaceholderText = "Ej: Sentadillas 4x10, Prensa 4x12" };
        private readonly TextBox _txtCargaKg = new() { Left = 250, Top = 140, Width = 120, PlaceholderText = "Carga kg (40)" };
        private readonly Button _btnAsignarRutina = new() { Left = 20, Top = 205, Width = 350, Height = 30, Text = "Asignar Rutina al Socio" };

        private readonly TextBox _txtTituloInsignia = new() { Left = 400, Top = 140, Width = 210, PlaceholderText = "Título insignia" };
        private readonly CheckBox _chkVerguenza = new() { Left = 620, Top = 142, Width = 140, Text = "Es de vergüenza" };
        private readonly TextBox _txtDescInsignia = new() { Left = 400, Top = 172, Width = 360, PlaceholderText = "Motivo o descripción" };
        private readonly Button _btnOtorgarInsignia = new() { Left = 400, Top = 205, Width = 360, Height = 30, Text = "Otorgar Insignia al Socio" };

        private readonly ListBox _lstNotificaciones = new() { Left = 20, Top = 275, Width = 740, Height = 165 };

        public EntrenadorDashboardForm(
            Entrenador entrenador,
            AuthController authController,
            GimnasioController gimnasioController)
        {
            _entrenador = entrenador;
            _authController = authController;
            _gimnasioController = gimnasioController;

            ConfigurarVentana();
            CargarSociosYAlertas();
        }

        private void ConfigurarVentana()
        {
            Text = $"SoyCrFisness - Panel de Entrenador ({_entrenador.Nombre})";
            Width = 800;
            Height = 495;
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
                Text = $"Entrenador(a): {_entrenador.Nombre} ({_entrenador.Id}) | Especialidad: {_entrenador.Especialidad}"
            };

            var lblSocio = new Label { Left = 20, Top = 52, Width = 350, Text = "Seleccione un Socio de la Plantilla:" };
            var lblRutina = new Label { Left = 20, Top = 116, Width = 350, Text = "Asignar Nueva Rutina (vigencia 15 días):" };
            var lblInsignia = new Label { Left = 400, Top = 116, Width = 360, Text = "Otorgar Insignia (Logro o Vergüenza):" };
            var lblAlertas = new Label { Left = 20, Top = 252, Width = 740, Text = "Timeline de Alertas del Entrenador:" };

            _btnAsignarRutina.Click += AlHacerClicAsignarRutina;
            _btnOtorgarInsignia.Click += AlHacerClicOtorgarInsignia;

            Controls.AddRange(
            [
                lblTitulo, lblSocio, _cmbSocios,
                lblRutina, _txtNombreRutina, _txtCargaKg, _txtEjercicios, _btnAsignarRutina,
                lblInsignia, _txtTituloInsignia, _chkVerguenza, _txtDescInsignia, _btnOtorgarInsignia,
                lblAlertas, _lstNotificaciones
            ]);
        }

        private void CargarSociosYAlertas()
        {
            _cmbSocios.Items.Clear();
            foreach (var u in _authController.ObtenerTodosLosUsuarios())
            {
                _cmbSocios.Items.Add($"{u.Id} - {u.Nombre} (IMC: {u.CalcularImc():F1})");
            }

            if (_cmbSocios.Items.Count > 0)
            {
                _cmbSocios.SelectedIndex = 0;
            }

            _lstNotificaciones.Items.Clear();
            foreach (var not in _gimnasioController.ObtenerNotificaciones(_entrenador.Id))
            {
                string marca = not.EsAlertaNegativa ? "[ALERTA]" : "[INFO]";
                _lstNotificaciones.Items.Add($"{not.Fecha:dd/MM HH:mm} {marca} {not.Mensaje}");
            }

            if (_lstNotificaciones.Items.Count == 0)
            {
                _lstNotificaciones.Items.Add("Sin alertas registradas para este entrenador.");
            }
        }

        private string ObtenerUsuarioIdSeleccionado()
        {
            string texto = _cmbSocios.SelectedItem?.ToString() ?? string.Empty;
            int indiceGuion = texto.IndexOf(" - ", StringComparison.Ordinal);
            return indiceGuion > 0 ? texto[..indiceGuion] : string.Empty;
        }

        private void AlHacerClicAsignarRutina(object? sender, EventArgs e)
        {
            string usuarioId = ObtenerUsuarioIdSeleccionado();
            if (string.IsNullOrEmpty(usuarioId)
                || string.IsNullOrWhiteSpace(_txtNombreRutina.Text)
                || !double.TryParse(_txtCargaKg.Text, out double carga))
            {
                MessageBox.Show("Complete nombre de rutina y carga numérica en kg.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevaRutina = new Rutina
            {
                UsuarioId = usuarioId,
                EntrenadorId = _entrenador.Id,
                Nombre = _txtNombreRutina.Text.Trim(),
                Ejercicios = string.IsNullOrWhiteSpace(_txtEjercicios.Text) ? "Rutina general supervisada" : _txtEjercicios.Text.Trim(),
                PesoCargaKg = carga,
                FechaAsignacion = DateTime.Now,
                FechaExpiracion = DateTime.Now.AddDays(15)
            };

            _gimnasioController.AsignarRutina(nuevaRutina);
            _txtNombreRutina.Clear();
            _txtCargaKg.Clear();
            _txtEjercicios.Clear();

            MessageBox.Show($"Rutina '{nuevaRutina.Nombre}' asignada a {usuarioId}.", "Rutina Guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AlHacerClicOtorgarInsignia(object? sender, EventArgs e)
        {
            string usuarioId = ObtenerUsuarioIdSeleccionado();
            if (string.IsNullOrEmpty(usuarioId) || string.IsNullOrWhiteSpace(_txtTituloInsignia.Text))
            {
                MessageBox.Show("Seleccione un socio y escriba el título de la insignia.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string titulo = _txtTituloInsignia.Text.Trim();
            string desc = string.IsNullOrWhiteSpace(_txtDescInsignia.Text) ? "Evaluación del entrenador" : _txtDescInsignia.Text.Trim();

            _gimnasioController.OtorgarInsignia(usuarioId, titulo, desc, _chkVerguenza.Checked);
            _txtTituloInsignia.Clear();
            _txtDescInsignia.Clear();
            _chkVerguenza.Checked = false;

            MessageBox.Show($"Insignia '{titulo}' otorgada a {usuarioId}.", "Insignia Registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}