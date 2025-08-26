using API_Web.Conexion;
using API_Web.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialOrdenesController : ControllerBase
    {
        // GET: api/<HistorialOrdenesController>
        [HttpGet("get/OpertaionCompleted")]
        public List<HistorialOrdenes> Get_OpertaionCompleted()
        {
            return BDDLocal.HistorialConexion.OperationCompleted();
        }

        // GET api/<HistorialOrdenesController>/5
        [HttpGet("get/OperationYear")]
        public List<HistorialOrdenes> Get_OperationYear (int year)
        {

            return BDDLocal.HistorialConexion.OperationForYear(year);
        }

        // POST api/<HistorialOrdenesController>
        [HttpPost("post/NewOperation")]
        public void Post_NewOperation(string action, string symbol, int quantity, decimal price)
        {
            BDDLocal.HistorialConexion.NewOperation(action, symbol, quantity, price);
        }
    }
}
