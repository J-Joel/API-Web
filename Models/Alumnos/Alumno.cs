namespace API_Web.Models.Alumnos
{
    public class Alumno
    {
        public int Id {  get; set; }
        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public List<string>? Materias { get; set; }

    }
}
