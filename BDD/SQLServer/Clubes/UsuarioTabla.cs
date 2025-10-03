using API_Web.Models.Clubes;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace API_Web.BDD.SQLServer.Clubes
{
    public class UsuarioTabla : ClubesConexion
    {
        #region Metodos GET
        public List<UsuarioDTO> ListadoDeUsuario()
        {
            List<UsuarioDTO> list = new List<UsuarioDTO>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT UsuarioId, UsuarioNombre, Rol, Activo FROM Usuario WHERE Activo = 1";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new UsuarioDTO()
                    {
                        UsuarioId = reader.GetInt32(0),
                        UsuarioNombre = reader.GetString(1),
                        Contraseña = "******",
                        Rol = reader.GetString(2),
                        Activo = reader.GetBoolean(3)
                    });
                }
            }
            return list;
        }

        public UsuarioDTO UsuarioPorId(int id)
        {
            UsuarioDTO usuario = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT UsuarioId, UsuarioNombre, Rol, Activo FROM Usuario WHERE UsuarioId = {id}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new UsuarioDTO()
                    {
                        UsuarioId = reader.GetInt32(0),
                        UsuarioNombre = reader.GetString(1),
                        Contraseña = "******",
                        Rol = reader.GetString(2),
                        Activo = reader.GetBoolean(3)
                    };
                }
                else
                {
                    return null;
                }
            }
            return usuario;
        }

        public UsuarioDTO UsuarioPorUsuNomb(string UsuNomb)
        {
            UsuarioDTO usuario = new();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT UsuarioId, UsuarioNombre, Rol, Activo FROM Usuario WHERE UsuarioNombre = @UsuarioNombre";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UsuarioNombre", UsuNomb);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new UsuarioDTO()
                    {
                        UsuarioId = reader.GetInt32(0),
                        UsuarioNombre = reader.GetString(1),
                        Contraseña = "******",
                        Rol = reader.GetString(2),
                        Activo = reader.GetBoolean(3)
                    };
                }
                else
                {
                    return null;
                }
            }
            return usuario;
        }
        #endregion

        #region Metodos POST
        public bool CrearUsuario(UsuarioDTO user, Dictionary<string, byte[]> encript)
        {
            string query = "INSERT INTO Usuario (UsuarioNombre, ContraseñaHash, Salt, Rol, Activo) " +
                                "VALUES (@UsuarioNombre, @ContraseñaHash, @Salt, @Rol, @Activo)";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UsuarioNombre", user.UsuarioNombre);
                        command.Parameters.AddWithValue("@ContraseñaHash", encript["hash"]);
                        command.Parameters.AddWithValue("@Salt", encript["salt"]);
                        command.Parameters.AddWithValue("@Rol", user.Rol);
                        command.Parameters.AddWithValue("@Activo", user.Activo);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0)
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

        #region Metodos PUT
        public bool UsuarioModificado(UsuarioDTO usuario, byte[] encryp)
        {
            string query = "UPDATE Usuario " +
                "SET UsuarioNombre = @UsuarioNombre, " +
                    "ContraseñaHash = @ContraseñaHash, " +
                    "Rol = @Rol, " +
                    "Activo = @Activo " +
                "WHERE UsuarioId = @UsuarioId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UsuarioNombre", usuario.UsuarioNombre);
                        command.Parameters.AddWithValue("@ContraseñaHash", encryp);
                        command.Parameters.AddWithValue("@Rol", usuario.Rol);
                        command.Parameters.AddWithValue("@Activo", usuario.Activo);
                        command.Parameters.AddWithValue("@UsuarioId", usuario.UsuarioId);

                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected == 0) // No se encontro el ID, en caso de que el registro sea borrado durante la operacion
                        {
                            return false;
                        }
                    }
                }
            }
            catch //(Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                return false; // Problema de conexion o logico
            }
            return true;
        }
        #endregion

        #region Metodos DELETE
        public bool UsuarioEliminado(int id)
        {
            string query = "DELETE FROM Usuario WHERE UsuarioId = @UsuarioId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UsuarioId", id);
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
        public bool UsuarioDeshabilitado(int id)
        {
            string query = "UPDATE Usuario " +
                "SET Activo = @Activo " +
                "WHERE UsuarioId = @UsuarioId";
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Activo", 0);
                        command.Parameters.AddWithValue("@UsuarioId", id);
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

        #region Metodos HASH
        public Dictionary<string, byte[]> ContraseñaHashNueva(string contraseña)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(contraseña, salt, 100000, HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(20);

            return new Dictionary<string, byte[]>
            {
                {"hash", hash },
                {"salt", salt }
            };
        }
        public byte[] ContraseñaHashRenovada(UsuarioDTO usu)
        {
            byte[] salt;
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT Salt FROM Usuario WHERE UsuarioId = @UsuarioId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UsuarioId", usu.UsuarioId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    salt = (byte[])reader["Salt"];
                }
                else
                {
                    salt = null;
                }
            }
            if (salt == null)
                return null;

            var pbkdf2 = new Rfc2898DeriveBytes(usu.Contraseña, salt, 100000, HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(20);

            return hash;
        }
        public bool ContraseñaHashValidado(int id, string contra)
        {
            byte[] salt = null;
            byte[] hash = null;
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT ContraseñaHash, Salt FROM Usuario WHERE UsuarioId = @UsuarioId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UsuarioId", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    hash = (byte[])reader["ContraseñaHash"];
                    salt = (byte[])reader["Salt"];
                }
            }
            if (hash == null || salt == null)
                return false;

            var pbkdf2 = new Rfc2898DeriveBytes(contra, salt, 100000, HashAlgorithmName.SHA256);

            byte[] hashGenerado = pbkdf2.GetBytes(20);

            return hashGenerado.SequenceEqual(hash);
        }
        #endregion
    }
}
