using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data;
using System.Data.SqlClient;
using System.Security.Policy;

namespace BNLayer
{
    public class ClienteNegocio
    {
        private ClientesDatos clienteDatos = new ClientesDatos();

        public List<Cliente> ObtenerClientes()
        {
            return clienteDatos.ObtenerClientes();
        }
        public Cliente ObtenerClientePorId(int Idcliente)
        {
            return clienteDatos.ObtenerClientesPorId(Idcliente);
        }
        public int AgregarCliente(Cliente cliente)
        {
            int id = clienteDatos.AgregarCliente(cliente);
            cliente.IdCliente = id;
            return id;
        }



        public void ActualizarCliente(Cliente cliente)
        {
            clienteDatos.ActualizarCliente(cliente);
        }

        public void EliminarCliente(int idcliente)
        {
            clienteDatos.EliminarCliente(idcliente);
        }



    }
}


