using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DataLayer
{
    public class ProductosDatos
    {
        public List<Producto> ObtenerProductos()
        {
            var lista = new List<Producto>();
            using (var con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "SELECT IdProducto, Nombre, Descripcion, PrecioCompra, PrecioVenta, Stock, Estado FROM Productos", con))
            {
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Producto(
                            reader.GetInt32(reader.GetOrdinal("IdProducto")),
                            reader.GetString(reader.GetOrdinal("Nombre")),
                            reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Descripcion")),
                            reader.GetDecimal(reader.GetOrdinal("PrecioCompra")),
                            reader.GetDecimal(reader.GetOrdinal("PrecioVenta")),
                            reader.GetInt32(reader.GetOrdinal("Stock")),
                            reader.GetBoolean(reader.GetOrdinal("Estado"))
                        ));
                    }
                }
            }
            return lista;
        }

        public Producto ObtenerProductoPorId(int idProducto)
        {
            Producto prod = null;
            using (var con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "SELECT IdProducto, Nombre, Descripcion, PrecioCompra, PrecioVenta, Stock, Estado FROM Productos WHERE IdProducto = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idProducto);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        prod = new Producto(
                            reader.GetInt32(reader.GetOrdinal("IdProducto")),
                            reader.GetString(reader.GetOrdinal("Nombre")),
                            reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("Descripcion")),
                            reader.GetDecimal(reader.GetOrdinal("PrecioCompra")),
                            reader.GetDecimal(reader.GetOrdinal("PrecioVenta")),
                            reader.GetInt32(reader.GetOrdinal("Stock")),
                            reader.GetBoolean(reader.GetOrdinal("Estado"))
                        );
                    }
                }
            }
            return prod;
        }

        public int AgregarProducto(Producto p)
        {
            using (var con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(@"
                INSERT INTO Productos
                    (Nombre, Descripcion, PrecioCompra, PrecioVenta, Stock, Estado)
                VALUES
                    (@Nombre, @Desc, @PCompra, @PVenta, @Stock, @Estado);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ", con))
            {
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Desc", (object)p.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PCompra", p.PrecioCompra);
                cmd.Parameters.AddWithValue("@PVenta", p.PrecioVenta);
                cmd.Parameters.AddWithValue("@Stock", p.Stock);
                cmd.Parameters.AddWithValue("@Estado", p.Estado);

                con.Open();
                return (int)cmd.ExecuteScalar();
            }
        }

        public void ActualizarProducto(Producto p)
        {
            using (var con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(@"
                UPDATE Productos
                   SET Nombre       = @Nombre,
                       Descripcion  = @Desc,
                       PrecioCompra = @PCompra,
                       PrecioVenta  = @PVenta,
                       Stock        = @Stock,
                       Estado       = @Estado
                 WHERE IdProducto   = @Id
            ", con))
            {
                cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
                cmd.Parameters.AddWithValue("@Desc", (object)p.Descripcion ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PCompra", p.PrecioCompra);
                cmd.Parameters.AddWithValue("@PVenta", p.PrecioVenta);
                cmd.Parameters.AddWithValue("@Stock", p.Stock);
                cmd.Parameters.AddWithValue("@Estado", p.Estado);
                cmd.Parameters.AddWithValue("@Id", p.IdProducto);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void EliminarProducto(int idProducto)
        {
            using (var con = conexion.ObtenerConexion())
            using (var cmd = new SqlCommand(
                "DELETE FROM Productos WHERE IdProducto = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", idProducto);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
