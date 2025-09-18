using API_Web.BDD;
using API_Web.Models.Historial;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Web.Controllers.Historial
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Historial")]
    public class HistorialOrdenesController : ControllerBase
    {
        // GET: api/<HistorialOrdenesController>
        [HttpGet("get/OpertaionCompleted")]
        public List<HistorialOrdenes> Get_OpertaionCompleted()
        {
            return BDDConexion.historialConexion.OperationCompleted();
        }

        // GET api/<HistorialOrdenesController>/5
        [HttpGet("get/OperationYear/{year}")]
        public List<HistorialOrdenes> Get_OperationYear (int year)
        {

            return BDDConexion.historialConexion.OperationForYear(year);
        }

        // POST api/<HistorialOrdenesController>
        [HttpPost("post/NewOperation")]
        public void Post_NewOperation(string action, string symbol, int quantity, decimal price)
        {
            BDDConexion.historialConexion.NewOperation(action, symbol, quantity, price);
        }
    }
}
