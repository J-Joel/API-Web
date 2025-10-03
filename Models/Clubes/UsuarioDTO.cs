namespace API_Web.Models.Clubes
{
    public class UsuarioDTO : IDisposable
    {
        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Contraseña { get; set; }
        public string? Rol { get; set; }
        public bool Activo { get; set; }

        private bool _isDisposed = false;
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }
        }
    }
}
