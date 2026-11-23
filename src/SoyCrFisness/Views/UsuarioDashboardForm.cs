
using System.Globalization;
using SoyCrFisness.Controllers;
using SoyCrFisness.Models;

namespace SoyCrFisness.Views
{
    public class UsuarioDashboardForm : Form
    {
        private readonly Usuario _usuario;
        private readonly GimnasioController _gimnasioController;

        private readonly ComboBox _cmbClase = new()
        {
            Left = 20,
            Top = 105,
            Width = 180,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        private readonly Button _btnRegistrarAsistencia = new()
        {
            Left = 210,
            Top = 104,
            Width = 170,
            Height = 27,
            Text = "Registrar Asistencia"
        };

        private readonly ListBox _lstRutinas = new()
        {
            Left = 20,
            Top = 170,
            Width = 360,
            Height = 110
        };

        private readonly ListBox _lstInsignias = new()
        {
            Left = 400,
            Top = 170,
            Width = 360,
            Height = 110
        };

        private readonly ListBox _lstNotificaciones = new()
        {
            Left = 20,
            Top = 315,
            Width = 740,
            Height = 130
        };

        private readonly ListBox _lstHistorialMediciones = new()
        {
            Left = 20,
            Top = 565,
            Width = 740,
            Height = 100
        };


        private readonly Label _lblEvolucion = new()
        {
            Left = 20,
            Top = 675,
            Width = 740,
            Height = 30,
            Text = "Evolución física: pendiente de calcular."
        };




        private readonly ListBox _lstRanking = new()
        {
            Left = 20,
            Top = 745,
            Width = 740,
            Height = 120
        };

        private readonly DateTimePicker _dtpFechaInicio = new()
        {
            Left = 20,
            Top = 715,
            Width = 150,
            Format = DateTimePickerFormat.Short,
            Value = DateTime.Today.AddDays(-30)
        };

        private readonly DateTimePicker _dtpFechaFin = new()
        {
            Left = 190,
            Top = 715,
            Width = 150,
            Format = DateTimePickerFormat.Short,
            Value = DateTime.Today
        };



        private readonly TextBox _numPeso = new()
        {
            Left = 20,
            Top = 495,
            Width = 150,
            TextAlign = HorizontalAlignment.Right
        };

        private readonly TextBox _numGrasa = new()
        {
            Left = 190,
            Top = 495,
            Width = 150,
            TextAlign = HorizontalAlignment.Right
        };

        private readonly TextBox _numMasaMuscular = new()
        {
            Left = 360,
            Top = 495,
            Width = 150,
            TextAlign = HorizontalAlignment.Right
        };

        private readonly Button _btnRegistrarMedicion = new()
        {
            Left = 540,
            Top = 494,
            Width = 170,
            Height = 27,
            Text = "Registrar Medición"
        };

        public UsuarioDashboardForm(
            Usuario usuario,
            GimnasioController gimnasioController)
        {
            _usuario = usuario;
            _gimnasioController = gimnasioController;

            _numPeso.Text = _usuario.PesoKg.ToString("F1");

            ConfigurarVentana();
            CargarDatos();
        }

        private void ConfigurarVentana()
        {
            Text = $"SoyCrFisness - Panel de Socio ({_usuario.Nombre})";
            Width = 800;
            Height = 950;
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

            var lblClase = new Label
            {
                Left = 20,
                Top = 82,
                Width = 250,
                Text = "Registrar visita o clase grupal:"
            };

            _cmbClase.Items.AddRange(
                "Hora abierta",
                "Yoga",
                "Crossfit",
                "Zumba",
                "Spinning"
            );

            _cmbClase.SelectedIndex = 0;

            _btnRegistrarAsistencia.Click += AlHacerClicRegistrarAsistencia;
            _btnRegistrarMedicion.Click += AlHacerClicRegistrarMedicion;

            var lblRutinas = new Label
            {
                Left = 20,
                Top = 148,
                Width = 360,
                Text = "Mis Rutinas Asignadas:"
            };

            var lblInsignias = new Label
            {
                Left = 400,
                Top = 148,
                Width = 360,
                Text = "Mis Insignias (Logros y Vergüenza):"
            };

            var lblNotificaciones = new Label
            {
                Left = 20,
                Top = 293,
                Width = 740,
                Text = "Timeline de Notificaciones y Alertas:"
            };

            var lblMediciones = new Label
            {
                Left = 20,
                Top = 460,
                Width = 740,
                Text = "Registrar nueva medición física:"
            };

            var lblPeso = new Label
            {
                Left = 20,
                Top = 478,
                Width = 150,
                Text = "Peso (kg)"
            };

            var lblGrasa = new Label
            {
                Left = 190,
                Top = 478,
                Width = 150,
                Text = "Grasa (%)"
            };

            var lblMasaMuscular = new Label
            {
                Left = 360,
                Top = 478,
                Width = 150,
                Text = "Músculo (kg)"
            };


            var lblHistorial = new Label
            {
                Left = 20,
                Top = 545,
                Width = 740,
                Text = "Historial de mediciones físicas:"
            };

            var lblRanking = new Label
            {
                Left = 20,
                Top = 690,
                Width = 740,
                Text = "Ranking de asistencias por período:"
            };


            _dtpFechaInicio.ValueChanged += (sender, e) => CargarDatos();
            _dtpFechaFin.ValueChanged += (sender, e) => CargarDatos();






            Controls.AddRange(
                lblTitulo,
                lblMetricas,

                lblClase,
                    _cmbClase,
                    _btnRegistrarAsistencia,

                lblRutinas,
                    _lstRutinas,

                lblInsignias,
                    _lstInsignias,

                lblNotificaciones,
                    _lstNotificaciones,

                lblMediciones,
                lblPeso,
                lblGrasa,

                lblMasaMuscular,
                _numPeso,
                _numGrasa,
                _numMasaMuscular,
                _btnRegistrarMedicion,

                lblHistorial,
                _lstHistorialMediciones,
                _lblEvolucion,

                 _lblEvolucion,
                 lblRanking,
                    _dtpFechaInicio,
                    _dtpFechaFin,
                    _lstRanking

            );
        }

        private void CargarDatos()
        {
            _lstRutinas.Items.Clear();

            foreach (var rutina in _gimnasioController
                .ObtenerRutinasPorUsuario(_usuario.Id)
                .OrderByDescending(r => r.FechaAsignacion)
                .Take(5))
            {
                _lstRutinas.Items.Add(
                    $"[{rutina.Id}] {rutina.Nombre} - " +
                    $"Carga: {rutina.PesoCargaKg} kg " +
                    $"(Vence: {rutina.FechaExpiracion:dd/MM/yyyy})"
                );
            }

            if (_lstRutinas.Items.Count == 0)
            {
                _lstRutinas.Items.Add("Sin rutinas asignadas todavía.");
            }

            _lstInsignias.Items.Clear();

            foreach (var insignia in _gimnasioController
                .ObtenerInsigniasPorUsuario(_usuario.Id))
            {
                string tipo = insignia.EsDeVerguenza
                    ? "[VERGÜENZA]"
                    : "[LOGRO]";

                _lstInsignias.Items.Add(
                    $"{tipo} {insignia.Titulo}: {insignia.Descripcion}"
                );
            }

            if (_lstInsignias.Items.Count == 0)
            {
                _lstInsignias.Items.Add(
                    "Aún no tienes insignias registradas."
                );
            }

            _lstNotificaciones.Items.Clear();

            foreach (var notificacion in _gimnasioController
                .ObtenerNotificaciones(_usuario.Id))
            {
                string marca = notificacion.EsAlertaNegativa
                    ? "[ALERTA]"
                    : "[INFO]";

                _lstNotificaciones.Items.Add(
                    $"{notificacion.Fecha:dd/MM HH:mm} " +
                    $"{marca} {notificacion.Mensaje}"
                );
            }

            if (_lstNotificaciones.Items.Count == 0)
            {
                _lstNotificaciones.Items.Add(
                    "Sin notificaciones recientes."
                );
         
            
            
            }


            _lstHistorialMediciones.Items.Clear();

            foreach (var medicion in _gimnasioController.ObtenerMedicionesPorUsuario(_usuario.Id))
            {
                _lstHistorialMediciones.Items.Add(
                    $"{medicion.FechaRegistro:dd/MM/yyyy HH:mm} | " +
                    $"Peso: {medicion.PesoKg:F1} kg | " +
                    $"Grasa: {medicion.PorcentajeGrasa:F1}% | " +
                    $"Músculo: {medicion.MasaMuscularKg:F1} kg"
                );
            }

            if (_lstHistorialMediciones.Items.Count == 0)
            {
                _lstHistorialMediciones.Items.Add("Todavía no tienes mediciones registradas.");

            }  



                var mediciones = _gimnasioController
                    .ObtenerMedicionesPorUsuario(_usuario.Id)
                    .OrderBy(m => m.FechaRegistro)
                    .ToList();

                if (mediciones.Count >= 2)
                {
                    var primera = mediciones.First();
                    var ultima = mediciones.Last();

                    double cambioPeso = ultima.PesoKg - primera.PesoKg;
                    double cambioGrasa = ultima.PorcentajeGrasa - primera.PorcentajeGrasa;
                    double cambioMusculo = ultima.MasaMuscularKg - primera.MasaMuscularKg;

                    _lblEvolucion.Text =
                        $"Evolución | Peso: {cambioPeso:+0.0;-0.0;0.0} kg | " +
                        $"Grasa: {cambioGrasa:+0.0;-0.0;0.0} puntos | " +
                        $"Músculo: {cambioMusculo:+0.0;-0.0;0.0} kg";
                }
                else
                {
                    _lblEvolucion.Text = "Evolución: se necesitan al menos 2 mediciones.";
                }


            _lstRanking.Items.Clear();

            DateTime fechaInicio = _dtpFechaInicio.Value.Date;
            DateTime fechaFin = _dtpFechaFin.Value.Date;

            if (fechaInicio > fechaFin)
            {
                _lstRanking.Items.Add("La fecha inicial no puede ser mayor que la final.");
            }
            else
            {
                var ranking = _gimnasioController
                    .ObtenerRankingAsistencias(fechaInicio, fechaFin)
                    .OrderByDescending(r => r.Value)
                    .ToList();
                var usuarios = _gimnasioController.ObtenerUsuarios();
                int posicion = 1;
                foreach (var socio in ranking)
                {

                    var usuarioRanking = usuarios.FirstOrDefault(u => u.Id == socio.Key);

                string nombre = usuarioRanking != null
                    ? usuarioRanking.Nombre
                    : socio.Key;

                _lstRanking.Items.Add(
                    $"{posicion}. {nombre} ({socio.Key}) | {socio.Value} asistencias"
                );

                posicion++;
                }

                if (ranking.Count == 0)
                {
                    _lstRanking.Items.Add("No hay asistencias en este período.");
                }
            }


        }


        private void AlHacerClicRegistrarAsistencia(
            object? sender,
            EventArgs e)
        {
            string clase = _cmbClase.SelectedItem?.ToString()
                ?? "Hora abierta";

            _gimnasioController.RegistrarAsistencia(
                _usuario.Id,
                clase
            );

            CargarDatos();

            MessageBox.Show(
                $"Asistencia registrada en '{clase}'.",
                "Asistencia Confirmada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );


        }

        private void AlHacerClicRegistrarMedicion(
            object? sender,
            EventArgs e)
        {
            if (!IntentarLeerNumero(_numPeso.Text, out double peso) ||
                !IntentarLeerNumero(_numGrasa.Text, out double grasa) ||
                !IntentarLeerNumero(_numMasaMuscular.Text, out double musculo))
            {
                MessageBox.Show(
                    "Introduce números válidos en peso, grasa y músculo.",
                    "Datos incorrectos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            if (peso <= 0 ||
                grasa < 0 || grasa > 100 ||
                musculo < 0 || musculo > peso)
            {
                MessageBox.Show(
                    "Verifica los valores:\n" +
                    "- El peso debe ser mayor que cero.\n" +
                    "- La grasa debe estar entre 0 y 100 %.\n" +
                    "- La masa muscular no puede ser negativa ni superar el peso.",
                    "Valores fuera de rango",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            var nuevaMedicion = new Medicion
            {
                UsuarioId = _usuario.Id,
                PesoKg = peso,
                PorcentajeGrasa = grasa,
                MasaMuscularKg = musculo
            };

            _gimnasioController.RegistrarMedicion(nuevaMedicion);
            CargarDatos();

            MessageBox.Show(
                "Medición física registrada correctamente.",
                "Registro exitoso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private static bool IntentarLeerNumero(
            string texto,
            out double resultado)
        {
            const NumberStyles estilo = NumberStyles.Float;

            return double.TryParse(
                texto,
                estilo,
                CultureInfo.CurrentCulture,
                out resultado
            ) || double.TryParse(
                texto,
                estilo,
                CultureInfo.InvariantCulture,
                out resultado
            );
        }
    }
}
