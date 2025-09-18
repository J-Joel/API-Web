using API_Web.BDD.BDLocal.Alumnos;
using API_Web.Models.Alumnos;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers.Alumnos
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Alumnos")]
    public class AlumnoController : ControllerBase
    {
        // GET: api/<AlumnoController>
        [HttpGet]
        public List<Alumno> Get()
        {
            return AlumnoLocal.listaAlumno; // Se devuelve una lista de la base de datos
        }

        // GET api/<AlumnoController>/5
        [HttpGet("{id}")]
        public ActionResult<Alumno> Get_PorID(int id)
        {
            Alumno alumno = AlumnoLocal.AlumnoPorID(id);
            if (alumno != null)
            {
                return alumno; // Se devuelve un objeto de la ID buscado
            }
            return NotFound(); // Error de busqueda
        }

        // POST api/<AlumnoController>
        [HttpPost]
        // Al colocar como parametro una clase/modelo, el swagger/api genera automáticamente un esquema de datos para completar el JSON
        // Lo malo de esto es que se pierde el seguimiento y control en el caso de que el objeto dependa de otro modelo, pero por lo visto es mejor asi(escalabilidad)...
        public ActionResult<Alumno> Post(Alumno nuevo) 
        {
            nuevo.Id = AlumnoLocal.UltimoID() + 1;
            AlumnoLocal.listaAlumno.Add(nuevo);
            return CreatedAtAction(nameof(Get), new { id = nuevo.Id }, nuevo);
        }

        // PUT api/<AlumnoController>/5
        [HttpPut("{id}")]
        public ActionResult<Alumno> Put(int id, Alumno alumno)
        {
            if (id != alumno.Id) return BadRequest(); // Error 400, la solicitud no se pudo procesar por error del Cliente

            Alumno alumnoExistente = AlumnoLocal.AlumnoPorID(id);
            if (alumnoExistente == null) return NotFound(); // Error 404

            alumnoExistente.Nombre = alumno.Nombre;
            alumnoExistente.Apellido = alumno.Apellido;
            alumnoExistente.Materias = alumno.Materias;

            return NoContent(); // Ok 204 No content, se realizo correctamente el cambio, no se devuelve nada
        }

        // DELETE api/<AlumnoController>/5
        [HttpDelete("{id}")]
        public ActionResult<Alumno> Delete(int id)
        {
            Alumno alumnoExistente = AlumnoLocal.AlumnoPorID(id);
            if (alumnoExistente == null) return NotFound(); // Error 404

            AlumnoLocal.listaAlumno.Remove(alumnoExistente);
            return NoContent(); // Ok 204
        }
    }
}
