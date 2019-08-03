using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TO;
using MySql.Data.MySqlClient;

namespace DAO
{
    /// <summary>
    /// Clase que se comunica con la base de datos para realizar las diferentes transacciones sobre contribuciones del sistema
    /// </summary>
    public class DAOContribucion
    {
        MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionString);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringM);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringJ);

        // connectionStringJ (Juan Diego)
        // connectionStringM (Melany)
        // connectionString (Asoc. Acompañame)

        /// <summary>
        /// Inserta una nueva contribución en la base de datos
        /// </summary>
        /// <param name="nuevaCont">Contribución a ingresar. Objeto que contiene los datos de la nueva contribución a ingresar</param>
        /// <returns>True en caso de que la contribución se haya ingresado correctamente, false de la contrario</returns>
        public bool agregarContribucion(TOContribucion nuevaCont)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "insert into contribucion (numeroRecibo, cuota, fecha, idPrestamo) values (@num, @cu, @fe, @idPr);";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            cmd.Parameters.AddWithValue("@num", nuevaCont.numeroRecibo);
            cmd.Parameters.AddWithValue("@cu", nuevaCont.cuota);
            cmd.Parameters.AddWithValue("@fe", nuevaCont.fecha);
            cmd.Parameters.AddWithValue("@idPr", nuevaCont.idPrestamo);
            int result = cmd.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// Retorna una lista de las contribuciones realizadas al préstamo especificado
        /// </summary>
        /// <param name="idPrestamo">Identificador numérico del préstamo</param>
        /// <returns>Lista de contribuciones realizadas a dicho préstamo</returns>
        public List<TOContribucion> obtenerContribucionesPrestamo(int idPrestamo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select * from contribucion where idPrestamo = @idPr;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idPr", idPrestamo);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOContribucion> lista = new List<TOContribucion>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lista.Add(new TOContribucion(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetInt32(4)));
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return lista;
        }

        /// <summary>
        /// Retorna una lista de las contribuciones realizadas al préstamo asociado al número de contrato
        /// </summary>
        /// <param name="contrato">Número de contrato del préstamo</param>
        /// <returns>Lista de contribuciones realizadas a dicho préstamo</returns>
        public List<TOContribucion> contribucionesPrestamo(string contrato)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            string qry = "select c.numeroRecibo, c.cuota, c.fecha from contribucion c join prestamo p on c.idPrestamo = p.idPrestamo where p.numeroContrato = @numCont";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@numCont", contrato);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOContribucion> lista = new List<TOContribucion>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TOContribucion cont = new TOContribucion();
                    cont.numeroRecibo = reader.GetString(0);
                    cont.cuota = reader.GetString(1);
                    cont.fecha = reader.GetDateTime(2);
                    lista.Add(cont);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return lista;
        }
    }
}
