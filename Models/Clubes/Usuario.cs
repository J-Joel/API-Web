namespace API_Web.Models.Clubes
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public byte[]? ContraseñaHash { get; set; }
        public byte[]? Salt { get; set; }
        public string? Rol { get; set; }
        public bool Activo { get; set; }
    }
}
