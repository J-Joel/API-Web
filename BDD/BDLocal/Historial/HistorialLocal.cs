using API_Web.Models.Historial;

namespace API_Web.BDD.BDLocal.Historial
{
    public class HistorialLocal
    {
        private static List<HistorialOrdenes> listOrders = new List<HistorialOrdenes>
        {
            new HistorialOrdenes
            {
                Tx_Number = 1,
                Order_Date = Convert.ToDateTime("2023-08-01 10:00:00"),
                Action = "BUY",
                Status = "PENDING",
                Symbol = "SPY",
                Quantity = 100,
                Price = 350.50m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 2,
                Order_Date = Convert.ToDateTime("2023-08-01 11:30:00"),
                Action = "SELL",
                Status = "EXECUTED",
                Symbol = "AAPL",
                Quantity = 50,
                Price = 150.25m,
                
            },
            new HistorialOrdenes
            {
                Tx_Number = 3,
                Order_Date = Convert.ToDateTime("2023-08-02 09:45:00"),
                Action = "BUY",
                Status = "PENDING",
                Symbol = "QQQ",
                Quantity = 200,
                Price = 380.75m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 4,
                Order_Date = Convert.ToDateTime("2023-08-02 14:20:00"),
                Action = "SELL",
                Status = "EXECUTED",
                Symbol = "IWM",
                Quantity = 75,
                Price = 220.00m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 5,
                Order_Date = Convert.ToDateTime("2023-08-03 10:15:00"),
                Action = "BUY",
                Status = "PENDING",
                Symbol = "IBM",
                Quantity = 150,
                Price = 120.50m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 6,
                Order_Date = Convert.ToDateTime("2023-08-03 12:45:00"),
                Action = "SELL",
                Status = "EXECUTED",
                Symbol = "DIA",
                Quantity = 30,
                Price = 275.80m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 7,
                Order_Date = Convert.ToDateTime("2023-08-04 11:00:00"),
                Action = "BUY",
                Status = "PENDING",
                Symbol = "GLD",
                Quantity = 80,
                Price = 180.40m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 8,
                Order_Date = Convert.ToDateTime("2023-08-04 13:30:00"),
                Action = "SELL",
                Status = "EXECUTED",
                Symbol = "SPY",
                Quantity = 120,
                Price = 355.20m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 9,
                Order_Date = Convert.ToDateTime("2023-08-05 10:30:00"),
                Action = "BUY",
                Status = "PENDING",
                Symbol = "AAPL",
                Quantity = 60,
                Price = 155.75m,
            },
            new HistorialOrdenes
            {
                Tx_Number = 10,
                Order_Date = Convert.ToDateTime("2023-08-05 15:15:00"),
                Action = "SELL",
                Status = "EXECUTED",
                Symbol = "QQQ",
                Quantity = 180,
                Price = 385.10m,
            },
        };
        public List<HistorialOrdenes> ListOrders { get => listOrders; set => listOrders = value; }
        public List<HistorialOrdenes> OperationCompleted()
        {
            List<HistorialOrdenes> list = new List<HistorialOrdenes>();
            foreach (HistorialOrdenes ord in ListOrders)
            {
                if(ord.Status == "EXECUTED") list.Add(ord);
            }
            return list;
        }
        public List<HistorialOrdenes> OperationForYear(int year)
        {
            List<HistorialOrdenes> list = new List<HistorialOrdenes>();
            foreach (HistorialOrdenes ord in ListOrders)
            {
                if (ord.Order_Date.Year == year) list.Add(ord);
            }
            return list;
        }
        public int LastId()
        {
            return ListOrders[ListOrders.Count() - 1].Tx_Number;
        }
        public void NewOperation(string action, string symbol, int quantity, decimal price)
        {
            ListOrders.Add(new HistorialOrdenes {
                Tx_Number = LastId() + 1,
                Order_Date = DateTime.Now,
                Action = action,
                Status = "PENDING",
                Symbol = symbol,
                Quantity = quantity,
                Price = price
            });
        }
    }
}
