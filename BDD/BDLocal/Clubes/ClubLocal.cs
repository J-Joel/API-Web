using API_Web.Models.Clubes;

namespace API_Web.BDD.BDLocal.Clubes
{
    public class ClubLocal
    {
        #region BDLocal
        private static List<Club> listaClubes = new List<Club>
        {
            new Club
            {
                ClubId = 0,
                Nombre = "Club Atlético Ejemplo",
                CantidadSocios = 1200,
                CantidadTitulos = 5,
                FechaFundacion = new DateOnly(1980,03,15),
                UbicacionEstadio = "Av. Siempre Viva 123, CABA",
                NombreEstadio = "Estadio Central",
                Activo = true
            }
        };
        public List<Club> ListaClubes { get => listaClubes; set => listaClubes = value; }
        #endregion

        #region Metodos GET
        public List<Club> ListadoDeClub()
        {
            return ListaClubes;
        }
        public List<Club> ListadoDeClubPaginado(int pagina, int tamaño)
        {
            return ListaClubes.Skip((pagina - 1) * tamaño).Take(tamaño).ToList();
        }
        public Club ClubPorId(int id)
        {
            Club club = ListaClubes.FirstOrDefault(p => p.ClubId == id);

            return club;
        }
        public Club ClubPorNombre(string nombre)
        {
            Club club = ListaClubes.FirstOrDefault(p => p.Nombre == nombre);

            return club;
        }
        #endregion

        #region Metodo POST
        // Carga multiples registros como tambien un registro
        public bool ClubesNuevos(List<Club> lista)
        {
            foreach (Club club in lista)
                ListaClubes.Add(new Club
                {
                    ClubId = UltimoId(),
                    Nombre = club.Nombre,
                    CantidadSocios = club.CantidadSocios,
                    CantidadTitulos = club.CantidadTitulos,
                    FechaFundacion = club.FechaFundacion,
                    UbicacionEstadio = club.UbicacionEstadio,
                    NombreEstadio = club.NombreEstadio,
                    Activo = true
                });

            return true;
        }
        public int UltimoId()
        {
            return ListaClubes[ListaClubes.Count() - 1].ClubId + 1;
        }
        #endregion

        #region Metodo PUT
        public bool ClubModificado(Club club)
        {
            Club clubMemoria = ClubPorId(club.ClubId);

            clubMemoria.Nombre = club.Nombre;
            clubMemoria.CantidadSocios = club.CantidadSocios;
            clubMemoria.CantidadTitulos = club.CantidadTitulos;
            clubMemoria.FechaFundacion = club.FechaFundacion;
            clubMemoria.UbicacionEstadio = club.UbicacionEstadio;
            clubMemoria.NombreEstadio = club.NombreEstadio;

            return true;
        }
        #endregion

        #region Metodos DELETE
        public bool ClubEliminado(int id)
        {
            Club club = ClubPorId(id);
            listaClubes.Remove(club);
            return true;
        }
        public bool ClubDeshabilitado(int id)
        {
            Club clubMemoria = ClubPorId(id);
            clubMemoria.Activo = false;
            return true;
        }
        #endregion
    }
}
