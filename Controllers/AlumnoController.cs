using API_Web.Conexion;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnoController : ControllerBase
    {
        // GET: api/<AlumnoController>
        [HttpGet]
        public IEnumerable<Alumno> Get()
        {
            return BDDLocal.listaAlumno; // Se devuelve una lista de la base de datos
        }

        // GET api/<AlumnoController>/5
        [HttpGet("{id}")]
        public ActionResult<Alumno> Get(int id)
        {
            Alumno alumno = BDDLocal.AlumnoPorID(id);
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
            nuevo.Id = BDDLocal.UltimoID() + 1;
            BDDLocal.listaAlumno.Add(nuevo);
            return CreatedAtAction(nameof(Get), new { id = nuevo.Id }, nuevo);
        }

        // PUT api/<AlumnoController>/5
        [HttpPut("{id}")]
        public ActionResult<Alumno> Put(int id, Alumno alumno)
        {
            if (id != alumno.Id) return BadRequest(); // Error 400, la solicitud no se pudo procesar por error del Cliente

            Alumno alumnoExistente = BDDLocal.AlumnoPorID(id);
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
            Alumno alumnoExistente = BDDLocal.AlumnoPorID(id);
            if (alumnoExistente == null) return NotFound(); // Error 404

            BDDLocal.listaAlumno.Remove(alumnoExistente);
            return NoContent(); // Ok 204
        }
    }
}
