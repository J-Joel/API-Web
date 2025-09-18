namespace API_Web.Models.Clubes
{
    public class Dirigente : IDisposable
    {
        public int DirigenteId { get; set; }
        public int ClubId { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string? Rol { get; set; }
        public int Dni { get; set; }
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
