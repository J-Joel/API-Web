namespace API_Web.Conexion
{
    public static class BDDLocal
    {

        public static dynamic? HistorialConexion  = null;
        private static List<string> Materias = 
            [
            "Programación VI", 
            "Ingeniería de Software II", 
            "Probabilidad y Estadística", 
            "Práctica Profesional III", 
            "Teología", 
            "Programación V", 
            "Investigación Operativa", 
            "Ingeniería de Software I"
            ];
        private static List<string> Nombres =
            [
            "Joel",
            "Facundo",
            "Franco",
            "Fabio",
            "Jennifer",
            "Leandro",
            "Kevin",
            "Mauricio",
            "Noelia",
            "Jose"
            ];
        private static List<string> Apellidos =
            [
            "Yucharico",
            "Gonzales",
            "Corsi",
            "Arleo",
            "Alegre",
            "Salvatierra",
            "Mestre",
            "Camacho",
            "Cuenca",
            "Torrez"
            ];
        public static List<Alumno> listaAlumno = new List<Alumno>
        {
            new Alumno
            {
                Id = 0,
                Nombre = Nombres[Random.Shared.Next(Nombres.Count)],
                Apellido = Apellidos[Random.Shared.Next(Apellidos.Count)],
                Materias = new List<string> 
                {
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)]
                }
            },
            new Alumno
            {
                Id = 1,
                Nombre = Nombres[Random.Shared.Next(Nombres.Count)],
                Apellido = Apellidos[Random.Shared.Next(Apellidos.Count)],
                Materias = new List<string>
                {
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)]
                }
            },
            new Alumno
            {
                Id = 2,
                Nombre = Nombres[Random.Shared.Next(Nombres.Count)],
                Apellido = Apellidos[Random.Shared.Next(Apellidos.Count)],
                Materias = new List<string>
                {
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)],
                    Materias[Random.Shared.Next(Materias.Count)]
                }
            }
        };
        static public Alumno AlumnoPorID(int id)
        {
            return listaAlumno.FirstOrDefault(p => p.Id == id);
        }
        static public int UltimoID()
        {
            return listaAlumno[listaAlumno.Count() - 1].Id;
        }
    }
}
