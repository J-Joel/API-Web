namespace API_Web.Models.Clubes
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LoginResponse
    {
        public int UsuarioId { get; set; }
        public string? Rol { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresAtUtc { get; set; }
    }
}
