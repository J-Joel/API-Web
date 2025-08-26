namespace API_Web.Models
{
    public class HistorialOrdenes
    {
        public int Tx_Number { get; set; }
        public DateTime Order_Date { get; set; }

        public string? Action { get; set; }

        public string? Status { get; set; }
        public string? Symbol { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
