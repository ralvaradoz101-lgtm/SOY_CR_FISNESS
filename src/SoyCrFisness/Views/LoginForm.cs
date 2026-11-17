using SoyCrFisness.Controllers;
using SoyCrFisness.Models;

namespace SoyCrFisness.Views
{
    public class LoginForm : Form
    {
        private readonly AuthController _authController;
        private readonly GimnasioController _gimnasioController;

        private readonly ComboBox _cmbRol = new() { Left = 30, Top = 45, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly TextBox _txtIdentificador = new() { Left = 30, Top = 105, Width = 280, PlaceholderText = "Ej: lda_m01, USR-01 o ENT-01" };
        private readonly Button _btnIngresar = new() { Left = 30, Top = 150, Width = 280, Height = 35, Text = "Iniciar Sesión" };
        private readonly Button _btnRegistrar = new() { Left = 30, Top = 195, Width = 280, Height = 32, Text = "Registrar Nuevo Usuario" };

        public LoginForm(AuthController authController, GimnasioController gimnasioController)
        {
            _authController = authController;
            _gimnasioController = gimnasioController;

            ConfigurarVentana();
        }

        private void ConfigurarVentana()
        {
            Text = "SoyCrFisness - Acceso al Gimnasio";
            Width = 360;
            Height = 290;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var lblRol = new Label { Left = 30, Top = 22, Width = 280, Text = "Seleccione su Rol:" };
            _cmbRol.Items.AddRange("Usuario", "Entrenador");
            _cmbRol.SelectedIndex = 0;

            var lblId = new Label { Left = 30, Top = 82, Width = 280, Text = "Usuario, ID o Correo:" };

            _btnIngresar.Click += AlHacerClicIngresar;
            _btnRegistrar.Click += AlHacerClicRegistrar;

            Controls.AddRange(lblRol, _cmbRol, lblId, _txtIdentificador, _btnIngresar, _btnRegistrar);
        }

        private void AlHacerClicIngresar(object? sender, EventArgs e)
        {
            string entrada = _txtIdentificador.Text.Trim();
            if (string.IsNullOrWhiteSpace(entrada))
            {
                MessageBox.Show("Ingrese un usuario, ID o correo válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cmbRol.SelectedItem?.ToString() == "Usuario")
            {
                Usuario? usuario = _authController.IniciarSesionUsuario(entrada);
                if (usuario == null)
                {
                    MessageBox.Show("Usuario no encontrado. Pruebe con 'lda_m01' o 'lda_f01'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Hide();
                using var panelUsuario = new UsuarioDashboardForm(usuario, _gimnasioController);
                panelUsuario.ShowDialog();
                Show();
                return;
            }

            Entrenador? entrenador = _authController.IniciarSesionEntrenador(entrada);
            if (entrenador == null)
            {
                MessageBox.Show("Entrenador no encontrado. Pruebe con 'ENT-01'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Hide();
            using var panelEntrenador = new EntrenadorDashboardForm(entrenador, _authController, _gimnasioController);
            panelEntrenador.ShowDialog();
            Show();
        }

        private void AlHacerClicRegistrar(object? sender, EventArgs e)
        {
            string nombre = Microsoft.VisualBasic.Interaction.InputBox("Nombre completo del nuevo socio:", "Registro SoyCrFisness");
            if (string.IsNullOrWhiteSpace(nombre))
            {
                return;
            }

            string user = Microsoft.VisualBasic.Interaction.InputBox("Nombre de usuario (ej: lda_nuevo):", "Registro SoyCrFisness");
            string pesoTexto = Microsoft.VisualBasic.Interaction.InputBox("Peso en kg (ej: 70):", "Registro SoyCrFisness");
            string alturaTexto = Microsoft.VisualBasic.Interaction.InputBox("Altura en metros (ej: 1.75):", "Registro SoyCrFisness");

            if (!double.TryParse(pesoTexto, out double peso) || !double.TryParse(alturaTexto, out double altura) || string.IsNullOrWhiteSpace(user))
            {
                MessageBox.Show("Datos inválidos. Verifique usuario, peso y altura.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevo = new Usuario
            {
                Nombre = nombre.Trim(),
                NombreUsuario = user.Trim(),
                Correo = $"{user.Trim()}@soycrfit.cr",
                Edad = 25,
                PesoKg = peso,
                AlturaMetros = altura,
                Meta = "Salud general",
                VisitasSemanales = 1
            };

            bool creado = _authController.RegistrarUsuario(nuevo);
            if (!creado)
            {
                MessageBox.Show("Ese nombre de usuario ya existe.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show($"Usuario registrado con ID {nuevo.Id}. Ya puede iniciar sesión.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}