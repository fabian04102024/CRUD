using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class ClientesDatos
    {
        public List<Cliente> ObtenerClientes()
        {
            List<Cliente> clientes = new List<Cliente>();
            using (SqlConnection con = conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT IdCliente, Nombre, Telefono, Direccion, Correo, Estado FROM Clientes", con))
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente(
                            reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            reader.GetString(reader.GetOrdinal("Nombre")),
                            reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                            reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion")),
                            reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                            reader.GetBoolean(reader.GetOrdinal("Estado"))
                        );
                        clientes.Add(cliente);
                    }
                }
            }
            return clientes;
        }

        public Cliente ObtenerClientesPorId(int idcliente)
        {
            Cliente cliente = null;
            using (SqlConnection con = conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(
                "SELECT IdCliente, Nombre, Telefono, Direccion, Correo, Estado FROM Clientes WHERE IdCliente = @IdCliente", con))
            {
                cmd.Parameters.AddWithValue("@IdCliente", idcliente);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cliente = new Cliente(
                            reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            reader.GetString(reader.GetOrdinal("Nombre")),
                            reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                            reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion")),
                            reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                            reader.GetBoolean(reader.GetOrdinal("Estado"))
                        );
                    }
                }
            }
            return cliente;
        }

        public int AgregarCliente(Cliente cliente)
        {
            using (var con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(@"
        INSERT INTO Clientes (Nombre, Telefono, Direccion, Correo, Estado)
        VALUES (@Nombre,@Telefono,@Direccion,@Correo,@Estado);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ", con))
            {
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", (object)cliente.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)cliente.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)cliente.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }



        public void ActualizarCliente(Cliente cliente)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE Clientes SET Nombre = @Nombre, Telefono = @Telefono, Direccion = @Direccion, Correo = @Correo, Estado = @Estado WHERE IdCliente = @IdCliente", con))
            {
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", (object)cliente.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)cliente.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)cliente.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", cliente.Estado);
                cmd.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarCliente(int idCliente)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            using (SqlCommand cmd = new SqlCommand(
                "DELETE FROM Clientes WHERE IdCliente = @IdCliente", con))
            {
                cmd.Parameters.AddWithValue("@IdCliente", idCliente);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
