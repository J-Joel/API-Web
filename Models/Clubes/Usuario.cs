namespace API_Web.Models.Clubes
{
    public class Usuario
    {
        // Se utiliza mas para el modo local
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public byte[]? ContraseñaHash { get; set; }
        public byte[]? Salt { get; set; }
        public string? Rol { get; set; }
        public bool Activo { get; set; }
    }
}
