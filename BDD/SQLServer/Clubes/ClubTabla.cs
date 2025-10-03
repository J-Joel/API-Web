using System.Data;
using API_Web.Models.Clubes;
using Microsoft.Data.SqlClient;

namespace API_Web.BDD.SQLServer.Clubes
{
    public class ClubTabla : ClubesConexion // Hereda la conexion para esta clase
    {
        #region Metodos GET
        public List<Club> ListadoDeClub()
        {
            List<Club> list = new List<Club>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM Club WHERE Activo = 1";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Club()
                    {
                        ClubId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        CantidadSocios = reader.GetInt32(2),
                        CantidadTitulos = reader.GetInt32(3),
                        FechaFundacion = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        UbicacionEstadio = reader.GetString(5),
                        NombreEstadio = reader.GetString(6),
                        Activo = reader.GetBoolean(7)
                    });
                }
            }
            return list;
        }
        public List<Club> ListadoDeClubPaginado(int pagina, int tamaño)
        {
            List<Club> list = new List<Club>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM Club WHERE Activo = 1 ORDER BY ClubId OFFSET @Pagina ROWS FETCH NEXT @Tamaño ROWS ONLY";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Pagina", (pagina - 1) * tamaño);
                command.Parameters.AddWithValue("@Tamaño", tamaño);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Club()
                    {
                        ClubId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        CantidadSocios = reader.GetInt32(2),
                        CantidadTitulos = reader.GetInt32(3),
                        FechaFundacion = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        UbicacionEstadio = reader.GetString(5),
                        NombreEstadio = reader.GetString(6),
                        Activo = reader.GetBoolean(7)
                    });
                }
            }
            return list;
        }
        public Club ClubPorId(int id)
        {
            Club club = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM Club WHERE ClubId = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    club = new Club()
                    {
                        ClubId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        CantidadSocios = reader.GetInt32(2),
                        CantidadTitulos = reader.GetInt32(3),
                        FechaFundacion = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        UbicacionEstadio = reader.GetString(5),
                        NombreEstadio = reader.GetString(6),
                        Activo = reader.GetBoolean(7)
                    };
                }
                else
                {
                    return null;
                }
            }
            return club;
        }
        public Club ClubPorNombre(string nombre)
        {
            Club club = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM Club WHERE Nombre = @Nombre";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombre);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    club = new Club()
                    {
                        ClubId = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        CantidadSocios = reader.GetInt32(2),
                        CantidadTitulos = reader.GetInt32(3),
                        FechaFundacion = DateOnly.FromDateTime(reader.GetDateTime(4)),
                        UbicacionEstadio = reader.GetString(5),
                        NombreEstadio = reader.GetString(6),
                        Activo = reader.GetBoolean(7)
                    };
                }
                else
                {
                    return null;
                }
            }
            return club;
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un registro
        public bool ClubesNuevos(List<Club> lista)
        {
            DataTable dt = new DataTable();
            // Se crea las columnas de la tabla temporal
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("CantidadSocios", typeof(Int32));
            dt.Columns.Add("CantidadTitulos", typeof(Int32));
            dt.Columns.Add("FechaFundacion", typeof(DateOnly));
            dt.Columns.Add("UbicacionEstadio", typeof(string));
            dt.Columns.Add("NombreEstadio", typeof(string));
            dt.Columns.Add("Activo", typeof(bool));

            // Se añade cada objeto a la tabla temporal
            foreach (Club club in lista) 
                dt.Rows.Add(club.Nombre, club.CantidadSocios, club.CantidadTitulos, club.FechaFundacion, club.UbicacionEstadio, club.NombreEstadio, club.Activo);
            
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    connection.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        // Se indica el nombre de la tabla a modificar
                        bulkCopy.DestinationTableName = "dbo.Club";

                        // Se mapea las columnas de la tabla temporal creada con las columnas de la base
                        bulkCopy.ColumnMappings.Add("Nombre", "Nombre");
                        bulkCopy.ColumnMappings.Add("CantidadSocios", "CantidadSocios");
                        bulkCopy.ColumnMappings.Add("CantidadTitulos", "CantidadTitulos");
                        bulkCopy.ColumnMappings.Add("FechaFundacion", "FechaFundacion");
                        bulkCopy.ColumnMappings.Add("UbicacionEstadio", "UbicacionEstadio");
                        bulkCopy.ColumnMappings.Add("NombreEstadio", "NombreEstadio");
                        bulkCopy.ColumnMappings.Add("Activo", "Activo");

                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false; // Problema de conexion
            }
            return true;
        }
        #endregion

        #region Metodo PUT
        public bool ClubModificado(Club club)
        {
            string query = "UPDATE Club " +
                "SET Nombre = @Nombre, " +
                    "CantidadSocios = @CantidadSocios, " +
                    "CantidadTitulos = @CantidadTitulos, " +
                    "FechaFundacion = @FechaFundacion, " +
                    "UbicacionEstadio = @UbicacionEstadio, " +
                    "NombreEstadio = @NombreEstadio, " +
                    "Activo = @Activo " +
                "WHERE ClubId = @ClubId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", club.Nombre);
                        command.Parameters.AddWithValue("@CantidadSocios", club.CantidadSocios);
                        command.Parameters.AddWithValue("@CantidadTitulos", club.CantidadTitulos);
                        command.Parameters.AddWithValue("@FechaFundacion", club.FechaFundacion);
                        command.Parameters.AddWithValue("@UbicacionEstadio", club.UbicacionEstadio);
                        command.Parameters.AddWithValue("@NombreEstadio", club.NombreEstadio);
                        command.Parameters.AddWithValue("@Activo", club.Activo);
                        command.Parameters.AddWithValue("@ClubId", club.ClubId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0) // No se encontro el ID, en caso de que el registro sea borrado durante la operacion
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false; // Problema de conexion
            }
            return true;
        }
        #endregion

        #region Metodos DELETE
        public bool ClubEliminado(int id)
        {
            string query = "DELETE FROM Club WHERE ClubId = @ClubId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClubId", id);
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
        public bool ClubDeshabilitado(int id)
        {
            string query = "UPDATE Club " +
                "SET Activo = @Activo " +
                "WHERE ClubId = @ClubId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Activo", 0);
                        command.Parameters.AddWithValue("@ClubId", id);
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
    } /// Fin de la clase "ClubTabla"
}
