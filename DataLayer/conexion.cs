
using System.Data.SqlClient;


namespace DataLayer
{
  
    public static class conexion
    {
    
        private static string connectionString = "Server=DESKTOP-VSK4022\\SQLEXPRESS01;Database=CRUD; Trusted_Connection=True;Encrypt=False;";

        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }

    } 
} 