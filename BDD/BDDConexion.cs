using API_Web.BDD.BDLocal.Historial;
using API_Web.BDD.SQLServer.Historial;

using API_Web.BDD.SQLServer.Clubes;

namespace API_Web.BDD
{
    public static class BDDConexion
    {
        public static dynamic? historialConexion = null;

        public static dynamic? clubTabla = null;
        public static dynamic? dirigenteTabla = null;
        public static dynamic? socioTabla = null;
        public static bool EstablecerConexion()
        {
            historialConexion = new HistorialSQL();
            if (!historialConexion.TestConnection())
            {
                Console.WriteLine("Error en la conexion de la BDD: MaxTheMuleBroker, se accedera a la BDD local de HistorialLocal");
                historialConexion = new HistorialLocal();
            }

            using (ClubesConexion conexion = new ClubesConexion())
            {
                if (!conexion.PruebaConexion())
                {
                    Console.WriteLine("Error en la conexion de la BDD: ClubesDB, se accedera a la BDD local de ClubesLocal");
                }
                else 
                {
                    clubTabla = new ClubTabla();
                    dirigenteTabla = new DirigenteTabla();
                    socioTabla = new SocioTabla();
                }
            }
            return true;
        }
    }
}
