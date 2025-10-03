using API_Web.Models.Clubes;
using System.Security.Cryptography;

namespace API_Web.BDD.BDLocal.Clubes
{
    public class UsuarioLocal
    {
        #region BDLocal
        private static List<Usuario> listaUsuarios = new List<Usuario>
        {
            new Usuario
            {
                UsuarioId = 0,
                UsuarioNombre = "admin",
                ContraseñaHash = new byte[] { 246, 253, 24, 12, 84, 151, 29, 163, 185, 99, 186, 183, 140, 64, 211, 112, 113, 9, 197, 167 }, // !Admin123
                Salt = new byte[] { 154, 214, 67, 11, 208, 51, 232, 176, 120, 93, 37, 12, 104, 238, 24, 79 }, // ???
                Rol = "Admin",
                Activo = true
            }
        };
        private static List<UsuarioDTO> listaUsuariosDTO = new List<UsuarioDTO>
        {
            new UsuarioDTO
            {
                UsuarioId = 0,
                UsuarioNombre = "admin",
                Contraseña = "*****", // !Admin123
                Rol = "Admin",
                Activo = true
            }
        };
        public List<Usuario> ListaUsuarios { get => listaUsuarios; set => listaUsuarios = value; }
        public List<UsuarioDTO> ListaUsuariosDTO { get => listaUsuariosDTO; set => listaUsuariosDTO = value; }
        #endregion

        #region Metodos GET
        public List<UsuarioDTO> ListadoDeUsuario()
        {
            return ListaUsuariosDTO;
        }

        public UsuarioDTO UsuarioPorId(int id)
        {
            UsuarioDTO usuario = listaUsuariosDTO.FirstOrDefault(p => p.UsuarioId == id);

            return usuario;
        }
        public UsuarioDTO UsuarioPorUsuNomb(string nombre)
        {
            UsuarioDTO usuario = listaUsuariosDTO.FirstOrDefault(p => p.UsuarioNombre == nombre);
            return usuario;
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un registro
        public bool CrearUsuario(UsuarioDTO user, Dictionary<string, byte[]> encript)
        {
            int ultimoId = UltimoId();
            ListaUsuarios.Add(new Usuario
            {
                UsuarioId = ultimoId,
                UsuarioNombre = user.UsuarioNombre,
                ContraseñaHash = encript["hash"],
                Salt = encript["salt"],
                Rol = user.Rol,
                Activo = user.Activo
            });
            ListaUsuariosDTO.Add(new UsuarioDTO
            {
                UsuarioId = ultimoId,
                UsuarioNombre = user.UsuarioNombre,
                Contraseña = "*****",
                Rol = user.Rol,
                Activo = user.Activo
            });

            return true;
        }
        public int UltimoId()
        {
            return ListaUsuarios[listaUsuarios.Count() - 1].UsuarioId + 1;
        }
        #endregion

        #region Metodo PUT
        public bool UsuarioModificado(UsuarioDTO usuario, byte[] encryp)
        {
            Usuario usuarioMemoria = ListaUsuarios.FirstOrDefault(p => p.UsuarioId == usuario.UsuarioId);
            UsuarioDTO usuarioDTOMemoria = UsuarioPorId(usuario.UsuarioId);

            usuarioMemoria.UsuarioNombre = usuario.UsuarioNombre;
            usuarioMemoria.ContraseñaHash = encryp;
            usuarioMemoria.Rol = usuario.Rol;
            usuarioMemoria.Activo = usuario.Activo;

            usuarioDTOMemoria.UsuarioNombre = usuario.UsuarioNombre;
            usuarioDTOMemoria.Rol = usuario.Rol;
            usuarioDTOMemoria.Activo = usuario.Activo;

            return true;
        }
        #endregion

        #region Metodos DELETE
        public bool UsuarioEliminado(int id)
        {
            Usuario usuarioMemoria = ListaUsuarios.FirstOrDefault(p => p.UsuarioId == id);
            UsuarioDTO usuarioDTOMemoria = UsuarioPorId(id);
            ListaUsuarios.Remove(usuarioMemoria);
            listaUsuariosDTO.Remove(usuarioDTOMemoria);
            return true;
        }
        public bool UsuarioDeshabilitado(int id)
        {
            Usuario usuarioMemoria = ListaUsuarios.FirstOrDefault(p => p.UsuarioId == id);
            UsuarioDTO usuarioDTOMemoria = UsuarioPorId(id);

            usuarioMemoria.Activo = false;
            usuarioDTOMemoria.Activo = false;

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
            Usuario usuarioMemoria = ListaUsuarios.FirstOrDefault(p => p.UsuarioId == usu.UsuarioId);

            var pbkdf2 = new Rfc2898DeriveBytes(usu.Contraseña, usuarioMemoria.Salt, 100000, HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(20);

            return hash;
        }
        public bool ContraseñaHashValidado(int id, string contra)
        {
            Usuario usuarioMemoria = ListaUsuarios.FirstOrDefault(p => p.UsuarioId == id);
            
            var pbkdf2 = new Rfc2898DeriveBytes(contra, usuarioMemoria.Salt, 100000, HashAlgorithmName.SHA256);

            byte[] hashGenerado = pbkdf2.GetBytes(20);

            return hashGenerado.SequenceEqual(usuarioMemoria.ContraseñaHash);
        }
        #endregion

        #region Ayuda Precarga
        // Utilizar las funciones donde se desee, los resultados se muestran en la consola, solamente se modica los que estan comentados con "Ingresar"
        public void GeneracionHashByte() // Para el precargado en C#
        {
            string contraseña = "Ingrese su contraseña"; // Ingresar
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(contraseña, salt, 100000, HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(20);

            Console.Write("Hash: { ");

            for (int i = 0; i < hash.Length; i++)
            {
                Console.Write(hash[i]);

                if (i < hash.Length - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine(" }");

            Console.Write("Salt: { ");

            for (int i = 0; i < salt.Length; i++)
            {
                Console.Write(salt[i]);

                if (i < salt.Length - 1)
                {
                    Console.Write(", ");
                }
            }
            Console.WriteLine(" }");
        }
        public void ObtenerCadenaHexadecimal() // Para el precargado en sqlserver
        {
            // Ingresar los bytes obtenidos en la funcion de arriba (ejemplo de: !Admin123, el "conjunto" de hash y salt forma la contraseña, si uno es distinto el otro tambien debe ser distinto)
            byte[] hash = new byte[] { 246, 253, 24, 12, 84, 151, 29, 163, 185, 99, 186, 183, 140, 64, 211, 112, 113, 9, 197, 167 }; // Ingresar
            byte[] salt = new byte[] { 154, 214, 67, 11, 208, 51, 232, 176, 120, 93, 37, 12, 104, 238, 24, 79 }; // Ingresar

            Console.WriteLine("Hash: 0x" + BitConverter.ToString(hash).Replace("-", ""));
            Console.WriteLine("Salt: 0x" + BitConverter.ToString(salt).Replace("-", ""));
        }
        #endregion
    }
}
