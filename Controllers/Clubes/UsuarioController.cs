using API_Web.BDD;
using API_Web.Models.Clubes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers.Clubes
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Clubes")]
    public class UsuarioController : ControllerBase
    {
        #region Controladores GET
        // GET: api/<UsuarioController>
        [HttpGet]
        public ActionResult<UsuarioDTO> Get()
        {
            return Ok(BDDConexion.usuarioTabla.ListadoDeUsuario());
        }
        // GET api/<UsuarioController>/5
        [HttpGet("Id/{id}")]
        public ActionResult<UsuarioDTO> GetPorId(int id)
        {
            UsuarioDTO usuario = BDDConexion.usuarioTabla.UsuarioPorId(id);
            if (usuario == null) 
                return NotFound(new { status = 404, message = "El ID no es valido" });

            return Ok(usuario);
        }
        [HttpGet("Nombre")]
        public ActionResult<UsuarioDTO> GetPorNombre(string UsuNomb)
        {
            UsuarioDTO usuario = BDDConexion.usuarioTabla.UsuarioPorUsuNomb(UsuNomb);

            if (usuario == null)
                return NotFound(new { status = 404, message = "El usuario no existe" });

            return Ok(usuario);
        }
        #endregion

        #region Controladores POST
        // POST api/<UsuarioController>
        [HttpPost]
        //[Authorize]
        public ActionResult Post(UsuarioDTO usuario)
        {
            if (BDDConexion.usuarioTabla.UsuarioPorUsuNomb(usuario.UsuarioNombre) != null)
                return BadRequest(new { status = 400, message = $"El Nombre de usuario: {usuario.UsuarioNombre} ya esta en uso, reingrese un nuevo nombre" });

            Dictionary<string, byte[]> encryp = BDDConexion.usuarioTabla.ContraseñaHashNueva(usuario.Contraseña);

            if (!BDDConexion.usuarioTabla.CrearUsuario(usuario, encryp))
                return StatusCode(500, new { status = 500, message = "Error al crear el registro" });

            return Created();
        }
        #endregion

        #region Controladores PUT
        // PUT api/<UsuarioController>/5
        [HttpPut]
        [Authorize]
        public ActionResult Put(UsuarioDTO usuario)
        {
            if (BDDConexion.usuarioTabla.UsuarioPorId(usuario.UsuarioId) == null)
                return NotFound(new { status = 404, message = $"El ID es invalido" });

            using (UsuarioDTO usuarioTemp = BDDConexion.usuarioTabla.UsuarioPorUsuNomb(usuario.UsuarioNombre))
            {
                if (usuarioTemp != null && usuarioTemp.UsuarioId != usuario.UsuarioId)
                    return BadRequest(new { status = 400, message = $"El nombre: {usuario.UsuarioNombre} ya esta registrado" });
            }

            byte[] encryp = BDDConexion.usuarioTabla.ContraseñaHashRenovada(usuario);
            
            if (!BDDConexion.usuarioTabla.UsuarioModificado(usuario, encryp))
                return StatusCode(500, new { status = 500, message = "Error al modificar el registro" });

            return NoContent();
        }
        #endregion

        #region Controladores DELETE
        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete(int id)
        {
            if (BDDConexion.usuarioTabla.UsuarioPorId(id) == null)
                return NotFound(new { status = 404, message = $"El ID es invalido" });

            if (!BDDConexion.usuarioTabla.UsuarioEliminado(id))
                return StatusCode(500, new { status = 500, message = "Error al eliminar el registro" });

            return NoContent();
        }
        [HttpDelete("Logico/{id}")]
        public ActionResult DeleteLogico(int id)
        {
            using (UsuarioDTO usuaripTemp = BDDConexion.usuarioTabla.UsuarioPorId(id))
            {
                if (usuaripTemp == null)
                    return NotFound(new { status = 404, message = "El ID no es valido" });
                if (!usuaripTemp.Activo)
                    return BadRequest(new { status = 400, message = "El registro ya se encuentra deshabilitado" });
            }

            if (!BDDConexion.usuarioTabla.UsuarioDeshabilitado(id))
                return StatusCode(500, new { status = 500, message = "Error al Deshabilitado el registro" });

            return NoContent();
        }
        #endregion
    }
}
