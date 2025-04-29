using System.Collections.Generic;
using DataLayer;

namespace BNLayer
{
    public class ProductoNegocio
    {
        private ProductosDatos productoDatos = new ProductosDatos();

        public List<Producto> ObtenerProductos()
        {
            return productoDatos.ObtenerProductos();
        }

        public Producto ObtenerProductoPorId(int idProducto)
        {
            return productoDatos.ObtenerProductoPorId(idProducto);
        }

        public int AgregarProducto(Producto producto)
        {
            int nuevoId = productoDatos.AgregarProducto(producto);
            producto.IdProducto = nuevoId;
            return nuevoId;
        }

        public void ActualizarProducto(Producto producto)
        {
            productoDatos.ActualizarProducto(producto);
        }

        public void EliminarProducto(int idProducto)
        {
            productoDatos.EliminarProducto(idProducto);
        }
    }
}
