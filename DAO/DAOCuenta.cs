using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using TO;
using System.Data;

namespace DAO
{
    /// <summary>
    /// Clase que se comunica con la base de datos para realizar las diferentes transacciones sobre las cuentas del sistema
    /// </summary>
    public class DAOCuenta
    {
        MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionString);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringM);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringJ);

        // connectionStringJ (Juan Diego)
        // connectionStringM (Melany)
        // connectionString (Asoc. Acompañame)

        /// <summary>
        /// Retorna los datos de la cuenta asociada al nombre de usuario y la contraseña
        /// </summary>
        /// <param name="id">Nombre de usuario de la cuenta</param>
        /// <param name="contra">Contraseña asociada al nombre de usuario</param>
        /// <returns>Demás datos de la cuenta</returns>
        public TOCuenta buscarCuenta(string id, string contra)
        {
            TOCuenta cuenta = new TOCuenta();
            MySqlCommand buscar = new MySqlCommand("select * from cuenta where idUsuario = @id and contrasenna = @contra", conex);
            buscar.Parameters.AddWithValue("@id", id);
            buscar.Parameters.AddWithValue("@contra", contra);

            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            MySqlDataReader reader = buscar.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cuenta.idUsuario = reader.GetString(0);
                    cuenta.nombreEmpleado = reader.GetString(1);
                    cuenta.contrasenna = reader.GetString(2);
                    cuenta.privilegio = reader.GetString(3);
                    cuenta.estado = reader.GetBoolean(4);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return cuenta;

        }

        /// <summary>
        /// Ingresa una nueva cuenta de usuario al sistema
        /// </summary>
        /// <param name="cuenta">Cuenta a ingresar. Objeto que contiene los datos de la cuenta</param>
        public void insertarCuenta(TOCuenta cuenta)
        {
            //String query = "begin tran if exists(select * from cuenta with (updlock, serializable) where idUsuario = @id) " +
            //    "begin update cuenta set nombreEmpleado = @nombre, contrasenna = @contra,  privilegio = @priv, estado = @estado where " +
            //    "idUsuario = @id; end else begin insert into cuenta(idUsuario, nombreEmpleado, contrasenna, privilegio, estado) " +
            //    "values (@id, @nombre, @contra, @priv, @estado); end commit tran;";
            String query = "insert into cuenta(idUsuario, nombreEmpleado, contrasenna, privilegio, estado) values (@id, @nombre, @contra, @priv, @estado);";
            MySqlCommand sentencia = new MySqlCommand(query, conex);
            sentencia.Parameters.AddWithValue("@id", cuenta.idUsuario);
            sentencia.Parameters.AddWithValue("@contra", cuenta.contrasenna);
            sentencia.Parameters.AddWithValue("@nombre", cuenta.nombreEmpleado);
            sentencia.Parameters.AddWithValue("@priv", cuenta.privilegio);
            sentencia.Parameters.AddWithValue("@estado", cuenta.estado);
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            sentencia.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }

        }
        
        /// <summary>
        /// Retorna una lista con todas las cuentas existentes en el sistema
        /// </summary>
        /// <returns>Lista de cuentas en el sistema</returns>
        public List<TOCuenta> listaCuentas()
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select * from cuenta;";
            MySqlCommand cmd = new MySqlCommand(qry, conex);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOCuenta> lista = new List<TOCuenta>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TOCuenta to = new TOCuenta();
                    to.idUsuario = reader.GetString(0);
                    to.nombreEmpleado = reader.GetString(1);
                    to.contrasenna = reader.GetString(2);
                    to.privilegio = reader.GetString(3);
                    to.estado = reader.GetBoolean(4);

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
        /// Verifica si existe una cuenta de usuario bajo el identificador (nombre de usuario) especificado. Retorna los datos
        /// de la cuenta
        /// </summary>
        /// <param name="identificador">Nombre de usuario</param>
        /// <returns>Datos de la cuenta asociada al nombre de usuario</returns>
        public TOCuenta existeId(string identificador)
        {
            TOCuenta cuenta = new TOCuenta();
            MySqlCommand buscar = new MySqlCommand("select * from cuenta where idUsuario = @id", conex);
            buscar.Parameters.AddWithValue("@id", identificador);

            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            MySqlDataReader reader = buscar.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cuenta.idUsuario = reader.GetString(0);
                    cuenta.nombreEmpleado = reader.GetString(1);
                    cuenta.contrasenna = reader.GetString(2);
                    cuenta.privilegio = reader.GetString(3);
                    cuenta.estado = reader.GetBoolean(4);
                }
            }

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return cuenta;
        }

        /// <summary>
        /// Modifica los datos de una cuenta del sistema
        /// </summary>
        /// <param name="cuenta">Cuenta a modificar y datos de la nueva cuenta</param>
        /// <returns>True en caso de que la cuenta se haya modificado correctamente, false de lo contrario</returns>
        public bool actualizarCuenta(TOCuenta cuenta)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "update cuenta set nombreEmpleado = @nombre, contrasenna = @contra, privilegio = @priv, estado = @est, estado = @est where idUsuario = @idCuenta";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idCuenta", cuenta.idUsuario);
            cmd.Parameters.AddWithValue("@nombre", cuenta.nombreEmpleado);
            cmd.Parameters.AddWithValue("@priv", cuenta.privilegio);
            cmd.Parameters.AddWithValue("@est", cuenta.estado);
            cmd.Parameters.AddWithValue("@contra", cuenta.contrasenna);
            int result = cmd.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }
    }
}
