namespace API_Web.Models.Clubes
{
    public class Club : IDisposable
    {
        public int ClubId { get; set; }
        public string? Nombre { get; set; }
        public int CantidadSocios { get; set; }
        public int CantidadTitulos { get; set; }
        public DateOnly FechaFundacion { get; set; }
        public string? UbicacionEstadio { get; set; }
        public string? NombreEstadio { get; set; }
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
