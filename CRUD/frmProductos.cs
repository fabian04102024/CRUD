using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using DataLayer;
using BNLayer;

namespace CRUD
{
    public partial class frmProductos : Form
    {
        private ProductoNegocio productoNegocio;
        private BindingList<Producto> listaBinding;
        private BindingSource bs;
        private int? productoSeleccionadoId;
        private bool enEdicion;
        private bool esNuevo;

        public frmProductos()
        {
            InitializeComponent();
            this.Load += frmProductos_Load;
        }

        private void frmProductos_Load(object sender, EventArgs e)
        {
            // inicialización negocio
            try
            {
                productoNegocio = new ProductoNegocio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar ProductoNegocio:\n{ex.Message}",
                                "Error Fatal", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DisableAllControls();
                return;
            }

            dgvProductos.AutoGenerateColumns = false;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.ReadOnly = true;
            ConfigurarColumnas();

            bs = new BindingSource();
            dgvProductos.DataSource = bs;
            CargarProductos();
            ConfiguracionInicio();

            dgvProductos.SelectionChanged += dgvProductos_SelectionChanged;
            txtPrecioCompra.KeyPress += SoloDigitosYDecimal;
            txtPrecioVenta.KeyPress += SoloDigitosYDecimal;
            txtStock.KeyPress += SoloDigitos;
        }

        private void ConfigurarColumnas()
        {
            if (dgvProductos.Columns.Count > 0) return;

            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                HeaderText = "ID",
                DataPropertyName = "IdProducto",
                Visible = true
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNombre",
                HeaderText = "Nombre",
                DataPropertyName = "Nombre"
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colDescripcion",
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion"
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrecioCompra",
                HeaderText = "Precio Compra",
                DataPropertyName = "PrecioCompra"
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colPrecioVenta",
                HeaderText = "Precio Venta",
                DataPropertyName = "PrecioVenta"
            });
            dgvProductos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colStock",
                HeaderText = "Stock",
                DataPropertyName = "Stock"
            });
            dgvProductos.Columns.Add(new DataGridViewCheckBoxColumn
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

        private void CargarProductos()
        {
            try
            {
                var lista = productoNegocio.ObtenerProductos();
                listaBinding = new BindingList<Producto>(lista);
                bs.DataSource = listaBinding;
                dgvProductos.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar productos:\n{ex.Message}",
                                "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfiguracionInicio()
        {
            LimpiarFormulario();
            productoSeleccionadoId = null;
            enEdicion = esNuevo = false;

            txtNombre.Enabled =
            txtDescripcion.Enabled =
            txtPrecioCompra.Enabled =
            txtPrecioVenta.Enabled =
            txtStock.Enabled =
            chkEstado.Enabled = false;

            dgvProductos.Enabled = true;

            btnNuevo.Enabled = true;
            btnAgregar.Enabled = false;
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void dgvProductos_SelectionChanged(object sender, EventArgs e)
        {
            if (enEdicion) return;

            if (dgvProductos.SelectedRows.Count == 1)
            {
                var prod = dgvProductos.SelectedRows[0].DataBoundItem as Producto;
                if (prod != null)
                {
                    productoSeleccionadoId = prod.IdProducto;
                    txtNombre.Text = prod.Nombre;
                    txtDescripcion.Text = prod.Descripcion;
                    txtPrecioCompra.Text = prod.PrecioCompra.ToString("0.00");
                    txtPrecioVenta.Text = prod.PrecioVenta.ToString("0.00");
                    txtStock.Text = prod.Stock.ToString();
                    chkEstado.Checked = prod.Estado;

                    enEdicion = true;
                    esNuevo = false;

                    txtNombre.Enabled =
                    txtDescripcion.Enabled =
                    txtPrecioCompra.Enabled =
                    txtPrecioVenta.Enabled =
                    txtStock.Enabled =
                    chkEstado.Enabled = true;

                    dgvProductos.Enabled = false;

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
            foreach (DataGridViewRow row in dgvProductos.Rows)
            {
                if ((row.DataBoundItem as Producto)?.IdProducto == id)
                {
                    row.Selected = true;
                    dgvProductos.FirstDisplayedScrollingRowIndex = row.Index;
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
            txtDescripcion.Clear();
            txtPrecioCompra.Clear();
            txtPrecioVenta.Clear();
            txtStock.Clear();
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
            if (!decimal.TryParse(txtPrecioCompra.Text, out _) ||
                !decimal.TryParse(txtPrecioVenta.Text, out _))
            {
                MessageBox.Show("Los precios deben ser números válidos.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrecioCompra.Focus();
                return false;
            }
            if (!int.TryParse(txtStock.Text, out _))
            {
                MessageBox.Show("El stock debe ser un entero.", "Validación",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }
            return true;
        }

        private void SoloDigitosYDecimal(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) &&
                !char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.')
                e.Handled = true;
        }

        private void SoloDigitos(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void chkEstado_CheckedChanged(object sender, EventArgs e) { }

        private void btnCliente_Click(object sender, EventArgs e)
        {
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
        }

        private void btnNuevo_Click_1(object sender, EventArgs e)
        {
            LimpiarFormulario();
            productoSeleccionadoId = null;
            enEdicion = esNuevo = true;

            txtNombre.Enabled =
            txtDescripcion.Enabled =
            txtPrecioCompra.Enabled =
            txtPrecioVenta.Enabled =
            txtStock.Enabled =
            chkEstado.Enabled = true;

            dgvProductos.Enabled = false;

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

            var np = new Producto(
                0,
                txtNombre.Text,
                txtDescripcion.Text,
                decimal.Parse(txtPrecioCompra.Text),
                decimal.Parse(txtPrecioVenta.Text),
                int.Parse(txtStock.Text),
                chkEstado.Checked
            );

            try
            {
                int idGenerado = productoNegocio.AgregarProducto(np);
                MessageBox.Show("Producto agregado exitosamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarProductos();
                SeleccionarEnGrid(idGenerado);
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar producto: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (!enEdicion || esNuevo || !productoSeleccionadoId.HasValue) return;
            if (!ValidarFormulario()) return;

            var up = new Producto(
                productoSeleccionadoId.Value,
                txtNombre.Text,
                txtDescripcion.Text,
                decimal.Parse(txtPrecioCompra.Text),
                decimal.Parse(txtPrecioVenta.Text),
                int.Parse(txtStock.Text),
                chkEstado.Checked
            );

            try
            {
                productoNegocio.ActualizarProducto(up);
                MessageBox.Show("Producto actualizado exitosamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarProductos();
                SeleccionarEnGrid(up.IdProducto);
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar producto: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (!productoSeleccionadoId.HasValue) return;
            if (MessageBox.Show("¿Eliminar este producto?", "Confirmar",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                productoNegocio.EliminarProducto(productoSeleccionadoId.Value);
                MessageBox.Show("Producto eliminado exitosamente.", "Éxito",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarProductos();
                ConfiguracionInicio();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar producto: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            CargarProductos();
            ConfiguracionInicio();
        }

        private void btnCliente_Click_1(object sender, EventArgs e)
        {
            frmcliente frm = new frmcliente();
            frm.Show();
            this.Hide();
        }

        private void btnProveedor_Click_1(object sender, EventArgs e)
        {
            frmProveedor frm = new frmProveedor();
            frm.Show();
            this.Hide();
        }

        private void chkEstado_CheckedChanged_1(object sender, EventArgs e)
        {
        }
    }
}
