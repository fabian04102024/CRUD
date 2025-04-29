using System.Collections.Generic;
using DataLayer;

namespace BNLayer
{
    public class ProveedorNegocio
    {
        private ProveedoresDatos proveedorDatos = new ProveedoresDatos();

        public List<Proveedor> ObtenerProveedores()
        {
            return proveedorDatos.ObtenerProveedores();
        }

        public Proveedor ObtenerProveedorPorId(int idProveedor)
        {
            return proveedorDatos.ObtenerProveedorPorId(idProveedor);
        }

        public int AgregarProveedor(Proveedor proveedor)
        {
            int nuevoId = proveedorDatos.AgregarProveedor(proveedor);
            proveedor.IdProveedor = nuevoId;
            return nuevoId;
        }


        public void ActualizarProveedor(Proveedor proveedor)
        {
            proveedorDatos.ActualizarProveedor(proveedor);
        }

        public void EliminarProveedor(int idProveedor)
        {
            proveedorDatos.EliminarProveedor(idProveedor);
        }
    }
}
