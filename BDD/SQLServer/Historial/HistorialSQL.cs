using API_Web.Models.Historial;
using Microsoft.Data.SqlClient;

namespace API_Web.BDD.SQLServer.Historial
{
    public class HistorialSQL
    {
        private string conexionString = $"Data Source=DESKTOP-CA64Q9R;DataBase=aaaa;Integrated Security=True;MultipleActiveResultSets=true;Encrypt=True;Persist Security Info=True;TrustServerCertificate=True;";
        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(conexionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public List<HistorialOrdenes> OperationCompleted()
        {
            List<HistorialOrdenes> list = new List<HistorialOrdenes>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = "SELECT * FROM ORDERS_HISTORY WHERE STATUS = 'EXECUTED'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new HistorialOrdenes()
                    {
                        Tx_Number = reader.GetInt32(0),
                        Order_Date = reader.GetDateTime(1),
                        Action = reader.GetString(2),
                        Status = reader.GetString(3),
                        Symbol = reader.GetString(4),
                        Quantity = reader.GetInt32(5),
                        Price = reader.GetDecimal(6),
                    });
                }
            }
            return list;
        }
        public List<HistorialOrdenes> OperationForYear(int year)
        {
            List<HistorialOrdenes> list = new List<HistorialOrdenes>();
            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                string query = $"SELECT * FROM ORDERS_HISTORY WHERE YEAR(ORDER_DATE) = {year}";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new HistorialOrdenes()
                    {
                        Tx_Number = reader.GetInt32(0),
                        Order_Date = reader.GetDateTime(1),
                        Action = reader.GetString(2),
                        Status = reader.GetString(3),
                        Symbol = reader.GetString(4),
                        Quantity = reader.GetInt32(5),
                        Price = reader.GetDecimal(6),
                    });
                }
            }
            return list;
        }
        public void NewOperation(string action, string symbol, int quantity, decimal price)
        {
            string query = "INSERT INTO ORDERS_HISTORY (ORDER_DATE, ACTION, STATUS, SYMBOL, QUANTITY, PRICE) VALUES (GETDATE(), @Action, 'PENDING', @Symbol, @Quantity, @Price)";

            using (SqlConnection connection = new SqlConnection(conexionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Action", action);
                command.Parameters.AddWithValue("@Symbol", symbol);
                command.Parameters.AddWithValue("@Quantity", quantity);
                command.Parameters.AddWithValue("@Price", price);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
