using API_Web.Models.Clubes;

namespace API_Web.BDD.BDLocal.Clubes
{
    public class DirigenteLocal
    {
        #region BDLocal
        private static List<Dirigente> listaDirigentes = new List<Dirigente>
        {
            new Dirigente
            {
                DirigenteId = 0,
                ClubId = 0,
                Nombre = "Carla",
                Apellido = "Pérez",
                FechaNacimiento = new DateOnly(1985,07,01),
                Rol = "Presidenta",
                Dni = 30123456,
                Activo = true
            }
        };
        public List<Dirigente> ListaDirigentes { get => listaDirigentes; set => listaDirigentes = value; }
        #endregion

        #region Metodos GET
        public List<Dirigente> ListadoDeDirigente()
        {
            return ListaDirigentes;
        }
        public List<Dirigente> ListadoDeDirigentePaginado(int pagina, int tamaño)
        {
            return ListaDirigentes.Skip((pagina - 1) * tamaño).Take(tamaño).ToList();
        }
        public Dirigente DirigentePorId(int id)
        {
            Dirigente dirigente = ListaDirigentes.FirstOrDefault(p => p.ClubId == id);

            return dirigente;
        }
        public Dirigente DirigentePorDni(int dni)
        {
            Dirigente dirigente = ListaDirigentes.FirstOrDefault(p => p.Dni == dni);

            return dirigente;
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un registro
        public bool DirigentesNuevos(List<Dirigente> lista)
        {
            foreach (Dirigente dirigente in lista)
                ListaDirigentes.Add(new Dirigente
                {
                    DirigenteId = UltimoId(),
                    ClubId = dirigente.ClubId,
                    Nombre = dirigente.Nombre,
                    Apellido = dirigente.Apellido,
                    FechaNacimiento = dirigente.FechaNacimiento,
                    Rol = dirigente.Rol,
                    Dni = dirigente.Dni,
                    Activo = true
                });

            return true;
        }
        public int UltimoId()
        {
            return ListaDirigentes[ListaDirigentes.Count() - 1].ClubId + 1;
        }
        #endregion

        #region Metodo PUT
        public bool DirigenteModificado(Dirigente dirigente)
        {
            Dirigente dirigenteMemoria = DirigentePorId(dirigente.DirigenteId);

            dirigenteMemoria.ClubId = dirigente.ClubId;
            dirigenteMemoria.Nombre = dirigente.Nombre;
            dirigenteMemoria.Apellido = dirigente.Apellido;
            dirigenteMemoria.FechaNacimiento = dirigente.FechaNacimiento;
            dirigenteMemoria.Rol = dirigente.Rol;
            dirigenteMemoria.Dni = dirigente.Dni;

            return true;
        }
        #endregion

        #region Metodos DELETE
        public bool DirigenteEliminado(int id)
        {
            Dirigente dirigenteMemoria = DirigentePorId(id);
            ListaDirigentes.Remove(dirigenteMemoria);
            return true;
        }
        public bool DirigenteDeshabilitado(int id)
        {
            Dirigente dirigenteMemoria = DirigentePorId(id);
            dirigenteMemoria.Activo = false;
            return true;
        }
        #endregion
    }
}
