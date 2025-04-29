using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }
        public bool Estado { get; set; }

        // Constructor
        public Cliente(int idCliente, string nombre, string telefono, string direccion, string correo, bool estado)
        {
            IdCliente = idCliente;
            Nombre = nombre;
            Telefono = telefono;
            Direccion = direccion;
            Correo = correo;
            Estado = estado;
        }

        // Constructor sin parámetros para cuando recibimos datos de la base de datos
        public Cliente() { }
    }

}
