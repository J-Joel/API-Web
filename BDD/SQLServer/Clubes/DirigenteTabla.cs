using System.Data;
using API_Web.Models.Clubes;
using Microsoft.Data.SqlClient;

namespace API_Web.BDD.SQLServer.Clubes
{
    public class DirigenteTabla : ClubesConexion // Hereda la conexion para esta clase
    {
        #region Metodo GET
        public List<Dirigente> ListadoDeDirigente()
        {
            List<Dirigente> list = new List<Dirigente>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM Dirigente WHERE Activo = 1";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Dirigente()
                    {
                        DirigenteId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        Rol = reader.GetString(5),
                        Dni = reader.GetInt32(6),
                        Activo = reader.GetBoolean(7)
                    });
                }
            }
            return list;
        }
        public List<Dirigente> ListadoDeDirigentePaginado(int pagina, int tamaño)
        {
            List<Dirigente> list = new List<Dirigente>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM Dirigente WHERE Activo = 1 ORDER BY DirigenteId OFFSET @Pagina ROWS FETCH NEXT @Tamaño ROWS ONLY";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Pagina", (pagina - 1) * tamaño);
                command.Parameters.AddWithValue("@Tamaño", tamaño);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Dirigente()
                    {
                        DirigenteId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        Rol = reader.GetString(5),
                        Dni = reader.GetInt32(6),
                        Activo = reader.GetBoolean(7)
                    });
                }
            }
            return list;
        }
        public Dirigente DirigentePorId(int id)
        {
            Dirigente dirigente = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM Dirigente WHERE DirigenteId = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dirigente = new Dirigente()
                    {
                        DirigenteId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        Rol = reader.GetString(5),
                        Dni = reader.GetInt32(6),
                        Activo = reader.GetBoolean(7)
                    };
                }
                else
                {
                    return null;
                }
            }
            return dirigente;
        }
        public Dirigente DirigentePorDni(int dni)
        {
            Dirigente dirigente = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM Dirigente WHERE Dni = {dni}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dirigente = new Dirigente()
                    {
                        DirigenteId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        Rol = reader.GetString(5),
                        Dni = reader.GetInt32(6),
                        Activo = reader.GetBoolean(7)
                    };
                }
                else
                {
                    return null;
                }
            }
            return dirigente;
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un unico registro
        public bool DirigentesNuevos(List<Dirigente> lista)
        {
            DataTable dt = new DataTable();
            // Se crea las columnas de la tabla temporal
            dt.Columns.Add("ClubId", typeof(Int32));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Apellido", typeof(string));
            dt.Columns.Add("FechaNacimiento", typeof(DateOnly));
            dt.Columns.Add("Rol", typeof(string));
            dt.Columns.Add("Dni", typeof(Int32));
            dt.Columns.Add("Activo", typeof(bool));

            // Se añade cada objeto a la tabla temporal
            foreach (Dirigente dirigente in lista)
                dt.Rows.Add(dirigente.ClubId, dirigente.Nombre, dirigente.Apellido, dirigente.FechaNacimiento, dirigente.Rol, dirigente.Dni, dirigente.Activo);

            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Se indica el nombre de la tabla a modificar
                        bulkCopy.DestinationTableName = "dbo.Dirigente";

                        // Se mapea las columnas de la tabla temporal creada con las columnas de la base
                        bulkCopy.ColumnMappings.Add("ClubId", "ClubId");
                        bulkCopy.ColumnMappings.Add("Nombre", "Nombre");
                        bulkCopy.ColumnMappings.Add("Apellido", "Apellido");
                        bulkCopy.ColumnMappings.Add("FechaNacimiento", "FechaNacimiento");
                        bulkCopy.ColumnMappings.Add("Rol", "Rol");
                        bulkCopy.ColumnMappings.Add("Dni", "Dni");
                        bulkCopy.ColumnMappings.Add("Activo", "Activo");

                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            catch
            {
                return false; // Problema de conexion
            }
            return true;
        }
        #endregion

        #region Metodo PUT
        public bool DirigenteModificado(Dirigente dirigente)
        {
            string query = "UPDATE Dirigente " +
                "SET ClubId = @ClubId, " +
                    "Nombre = @Nombre, " +
                    "Apellido = @Apellido, " +
                    "FechaNacimiento = @FechaNacimiento, " +
                    "Rol = @Rol, " +
                    "Dni = @Dni " +
                    "Activo = @Activo " +
                "WHERE DirigenteId = @DirigenteId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DirigenteId", dirigente.DirigenteId);
                        command.Parameters.AddWithValue("@ClubId", dirigente.ClubId);
                        command.Parameters.AddWithValue("@Nombre", dirigente.Nombre);
                        command.Parameters.AddWithValue("@Apellido", dirigente.Apellido);
                        command.Parameters.AddWithValue("@FechaNacimiento", dirigente.FechaNacimiento);
                        command.Parameters.AddWithValue("@Rol", dirigente.Rol);
                        command.Parameters.AddWithValue("@Dni", dirigente.Dni);
                        command.Parameters.AddWithValue("@Activo", dirigente.Activo);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0) // No se encontro el ID, en caso de que el registro sea borrado durante la operacion
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false; // Problema de conexion
            }
            return true;
        }
        #endregion

        #region Metodo DELETE
        public bool DirigenteEliminado(int id)
        {
            string query = "DELETE FROM Dirigente WHERE DirigenteId = @DirigenteId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DirigenteId", id);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0) // No se encontro el ID, en caso de que el registro sea borrado durante la operacion
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false; // Problema de conexion
            }
            return true;
        }
        public bool DirigenteDeshabilitado(int id)
        {
            string query = "UPDATE Dirigente " +
                "SET Activo = @Activo " +
                "WHERE DirigenteId = @DirigenteId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Activo", 0);
                        command.Parameters.AddWithValue("@DirigenteId", id);
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0) // No se encontro el ID, en caso de que el registro sea borrado durante la operacion
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false; // Problema de conexion
            }
            return true;
        }
        #endregion
    }
}