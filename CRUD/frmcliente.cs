using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DataLayer;
using BNLayer;

namespace CRUD
{
    public partial class frmcliente : Form
    {
        private ClienteNegocio clienteNegocio;
        private BindingList<Cliente> listaBinding;
        private BindingSource bs;
        private int? clienteSeleccionadoId;
        private bool enEdicion;
        private bool esNuevo;

        public frmcliente()
        {
            InitializeComponent();

            
            this.Load += frmcliente_Load;
            dgvCliente.SelectionChanged += dgvCliente_SelectionChanged;
            txtTelefono.KeyPress += txtTelefono_KeyPress;

            // Configuración del DataGridView
            dgvCliente.MultiSelect = false;
            dgvCliente.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCliente.AllowUserToAddRows = false;
            dgvCliente.AllowUserToDeleteRows = true;
            dgvCliente.ReadOnly = true;

            bs = new BindingSource();
            clienteSeleccionadoId = null;
            enEdicion = false;
            esNuevo = false;
        }

        private void frmcliente_Load(object sender, EventArgs e)
        {
            try
            {
                clienteNegocio = new ClienteNegocio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error crítico al inicializar: {ex.Message}", "Error Fatal",
                                MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DisableAllControls();
                return;
            }

            dgvCliente.AutoGenerateColumns = false;
            ConfigurarColumnas();
            dgvCliente.DataSource = bs;
            CargarClientes();
            ConfiguracionInicio();
        }

        private void ConfigurarColumnas()
        {
            if (dgvCliente.Columns.Count > 0) return;

            dgvCliente.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "IdCliente",
                Visible = true  // mostrar el ID
            });
            dgvCliente.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre"
            });
            dgvCliente.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono"
            });
            dgvCliente.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDireccion",
                HeaderText = "Dirección",
                DataPropertyName = "Direccion"
            });
            dgvCliente.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCorreo",
                HeaderText = "Correo",
                DataPropertyName = "Correo"
            });
            dgvCliente.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Activo",
                DataPropertyName = "Estado"
            });
        }

        private void DisableAllControls()
        {
            foreach (Control ctrl in this.Controls)
                if (ctrl != null)
                    ctrl.Enabled = false;
        }

        private void CargarClientes()
        {
            try
            {
                var lista = clienteNegocio.ObtenerClientes();
                listaBinding = new BindingList<Cliente>(lista);
                bs.DataSource = listaBinding;
                dgvCliente.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error de Carga",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfiguracionInicio()
        {
            LimpiarFormulario();
            enEdicion = false;
            esNuevo = false;
            clienteSeleccionadoId = null;

            txtNombre.Enabled =
            txtTelefono.Enabled =
            txtDireccion.Enabled =
            txtCorreo.Enabled =
            chkEstado.Enabled = false;

            dgvCliente.Enabled = true;

            btnNuevo.Enabled = true;
            btnAgregar.Enabled = false;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void dgvCliente_SelectionChanged(object sender, EventArgs e)
        {
            if (enEdicion) return;

            if (dgvCliente.SelectedRows.Count == 1)
            {
                var cli = dgvCliente.SelectedRows[0].DataBoundItem as Cliente;
                if (cli != null)
                {
                    clienteSeleccionadoId = cli.IdCliente;
                    txtNombre.Text = cli.Nombre;
                    txtTelefono.Text = cli.Telefono;
                    txtDireccion.Text = cli.Direccion;
                    txtCorreo.Text = cli.Correo;
                    chkEstado.Checked = cli.Estado;

                    enEdicion = true;
                    esNuevo = false;

                    txtNombre.Enabled =
                    txtTelefono.Enabled =
                    txtDireccion.Enabled =
                    txtCorreo.Enabled =
                    chkEstado.Enabled = true;

                    dgvCliente.Enabled = false;

                    btnNuevo.Enabled = false;
                    btnAgregar.Enabled = false;
                    btnActualizar.Enabled = true;
                    btnEliminar.Enabled = true;
                    btnCancelar.Enabled = true;
                }
            }
            else
            {
                ConfiguracionInicio();
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
           
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
        }

        private void SeleccionarEnGrid(int id)
        {
            foreach (DataGridViewRow row in dgvCliente.Rows)
            {
                if ((row.DataBoundItem as Cliente)?.IdCliente == id)
                {
                    row.Selected = true;
                    dgvCliente.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
          
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
           
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtCorreo.Clear();
            chkEstado.Checked = true;
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                MessageBox.Show("El correo es obligatorio.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
                return false;
            }
            if (!EsCorreoValido(txtCorreo.Text))
            {
                MessageBox.Show("El correo no tiene un formato válido.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text) && !txtTelefono.Text.All(char.IsDigit))
            {
                MessageBox.Show("El teléfono sólo debe contener números.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefono.Focus();
                return false;
            }
            return true;
        }

        private bool EsCorreoValido(string correo)
        {
            try
            {
                var m = new System.Net.Mail.MailAddress(correo);
                return m.Address == correo && correo.Split('@')[1].Contains(".");
            }
            catch
            {
                return false;
            }
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        // Eventos vacíos que el Designer referencia
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void chkEstado_CheckedChanged(object sender, EventArgs e) { }
        private void frmcliente_Load_1(object sender, EventArgs e) { }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
            
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void iconMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            LimpiarFormulario();
            enEdicion = true;
            esNuevo = true;
            clienteSeleccionadoId = null;

            txtNombre.Enabled =
            txtTelefono.Enabled =
            txtDireccion.Enabled =
            txtCorreo.Enabled =
            chkEstado.Enabled = true;

            dgvCliente.Enabled = false;

            btnNuevo.Enabled = false;
            btnAgregar.Enabled = true;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = true;

            txtNombre.Focus();
        }

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {

            if (!ValidarFormulario()) return;

            var nvo = new Cliente(
                0,
                txtNombre.Text,
                txtTelefono.Text,
                txtDireccion.Text,
                txtCorreo.Text,
                chkEstado.Checked
            );

            try
            {
                int idGenerado = clienteNegocio.AgregarCliente(nvo);
                MessageBox.Show($"Cliente agregado exitosamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                SeleccionarEnGrid(idGenerado);
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (!enEdicion || esNuevo || !clienteSeleccionadoId.HasValue) return;
            if (!ValidarFormulario()) return;

            var upd = new Cliente(
                clienteSeleccionadoId.Value,
                txtNombre.Text,
                txtTelefono.Text,
                txtDireccion.Text,
                txtCorreo.Text,
                chkEstado.Checked
            );

            try
            {
                clienteNegocio.ActualizarCliente(upd);
                MessageBox.Show("Cliente actualizado.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                SeleccionarEnGrid(upd.IdCliente);
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProveedor_Click_1(object sender, EventArgs e)
        {

            frmProveedor frmProveedor = new frmProveedor();
            frmProveedor.Show();
            this.Hide();
        }

        private void btnProducto_Click_1(object sender, EventArgs e)
        {
            frmProductos frmProducto = new frmProductos();
            frmProducto.Show();
            this.Hide();
        }

        private void lblClientes_Click(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            CargarClientes();
            ConfiguracionInicio();
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (!clienteSeleccionadoId.HasValue) return;
            if (MessageBox.Show("¿Eliminar este cliente?", "Confirmar",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                clienteNegocio.EliminarCliente(clienteSeleccionadoId.Value);
                MessageBox.Show("Cliente eliminado.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarClientes();
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
