using API_Web.Models.Clubes;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_Web.BDD.SQLServer.Clubes
{
    public class SocioTabla : ClubesConexion // Hereda la conexion para esta clase
    {

        #region Metodo GET
        public List<Socio>ListadoDeSocio()
        {
            List<Socio> list = new List<Socio>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM Socio WHERE Activo = 1";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Socio()
                    {
                        SocioId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        FechaAsociado = DateOnly.FromDateTime(reader.GetDateTime(5)),
                        Dni = reader.GetInt32(6),
                        CantidadAsistencias = reader.GetInt32(7),
                        Activo = reader.GetBoolean(8)
                    });
                }
            }
            return list;
        }
        public List<Socio> ListadoDeSocioPaginado(int pagina, int tamaño)
        {
            List<Socio> list = new List<Socio>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM Socio WHERE Activo = 1 ORDER BY SocioId OFFSET @Pagina ROWS FETCH NEXT @Tamaño ROWS ONLY";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Pagina", (pagina - 1) * tamaño);
                command.Parameters.AddWithValue("@Tamaño", tamaño);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Socio()
                    {
                        SocioId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        FechaAsociado = DateOnly.FromDateTime(reader.GetDateTime(5)),
                        Dni = reader.GetInt32(6),
                        CantidadAsistencias = reader.GetInt32(7),
                        Activo = reader.GetBoolean(8)
                    });
                }
            }
            return list;
        }
        #endregion

        #region Metodo GET(id)
        public Socio SocioPorId(int id)
        {
            Socio socio = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM Socio WHERE SocioId = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    socio = new Socio()
                    {
                        SocioId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        FechaAsociado = DateOnly.FromDateTime(reader.GetDateTime(5)),
                        Dni = reader.GetInt32(6),
                        CantidadAsistencias = reader.GetInt32(7),
                        Activo = reader.GetBoolean(8)
                    };
                }
                else
                {
                    return null;
                }
            }
            return socio;
        }
        #endregion

        #region Metodo GET(Dni)
        public Socio SocioPorDni(int dni)
        {
            Socio socio = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM Socio WHERE Dni = {dni}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    socio = new Socio()
                    {
                        SocioId = reader.GetInt32(0),
                        ClubId = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        Apellido = reader.GetString(3),
                        FechaNacimiento = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        FechaAsociado = DateOnly.FromDateTime(reader.GetDateTime(5)),
                        Dni = reader.GetInt32(6),
                        CantidadAsistencias = reader.GetInt32(7),
                        Activo = reader.GetBoolean(8)
                    };
                }
                else
                {
                    return null;
                }
            }
            return socio;
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un unico registro
        public bool SociosNuevos(List<Socio> lista)
        {
            DataTable dt = new DataTable();
            // Se crea las columnas de la tabla temporal
            dt.Columns.Add("ClubId", typeof(Int32));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Apellido", typeof(string));
            dt.Columns.Add("FechaNacimiento", typeof(DateOnly));
            dt.Columns.Add("FechaAsociado", typeof(DateOnly));
            dt.Columns.Add("Dni", typeof(Int32));
            dt.Columns.Add("CantidadAsistencias", typeof(Int32));
            dt.Columns.Add("Activo", typeof(bool));

            // Se añade cada objeto a la tabla temporal
            foreach (Socio socio in lista)
                dt.Rows.Add(socio.ClubId, socio.Nombre, socio.Apellido, socio.FechaNacimiento, socio.FechaAsociado, socio.Dni, socio.CantidadAsistencias, socio.Activo);

                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Se indica el nombre de la tabla a modificar
                        bulkCopy.DestinationTableName = "dbo.Socio";

                        // Se mapea las columnas de la tabla temporal creada con las columnas de la base
                        bulkCopy.ColumnMappings.Add("ClubId", "ClubId");
                        bulkCopy.ColumnMappings.Add("Nombre", "Nombre");
                        bulkCopy.ColumnMappings.Add("Apellido", "Apellido");
                        bulkCopy.ColumnMappings.Add("FechaNacimiento", "FechaNacimiento");
                        bulkCopy.ColumnMappings.Add("FechaAsociado", "FechaAsociado");
                        bulkCopy.ColumnMappings.Add("Dni", "Dni");
                        bulkCopy.ColumnMappings.Add("CantidadAsistencias", "CantidadAsistencias");
                        bulkCopy.ColumnMappings.Add("Activo", "Activo");

                        bulkCopy.WriteToServer(dt);
                    }
                }

            return true;
        }
        #endregion

        #region Metodo PUT
        public bool SocioModificado(Socio socio)
        {
            string query = "UPDATE Socio " +
                "SET ClubId = @ClubId, " +
                    "Nombre = @Nombre, " +
                    "Apellido = @Apellido, " +
                    "FechaNacimiento = @FechaNacimiento, " +
                    "FechaAsociado = @FechaAsociado, " +
                    "Dni = @Dni, " +
                    "CantidadAsistencias = @CantidadAsistencias " +
                    "Activo = @Activo" +
                "WHERE SocioId = @SocioId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SocioId", socio.SocioId);
                        command.Parameters.AddWithValue("@ClubId", socio.ClubId);
                        command.Parameters.AddWithValue("@Nombre", socio.Nombre);
                        command.Parameters.AddWithValue("@Apellido", socio.Apellido);
                        command.Parameters.AddWithValue("@FechaNacimiento", socio.FechaNacimiento);
                        command.Parameters.AddWithValue("@FechaAsociado", socio.FechaAsociado);
                        command.Parameters.AddWithValue("@Dni", socio.Dni);
                        command.Parameters.AddWithValue("@CantidadAsistencias", socio.CantidadAsistencias);
                        command.Parameters.AddWithValue("@Activo", socio.Activo);

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
        public bool SocioEliminado(int id)
        {
            string query = "DELETE FROM Socio WHERE SocioId = @SocioId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SocioId", id);
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
        public bool SocioDeshabilitado(int id)
        {
            string query = "UPDATE Socio " +
                "SET Activo = @Activo " +
                "WHERE SocioId = @SocioId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Activo", 0);
                        command.Parameters.AddWithValue("@SocioId", id);
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
