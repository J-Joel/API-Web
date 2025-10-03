using API_Web.BDD;
using API_Web.Models.Clubes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers.Clubes
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Clubes")]
    public class DirigenteController : ControllerBase
    {
        #region Controladores GET
        // GET: api/<DirigenteController>
        [HttpGet]
        public ActionResult<Dirigente> Get()
        {
            return Ok(BDDConexion.dirigenteTabla.ListadoDeDirigente());
        }
        [HttpGet("paginado")]
        public ActionResult<Dirigente> GetPaginado(int pagina = 0, int tamaño = 0)
        {
            if (pagina < 1 || tamaño < 1)
                return BadRequest(new { status = 400, message = "El tamaño y pagina de los registros a visualizar deben ser mayor a 1" });
            return Ok(BDDConexion.dirigenteTabla.ListadoDeDirigentePaginado(pagina, tamaño));
        }

        // GET api/<DirigenteController>/5
        [HttpGet("{id}")]
        public ActionResult<Dirigente> Get(int id)
        {
            Dirigente dirigente = BDDConexion.dirigenteTabla.DirigentePorId(id);

            if (dirigente == null)
                return NotFound(new { status = 400, message = "El ID no es valido" });

            return Ok(dirigente);
        }
        #endregion

        #region Controladores POST
        // POST api/<DirigenteController>
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromBody] List<Dirigente> lista)
        {
            foreach (Dirigente dirigente in lista)
            {
                if (BDDConexion.dirigenteTabla.DirigentePorDni(dirigente.Dni) != null)
                    return BadRequest(new { status = 400, message = $"El DNI del Dirigente: {dirigente.Nombre}, ya esta registrado" });

                if (BDDConexion.clubTabla.ClubPorId(dirigente.ClubId) == null)
                    return BadRequest(new { status = 400, message = $"El ID del club del Dirigente: {dirigente.Nombre}, no es valido" });
            }

            if (!BDDConexion.dirigenteTabla.DirigentesNuevos(lista))
                return StatusCode(500, new { status = 500, message = "Error al crear el registro" });

            return Created();
        }
        #endregion

        #region Controladores PUT
        // PUT api/<DirigenteController>/5
        //[HttpPut("{id}")]
        [HttpPut]
        [Authorize]
        public ActionResult Put([FromBody] Dirigente dirigente)
        {
            if (BDDConexion.dirigenteTabla.DirigentePorId(dirigente.DirigenteId) == null)
                return NotFound(new { status = 400, message = "El ID no es valido" });

            using (Dirigente dirigentetemp = BDDConexion.dirigenteTabla.DirigentePorDni(dirigente.Dni))
            {
                if (dirigentetemp != null && dirigentetemp.DirigenteId != dirigente.DirigenteId)
                    return BadRequest(new { status = 400, message = "El DNI ya esta registrado" });
            }

            if (BDDConexion.clubTabla.ClubPorId(dirigente.ClubId) == null)
                return BadRequest(new { status = 400, message = "El ID del club no es valido" });

            if (!BDDConexion.dirigenteTabla.DirigenteModificado(dirigente))
                return StatusCode(500, new { status = 500, message = "Error al modificar el registro" });

            return NoContent();
        }
        #endregion

        #region Controladores DELETE
        // DELETE api/<DirigenteController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (BDDConexion.dirigenteTabla.DirigentePorId(id) == null)
                return NotFound(new { status = 400, message = "El ID no es valido" });

            if (!BDDConexion.dirigenteTabla.DirigenteEliminado(id))
                return StatusCode(500, new { status = 500, message = "Error al eliminar el registro" });

            return NoContent();
        }
        [HttpDelete("logico/{id}")]
        [Authorize]
        public ActionResult DeleteLogico(int id)
        {
            using (Dirigente dirigentetemp = BDDConexion.dirigenteTabla.DirigentePorId(id))
            {
                if (dirigentetemp == null)
                    return NotFound(new { status = 404, message = "El ID no es valido" });
                if (!dirigentetemp.Activo)
                    return BadRequest(new { status = 400, message = "El registro ya se encuentra deshabilitado" });
            }

            if (!BDDConexion.dirigenteTabla.DirigenteDeshabilitado(id))
                return StatusCode(500, new { status = 500, message = "Error al Deshabilitado el registro" });

            return NoContent();
        }
        #endregion
    }
}
