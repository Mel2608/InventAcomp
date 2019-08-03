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
    /// Clase que se comunica con la base de datos para realizar los diferentes préstamos sobre las cuentas del sistema
    /// </summary>
    public class DAOPrestamo
    {
        MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionString);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringM);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringJ);

        // connectionStringJ (Juan Diego)
        // connectionStringM (Melany)
        // connectionString (Asoc. Acompañame)


        /// <summary>
        /// Ingresa un nuevo préstamo en la base de datos
        /// </summary>
        /// <param name="nuevoPrest">Préstamo a ingresar. Objeto Préstamo que contiene los datos del préstamo a ingresar</param>
        /// <returns>True en caso de que el préstamo se haya ingresado correctamente, false de la contrario</returns>
        public bool agregarPrestamo(TOPrestamo nuevoPrest)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "insert into prestamo (numeroContrato, paciente, responsable, fechaPrestamo, fechaEntrega, estado, idArticulo) values (@con, @pac, @res, @fecPr, @fecEn, 1, @idArt);";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            cmd.Parameters.AddWithValue("@con", nuevoPrest.numeroContrato);
            cmd.Parameters.AddWithValue("@pac", nuevoPrest.paciente);
            cmd.Parameters.AddWithValue("@res", nuevoPrest.responsable);
            cmd.Parameters.AddWithValue("@fecPr", nuevoPrest.fechaPrestamo);
            cmd.Parameters.AddWithValue("@fecEn", nuevoPrest.fechaEntrega);
            cmd.Parameters.AddWithValue("@idArt", nuevoPrest.idArticulo);
            int result = cmd.ExecuteNonQuery();

            bool prestado = articuloPrestado(nuevoPrest.idArticulo);

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }

            if (prestado && result > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Establece el estado del artículo como 'prestado' (prestado = 1). Utilizado al realizar un nuevo préstamo. Método
        /// usado localmente
        /// </summary>
        /// <param name="idArticulo">Identificador numérico del artículo</param>
        /// <returns>True en caso de que el artículo se haya modificado correctamente, false de la contrario</returns>
        private bool articuloPrestado(int idArticulo)
        {
            String qry = "update articulo set prestado = 1 where idArticulo = @idArt";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            cmd.Parameters.AddWithValue("@idArt", idArticulo);

            int res = cmd.ExecuteNonQuery();
            return (res > 0 ? true : false);
        }

        /// <summary>
        /// Modifica la categoría de un nuevo artículo a 'Préstamo'. Método obsoleto, no utilizado actualmente
        /// </summary>
        /// <param name="idArticulo">Identificador numérico del artículo a modificar</param>
        /// <returns>True en caso de que el artículo se haya modificado correctamente, false de la contrario</returns>
        private bool modificarCategoriaPrestamo(int idArticulo)
        {
            String qry = "update articulo set idCategoria = (select idCategoria from categoria where nombre = 'Préstamo') where idArticulo = @idArt";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            cmd.Parameters.AddWithValue("@idArt", idArticulo);

            int res = cmd.ExecuteNonQuery();
            return (res > 0 ? true : false);
        }
        
        /// <summary>
        /// Retorna una lista con todos los préstamos asociados al artículo especificado
        /// </summary>
        /// <param name="idArticulo">Identificador numérico del artículo</param>
        /// <returns>Lista de préstamos asociados al artículo</returns>
        public List<TOPrestamo> obtenerPrestamosArticulo(int idArticulo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select * from prestamo where idArticulo = @idArt;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idArt", idArticulo);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOPrestamo> lista = new List<TOPrestamo>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lista.Add(new TOPrestamo(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetDateTime(5), reader.GetInt32(6)));
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return lista;
        }

        /// <summary>
        /// Elimina un préstamo de la base de datos
        /// </summary>
        /// <param name="idPrestamo">Identificador del préstamo a eliminar</param>
        /// <returns>True en caso de que el artículo se haya eliminado correctamente, false de la contrario</returns>
        public bool eliminarPrestamo(int idPrestamo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "delete from prestamo where idPrestamo = @idPres;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idPres", idPrestamo);

            int result = cmd.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// Retorna una lista con datos del préstamo, y su artículo correspondiente, que actualmente se encuentren bajo préstamo
        /// </summary>
        /// <returns>Lista de préstamos actualmente en condición de préstamo</returns>
        public List<TOPrestamo> obtenerArticulosPrestamo()
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            String qry = "select p.idPrestamo, p.numeroContrato, p.paciente, p.responsable, p.fechaPrestamo, p.fechaEntrega, a.numeroPlaca, a.nombre from prestamo p join articulo a on p.idArticulo = a.idArticulo where p.estado = 1;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOPrestamo> lista = new List<TOPrestamo>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lista.Add(new TOPrestamo(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetDateTime(4), reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return lista;
        }
        
        /// <summary>
        /// Retorna una lista con los artículos que actualmente se encuentren disponibles para prestar (que no estén
        /// bajo préstamo)
        /// </summary>
        /// <returns>Lista con los artículos disponibles para prestar</returns>
        public List<TOArticulo> consultarArticulosDisponibles()
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select a.idArticulo, a.numeroPlaca, a.nombre, a.fechaIngreso, a.descripcion, a.estado, a.ubicacion, c.nombre from articulo a join categoria c on a.idCategoria = c.idCategoria where a.prestado = 0;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOArticulo> lista = new List<TOArticulo>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TOArticulo to = new TOArticulo();
                    to.idArticulo = reader.GetInt32(0);
                    to.numeroPlaca = reader.GetString(1);
                    to.nombArticulo = reader.GetString(2);
                    to.fechaIngreso = reader.GetDateTime(3);
                    to.descripcArticulo = reader.GetString(4);
                    to.estadoArticulo = reader.GetString(5);
                    to.ubicacionArticulo = reader.GetString(6);
                    to.nombreCategoria = reader.GetString(7);
                    lista.Add(to);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return lista;
        }

        /// <summary>
        /// Desactiva el préstamo de la base de datos (no lo elimina). Además, establece el estado de 'prestado' del
        /// artículo como 0
        /// </summary>
        /// <param name="idPrestamo"></param>
        /// <returns></returns>
        public bool terminarPrestamo(int idPrestamo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            string query1 = "select idArticulo from prestamo where idPrestamo = @idPr";
            MySqlCommand cmd1 = new MySqlCommand(query1, conex);
            cmd1.Parameters.AddWithValue("@idPr", idPrestamo);
            int idArticuloPrestamo = Convert.ToInt32(cmd1.ExecuteScalar());

            string query2 = "update prestamo set estado = 0 where idPrestamo = @idPre;" +
                "update articulo set prestado = 0 where idArticulo = @idArt";
            MySqlCommand cmd2 = new MySqlCommand(query2, conex);
            cmd2.Parameters.AddWithValue("@idPre", idPrestamo);
            cmd2.Parameters.AddWithValue("@idArt", idArticuloPrestamo);
            int result = cmd2.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// Retorna el identificador numérico del artículo asociado al préstamo especificado. No necesariamente el artículo
        /// debe encontrarse en condición de préstamo
        /// </summary>
        /// <param name="idPrestamo">Identificador numérico del préstamo</param>
        /// <returns>Identificador del artículo</returns>
        public Int32 articuloEnPrestamo(int idPrestamo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            string query = "select idArticulo from prestamo where idPrestamo = @idPr";
            MySqlCommand cmd = new MySqlCommand(query, conex);
            cmd.Parameters.AddWithValue("@idPr", idPrestamo);
            int idArticuloPrestamo = Convert.ToInt32(cmd.ExecuteScalar());

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return idArticuloPrestamo;
        }

        /// <summary>
        /// Retorna los datos del préstamo según su identificador
        /// </summary>
        /// <param name="idPrestamo">Identificador numérico del préstamo</param>
        /// <returns>Demás datos del préstamo</returns>
        public TOPrestamo obtenerPrestamo(int idPrestamo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select numeroContrato, paciente, responsable, fechaPrestamo, fechaEntrega, idArticulo from prestamo where idPrestamo = @idPrest;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idPrest", idPrestamo);

            MySqlDataReader reader = cmd.ExecuteReader();
            TOPrestamo prestamo = new TOPrestamo();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    prestamo.numeroContrato = reader.GetString(0);
                    prestamo.paciente = reader.GetString(1);
                    prestamo.responsable = reader.GetString(2);
                    prestamo.fechaPrestamo = reader.GetDateTime(3);
                    prestamo.fechaEntrega = reader.GetDateTime(4);
                    prestamo.idArticulo = reader.GetInt32(5);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return prestamo;
        }

        /// <summary>
        /// Retorna una lista con el histórico de préstamos asociados con el artículo especificado
        /// </summary>
        /// <param name="idArticulo">Identificador numérico del artículo</param>
        /// <returns>Lista con los préstamos asociados al artículo</returns>
        public List<TOPrestamo> prestamosArticulo(int idArticulo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            string qry = "select idPrestamo, numeroContrato, paciente, responsable, fechaPrestamo, fechaEntrega from prestamo where idArticulo = @idArt;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idArt", idArticulo);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOPrestamo> lista = new List<TOPrestamo>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TOPrestamo prest = new TOPrestamo();
                    prest.idPrestamo = reader.GetInt32(0);
                    prest.numeroContrato = reader.GetString(1);
                    prest.paciente = reader.GetString(2);
                    prest.responsable = reader.GetString(3);
                    prest.fechaPrestamo = reader.GetDateTime(4);
                    prest.fechaEntrega = reader.GetDateTime(5);
                    lista.Add(prest);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return lista;
        }

        /// <summary>
        /// Indica si existe un préstamo en la base de datos bajo el número de contrato especificado
        /// </summary>
        /// <param name="contrato">Número de contrato del préstamo</param>
        /// <returns>True si existe el préstamo, false de lo contrario</returns>
        public bool existePrestamo(string contrato)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            string qry = "select count(*) from prestamo where numeroContrato = @numCont";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@numCont", contrato);
            int result = Convert.ToInt32(cmd.ExecuteScalar());

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// Retorna los datos del préstamo según su número de contrato
        /// </summary>
        /// <param name="contrato">Número de contrato del préstamo</param>
        /// <returns>Demás datos del préstamo</returns>
        public TOPrestamo obtenerPrestamoContrato(string contrato)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            string qry = "select paciente, responsable, FechaPrestamo, FechaEntrega, estado from prestamo where NumeroContrato = @numCont;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@numCont", contrato);

            MySqlDataReader reader = cmd.ExecuteReader();
            TOPrestamo prest = new TOPrestamo();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    prest.paciente = reader.GetString(0);
                    prest.responsable = reader.GetString(1);
                    prest.fechaPrestamo = reader.GetDateTime(2);
                    prest.fechaEntrega = reader.GetDateTime(3);
                    prest.estado = reader.GetBoolean(4);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return prest;
        }

    }
}
