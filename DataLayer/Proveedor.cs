using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public bool Estado { get; set; }

        public Proveedor() { }

        public Proveedor(int idProveedor, string nombre, string telefono, string direccion, string correo, bool estado)
        {
            IdProveedor = idProveedor;
            Nombre = nombre;
            Telefono = telefono;
            Direccion = direccion;
            Correo = correo;
            Estado = estado;
        }
    }

}
