using API_Web.Models.Clubes;

namespace API_Web.BDD.BDLocal.Clubes
{
    public class SocioLocal
    {
        #region BDLocal
        private static List<Socio> listaSocios = new List<Socio>
        {
            new Socio
            {
                SocioId = 0,
                ClubId = 1,
                Nombre = "Juan",
                Apellido = "González",
                FechaNacimiento = new DateOnly(2000,11,20),
                FechaAsociado = new DateOnly(2021,03,10),
                Dni = 40999888,
                CantidadAsistencias = 12,
                Activo = true
            }
        };
        public List<Socio> ListaSocios { get => listaSocios; set => listaSocios = value; }
        #endregion

        #region Metodos GET
        public List<Socio> ListadoDeSocio()
        {
            return ListaSocios;
        }
        public List<Socio> ListadoDeSocioPaginado(int pagina, int tamaño)
        {
            return ListaSocios.Skip((pagina - 1) * tamaño).Take(tamaño).ToList();
        }
        public Socio SocioPorId(int id)
        {
            return ListaSocios.FirstOrDefault(p => p.SocioId == id);
        }
        public Socio SocioPorDni(int dni)
        {
            return ListaSocios.FirstOrDefault(p => p.Dni == dni);
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un registro
        public bool SociosNuevos(List<Socio> lista)
        {
            foreach (Socio socio in lista)
                ListaSocios.Add(new Socio
                {
                    SocioId = UltimoId(),
                    ClubId = socio.ClubId,
                    Nombre = socio.Nombre,
                    Apellido = socio.Apellido,
                    FechaNacimiento = socio.FechaNacimiento,
                    FechaAsociado = socio.FechaAsociado,
                    Dni = socio.Dni,
                    CantidadAsistencias = socio.CantidadAsistencias,
                    Activo = true
                });

            return true;
        }
        public int UltimoId()
        {
            return ListaSocios[ListaSocios.Count() - 1].ClubId + 1;
        }
        #endregion

        #region Metodo PUT
        public bool SocioModificado(Socio socio)
        {
            Socio socioMemoria = SocioPorId(socio.SocioId);

            socioMemoria.ClubId = socio.ClubId;
            socioMemoria.Nombre = socio.Nombre;
            socioMemoria.Apellido = socio.Apellido;
            socioMemoria.FechaNacimiento = socio.FechaNacimiento;
            socioMemoria.FechaAsociado = socio.FechaAsociado;
            socioMemoria.Dni = socio.Dni;
            socioMemoria.CantidadAsistencias = socio.CantidadAsistencias;

            return true;
        }
        #endregion

        #region Metodos DELETE
        public bool SocioEliminado(int id)
        {
            Socio socioMemoria = SocioPorId(id);
            ListaSocios.Remove(socioMemoria);
            return true;
        }
        public bool SocioDeshabilitado(int id)
        {
            Socio socioMemoria = SocioPorId(id);
            socioMemoria.Activo = false;
            return true;
        }
        #endregion
    }
}
