using API_Web.BDD;
using API_Web.Models.Clubes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers.Clubes
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Clubes")]
    public class ClubController : ControllerBase
    {
        #region Controladores GET
        // GET: api/<ClubController>
        [HttpGet]
        public ActionResult<Club> Get() // Se requiere que la respuesta sea un ActionResult para que se pueda enviar los distintos estados en conjunto
        {
            return Ok(BDDConexion.clubTabla.ListadoDeClub());
        }
        [HttpGet("paginado")]
        public ActionResult<Club> GetPaginado(int pagina = 0, int tamaño = 0)
        {
            if (pagina < 1 || tamaño < 1)
                return BadRequest(new { status = 400, message = "El tamaño y pagina de los registros a visualizar deben ser mayor a 1" });
            return Ok(BDDConexion.clubTabla.ListadoDeClubPaginado(pagina, tamaño));
        }

        // GET api/<ClubController>/5
        [HttpGet("{id}")]
        public ActionResult<Club> Get(int id)
        {
            Club club = BDDConexion.clubTabla.ClubPorId(id);

            if (club == null) 
                return NotFound(new { status = 404, message = "El ID no es valido" });

            return Ok(club);
        }
        #endregion

        #region Controladores POST
        // POST api/<ClubController>
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromBody] List<Club> lista) // No se tiene encuenta el id incremental
        {
            if (lista.Count() != lista.Select(c => c.Nombre).Distinct().ToList().Count())
                return BadRequest(new { status = 400, message = $"Los nombres de los clubes ingresados deben ser unicos entre si" });
            
            foreach (Club club in lista)
            {
                if (BDDConexion.clubTabla.ClubPorNombre(club.Nombre) != null)
                    return BadRequest(new { status = 400, message = $"El nombre: {club.Nombre} ya esta en uso, reingrese un nuevo nombre" });

                if (club.CantidadSocios < 0 || club.CantidadTitulos < 0)
                    return BadRequest(new { status = 400, message = $"La cantidad de Titulos y Socios del club: {club.Nombre}, deben ser mayor o igual a 0" });

                if (club.FechaFundacion > DateOnly.FromDateTime(DateTime.Now))
                    return BadRequest(new { status = 400, message = $"La fecha de fundación del club: {club.Nombre}, es invalida" });
            }

            if (!BDDConexion.clubTabla.ClubesNuevos(lista))
                return StatusCode(500, new { status = 500, message = "Error al crear el registro" });
            
            return Created();
        }
        #endregion

        #region Controladores PUT
        // PUT api/<ClubController>/5
        //[HttpPut("{id}")] // Diria que no es necesario indicar el id por separado, ademas del que el cliente tendra acceso a ese dato, el mismo objeto tendra el id
        // PUT api/<ClubController>
        [HttpPut]
        [Authorize]
        public ActionResult Put([FromBody] Club club)
        {
            if (BDDConexion.clubTabla.ClubPorId(club.ClubId) == null)
                return BadRequest(new { status = 400, message = $"El ID es invalido" });

            using (Club clubtemp = BDDConexion.clubTabla.ClubPorNombre(club.Nombre))
            {
                if (clubtemp != null && clubtemp.ClubId != club.ClubId)
                    return BadRequest(new { status = 400, message = $"El nombre: {club.Nombre} ya esta registrado" });
            }

            if (club.CantidadSocios < 0 || club.CantidadTitulos < 0)
                return BadRequest(new { status = 400, message = "La cantidad de Titulos y Socios deben ser mayor o igual a 0" });

            if (club.FechaFundacion > DateOnly.FromDateTime(DateTime.Now))
                return BadRequest(new { status = 400, message = "La fecha de fundación es invalida" });

            if (!BDDConexion.clubTabla.ClubModificado(club))
                return StatusCode(500, new { status = 500, message = "Error al modificar el registro" });

            return NoContent();
        }
        #endregion

        #region Controladores DELETE
        // DELETE api/<ClubController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (BDDConexion.clubTabla.ClubPorId(id) == null)
                return NotFound(new { status = 404, message = "El ID no es valido" });

            if (!BDDConexion.clubTabla.ClubEliminado(id))
                return StatusCode(500, new { status = 500, message = "Error al eliminar el registro" });

            return NoContent();
        }
        [HttpDelete("logico/{id}")]
        [Authorize]
        public ActionResult DeleteLogico(int id)
        {
            using (Club clubtemp = BDDConexion.clubTabla.ClubPorId(id)) 
            {
                if (clubtemp == null)
                    return NotFound(new { status = 404, message = "El ID no es valido" });
                if (!clubtemp.Activo)
                    return BadRequest(new { status = 400, message = "El registro ya se encuentra deshabilitado" });
            }

            if (!BDDConexion.clubTabla.ClubDeshabilitado(id))
                return StatusCode(500, new { status = 500, message = "Error al Deshabilitado el registro" });

            return NoContent();
        }
        #endregion

        // Validaciones con FluentValidation - mas validaciones que no se pidio(no hace falta)
        // Mapeos con AutoMapper(Genera conflictos o la documentacion es antigua, posibilidad de empeorar el rendimiento) - El uso de esta metodologia facilitaria el "modo local".
    }
}
