using Microsoft.Data.SqlClient;

namespace API_Web.BDD.SQLServer.Clubes
{
    public class ClubesConexion : IDisposable // Clase padre necesaria para la usabilidad de la clase en un bloque using
    {
        protected string conexionString = $"Data Source=DESKTOP-CA64Q9R;DataBase=ClubesDB;Integrated Security=True;MultipleActiveResultSets=true;Encrypt=True;Persist Security Info=True;TrustServerCertificate=True;";
        private bool _isDisposed = false;
        public bool PruebaConexion()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    connection.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        // Metodo necesario para creacion de clases usables en como "using", es llamado al finalizar el bloque
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }
        }
    }
}
