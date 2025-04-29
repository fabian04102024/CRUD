using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DataLayer;
using BNLayer;

namespace CRUD
{
    public partial class frmProveedor : Form
    {
        private ProveedorNegocio proveedorNegocio;
        private BindingList<Proveedor> listaBinding;
        private BindingSource bs;
        private int? seleccionadoId;
        private bool enEdicion, esNuevo;

        public frmProveedor()
        {
            InitializeComponent();
            this.Load += frmProveedor_Load;
            dgvProveedor.SelectionChanged += dgvProveedor_SelectionChanged;
            txtTelefono.KeyPress += txtTelefono_KeyPress;
            dgvProveedor.MultiSelect = false;
            dgvProveedor.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProveedor.AllowUserToAddRows = false;
            dgvProveedor.AllowUserToDeleteRows = true;
            dgvProveedor.ReadOnly = true;
            bs = new BindingSource();
            seleccionadoId = null;
            enEdicion = esNuevo = false;
        }

        private void frmProveedor_Load(object sender, EventArgs e)
        {
            try
            {
                proveedorNegocio = new ProveedorNegocio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar ProveedorNegocio:\n{ex.Message}", "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DisableAllControls();
                return;
            }
            dgvProveedor.AutoGenerateColumns = false;
            ConfigurarColumnas();
            dgvProveedor.DataSource = bs;
            CargarProveedores();
            ConfiguracionInicio();
        }

        private void ConfigurarColumnas()
        {
            if (dgvProveedor.Columns.Count > 0) return;
            dgvProveedor.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "IdProveedor",
                Visible = true

            });
            dgvProveedor.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre"
            });
            dgvProveedor.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colTelefono",
                HeaderText = "Teléfono",
                DataPropertyName = "Telefono"
            });
            dgvProveedor.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDireccion",
                HeaderText = "Dirección",
                DataPropertyName = "Direccion"
            });
            dgvProveedor.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colCorreo",
                HeaderText = "Correo",
                DataPropertyName = "Correo"
            });
            dgvProveedor.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "colEstado",
                HeaderText = "Activo",
                DataPropertyName = "Estado"
            });
        }

        private void DisableAllControls()
        {
            foreach (Control c in this.Controls)
                c.Enabled = false;
        }

        private void CargarProveedores()
        {
            try
            {
                var list = proveedorNegocio.ObtenerProveedores();
                listaBinding = new BindingList<Proveedor>(list);
                bs.DataSource = listaBinding;
                dgvProveedor.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar proveedores:\n{ex.Message}", "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfiguracionInicio()
        {
            LimpiarFormulario();
            seleccionadoId = null;
            enEdicion = esNuevo = false;
            txtNombre.Enabled = txtTelefono.Enabled = txtDireccion.Enabled = txtCorreo.Enabled = chkEstado.Enabled = false;
            dgvProveedor.Enabled = true;
            btnNuevo.Enabled = true;
            btnAgregar.Enabled = false;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void dgvProveedor_SelectionChanged(object sender, EventArgs e)
        {
            if (enEdicion) return;
            if (dgvProveedor.SelectedRows.Count == 1)
            {
                var p = dgvProveedor.SelectedRows[0].DataBoundItem as Proveedor;
                if (p != null)
                {
                    seleccionadoId = p.IdProveedor;
                    txtNombre.Text = p.Nombre;
                    txtTelefono.Text = p.Telefono;
                    txtDireccion.Text = p.Direccion;
                    txtCorreo.Text = p.Correo;
                    chkEstado.Checked = p.Estado;
                    enEdicion = true;
                    esNuevo = false;
                    txtNombre.Enabled = txtTelefono.Enabled = txtDireccion.Enabled = txtCorreo.Enabled = chkEstado.Enabled = true;
                    dgvProveedor.Enabled = false;
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
            foreach (DataGridViewRow row in dgvProveedor.Rows)
            {
                if ((row.DataBoundItem as Proveedor)?.IdProveedor == id)
                {
                    row.Selected = true;
                    dgvProveedor.FirstDisplayedScrollingRowIndex = row.Index;
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
                MessageBox.Show("El nombre es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text) && !txtTelefono.Text.All(char.IsDigit))
            {
                MessageBox.Show("El teléfono debe ser numérico.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefono.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) || !EsCorreoValido(txtCorreo.Text))
            {
                MessageBox.Show("Correo obligatorio y válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCorreo.Focus();
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

        private void label1_Click(object sender, EventArgs e) { }

        private void lblDireccion_Click(object sender, EventArgs e) { }

        private void label1_Click_1(object sender, EventArgs e) { }

        private void frmProveedor_Load_1(object sender, EventArgs e)
        {

        }

        private void btnProducto_Click(object sender, EventArgs e)
        {
          
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            

        }

        private void lblManejoProveedor_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTelefono_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            enEdicion = true;
            esNuevo = true;
            seleccionadoId = null;
            txtNombre.Enabled = txtTelefono.Enabled = txtDireccion.Enabled = txtCorreo.Enabled = chkEstado.Enabled = true;
            dgvProveedor.Enabled = false;
            btnNuevo.Enabled = false;
            btnAgregar.Enabled = true;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = true;
            txtNombre.Focus();
        }


        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;

            var np = new Proveedor(
                0,
                txtNombre.Text,
                txtTelefono.Text,
                txtDireccion.Text,
                txtCorreo.Text,
                chkEstado.Checked
            );

            try
            {
                int idGenerado = proveedorNegocio.AgregarProveedor(np);
                MessageBox.Show(
                    $"Proveedor agregado.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                CargarProveedores();
                SeleccionarEnGrid(idGenerado);
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar proveedor: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (!enEdicion || esNuevo || !seleccionadoId.HasValue) return;
            if (!ValidarFormulario()) return;
            try
            {
                var up = new Proveedor(seleccionadoId.Value, txtNombre.Text, txtTelefono.Text, txtDireccion.Text, txtCorreo.Text, chkEstado.Checked);
                proveedorNegocio.ActualizarProveedor(up);
                MessageBox.Show("Proveedor actualizado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar proveedor:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (!seleccionadoId.HasValue) return;
            if (MessageBox.Show("¿Eliminar este proveedor?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            try
            {
                proveedorNegocio.EliminarProveedor(seleccionadoId.Value);
                MessageBox.Show("Proveedor eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarProveedores();
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar proveedor:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            CargarProveedores();
            ConfiguracionInicio();
        }

        private void btnCliente_Click_1(object sender, EventArgs e)
        {
            frmcliente frmcliente = new frmcliente();
            frmcliente.Show();
            this.Hide();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            frmProductos frmProductos = new frmProductos();
            frmProductos.Show();
            this.Hide();
        }

        private void dgvProveedor_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvProveedor_SelectionChanged(sender, EventArgs.Empty);
        }

    }
}
