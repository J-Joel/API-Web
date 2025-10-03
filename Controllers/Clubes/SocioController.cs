using API_Web.BDD;
using API_Web.Models.Clubes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers.Clubes
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Clubes")]
    public class SocioController : ControllerBase
    {
        #region Controladores GET
        // GET: api/<SocioController>
        [HttpGet]
        public ActionResult<Socio> Get()
        {
            return Ok(BDDConexion.socioTabla.ListadoDeSocio());
        }
        [HttpGet("paginado")]
        public ActionResult<Dirigente> GetPaginado(int pagina = 0, int tamaño = 0)
        {
            if (pagina < 1 || tamaño < 1)
                return BadRequest(new { status = 400, message = "El tamaño y pagina de los registros a visualizar deben ser mayor a 1" });
            return Ok(BDDConexion.socioTabla.ListadoDeSocioPaginado(pagina, tamaño));
        }

        // GET api/<SocioController>/5
        [HttpGet("{id}")]
        public ActionResult<Socio> Get(int id)
        {
            Socio socio = BDDConexion.socioTabla.SocioPorId(id);

            if (socio == null)
                return NotFound(new { status = 404, message = "El ID no es valido" });

            return Ok(socio);
        }
        #endregion

        #region Controladores POST
        // POST api/<SocioController>
        [HttpPost]
        [Authorize]
        public ActionResult Post([FromBody] List<Socio> lista)
        {
            foreach (Socio socio in lista)
            {
                if (BDDConexion.socioTabla.SocioPorDni(socio.Dni) != null)
                    return BadRequest(new { status = 400, message = $"El DNI del Socio: {socio.Nombre}, ya esta registrado" });

                if (socio.FechaNacimiento > socio.FechaAsociado)
                    return BadRequest(new { status = 400, message = $"La Fecha de nacimiento debe ser menor a la Fecha de asociado para el Socio: {socio.Nombre}" });

                if (socio.CantidadAsistencias < 0)
                    return BadRequest(new { status = 400, message = $"La cantidad de asistencias debe ser mayor o igual 0, para el Socio: {socio.Nombre}" });

                if (BDDConexion.clubTabla.ClubPorId(socio.ClubId) == null)
                    return BadRequest(new { status = 400, message = $"El ID del club del Socio: {socio.Nombre}, no es valido" });
            }

            if (!BDDConexion.socioTabla.SociosNuevos(lista))
                return StatusCode(500, new { status = 500, message = "Error al crear el registro" });

            return Created();
        }
        #endregion

        #region Controladores PUT
        // PUT api/<SocioController>/5
        //[HttpPut("{id}")]
        [HttpPut]
        [Authorize]
        public ActionResult Put([FromBody] Socio socio)
        {
            if (BDDConexion.socioTabla.SocioPorId(socio.SocioId) == null)
                return NotFound(new { status = 404, message = "El ID no es valido" });

            using (Socio sociotemp = BDDConexion.socioTabla.SocioPorDni(socio.Dni))
            {
                if (sociotemp != null && sociotemp.SocioId != socio.SocioId)
                    return BadRequest(new { status = 400, message = "El DNI ya esta registrado" });
            }

            if (socio.FechaNacimiento > socio.FechaAsociado)
                return BadRequest(new { status = 400, message = "La Fecha de nacimiento debe ser menor a la Fecha de asociado" });

            if (socio.CantidadAsistencias < 0)
                return BadRequest(new { status = 400, message = "La cantidad de asistencias debe ser mayor o igual 0" });

            if (BDDConexion.clubTabla.ClubPorId(socio.ClubId) == null)
                return BadRequest(new { status = 400, message = "El ID del club no es valido" });

            if (!BDDConexion.socioTabla.SocioModificado(socio))
                return StatusCode(500, new { status = 500, message = "Error al modificar el registro" });

            return NoContent();
        }
        #endregion

        #region Controladores DELETE
        // DELETE api/<SocioController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (BDDConexion.socioTabla.SocioPorId(id) == null)
                return NotFound(new { status = 404, message = "El ID no es valido" });

            if (!BDDConexion.socioTabla.SocioEliminado(id))
                return StatusCode(500, new { status = 500, message = "Error al eliminar el registro" });

            return NoContent();
        }
        [HttpDelete("logico/{id}")]
        [Authorize]
        public ActionResult DeleteLogico(int id)
        {
            using (Socio sociotemp = BDDConexion.socioTabla.SocioPorId(id))
            {
                if (sociotemp == null)
                    return NotFound(new { status = 404, message = "El ID no es valido" });
                if (!sociotemp.Activo)
                    return BadRequest(new { status = 400, message = "El registro ya se encuentra deshabilitado" });
            }

            if (!BDDConexion.socioTabla.SocioDeshabilitado(id))
                return StatusCode(500, new { status = 500, message = "Error al Deshabilitado el registro" });

            return NoContent();
        }
        #endregion
    }
}
