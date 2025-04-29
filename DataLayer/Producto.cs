using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal PrecioCompra { get; set; }
        public decimal PrecioVenta { get; set; }
        public int Stock { get; set; }
        public bool Estado { get; set; }

        public Producto() { }

        public Producto(int idProducto, string nombre, string descripcion, decimal precioCompra, decimal precioVenta, int stock, bool estado)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Descripcion = descripcion;
            PrecioCompra = precioCompra;
            PrecioVenta = precioVenta;
            Stock = stock;
            Estado = estado;
        }
    }

}
