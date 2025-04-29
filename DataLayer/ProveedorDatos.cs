using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class ProveedoresDatos
    {
        public List<Proveedor> ObtenerProveedores()
        {
            var lista = new List<Proveedor>();
            using (SqlConnection con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "SELECT IdProveedor, Nombre, Telefono, Direccion, Correo, Estado FROM Proveedores", con))
            {
                try
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Proveedor(
                                reader.GetInt32(reader.GetOrdinal("IdProveedor")),
                                reader.GetString(reader.GetOrdinal("Nombre")),
                                reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                                reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion")),
                                reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                                reader.GetBoolean(reader.GetOrdinal("Estado"))
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al obtener proveedores: {ex.Message}", ex);
                }
            }
            return lista;
        }

        public Proveedor ObtenerProveedorPorId(int idProveedor)
        {
            Proveedor proveedor = null;
            using (SqlConnection con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "SELECT IdProveedor, Nombre, Telefono, Direccion, Correo, Estado FROM Proveedores WHERE IdProveedor = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idProveedor);
                try
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            proveedor = new Proveedor(
                                reader.GetInt32(reader.GetOrdinal("IdProveedor")),
                                reader.GetString(reader.GetOrdinal("Nombre")),
                                reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                                reader.IsDBNull(reader.GetOrdinal("Direccion")) ? null : reader.GetString(reader.GetOrdinal("Direccion")),
                                reader.IsDBNull(reader.GetOrdinal("Correo")) ? null : reader.GetString(reader.GetOrdinal("Correo")),
                                reader.GetBoolean(reader.GetOrdinal("Estado"))
                            );
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al obtener proveedor por ID: {ex.Message}", ex);
                }
            }
            return proveedor;
        }

        public int AgregarProveedor(Proveedor p)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(@"
        INSERT INTO Proveedores 
            (Nombre, Telefono, Direccion, Correo, Estado)
        VALUES
            (@Nombre, @Telefono, @Direccion, @Correo, @Estado);
        SELECT CAST(SCOPE_IDENTITY() AS INT);
    ", con))
            {
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", (object)p.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)p.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)p.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", p.Estado);
                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }


        public void ActualizarProveedor(Proveedor p)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "UPDATE Proveedores SET Nombre=@Nombre, Telefono=@Telefono, Direccion=@Direccion, Correo=@Correo, Estado=@Estado " +
                "WHERE IdProveedor=@Id", con))
            {
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Telefono", (object)p.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Direccion", (object)p.Direccion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)p.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", p.Estado);
                cmd.Parameters.AddWithValue("@Id", p.IdProveedor);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al actualizar proveedor: {ex.Message}", ex);
                }
            }
        }

        public void EliminarProveedor(int idProveedor)
        {
            using (SqlConnection con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "DELETE FROM Proveedores WHERE IdProveedor=@Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idProveedor);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error al eliminar proveedor: {ex.Message}", ex);
                }
            }
        }
    }
}

