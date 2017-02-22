using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using MySql.Data.MySqlClient;

namespace EJEMPLO
{
    class Conexionmysql
    {
        public static MySqlConnection ObtenerConexion()
        {
            MySqlConnection miconexion = new MySqlConnection("server=localhost;user id=root;database=hotelsancarlos;password=");
            miconexion.Open();
            return miconexion;
        }
        public static MySqlConnection Desconectar()
        {
            MySqlConnection miconexion = new MySqlConnection("server=localhost;user id=root;database=hotelsancarlos;password=");
            miconexion.Close();
            return miconexion;
        }

        public static void EjecutarMySql(String Query)
        {
            MySqlCommand MiComando = new MySqlCommand(Query, ObtenerConexion());
            int FilasAfectadas = MiComando.ExecuteNonQuery();
            if (FilasAfectadas > 0)
            {
                //MessageBox.Show("Operacion Realizada Exitosamente", "La base de datos ha sido modificada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //MessageBox.Show("No se pudo realizar la modificacion de la base de datos", "Error del sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
