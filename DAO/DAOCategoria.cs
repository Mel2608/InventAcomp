using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO;
using MySql.Data.MySqlClient;

namespace DAO
{
    /// <summary>
    /// Clase que se comunica con la base de datos para realizar las diferentes transacciones sobre categorías del sistema
    /// </summary>
    public class DAOCategoria
    {
        MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionString);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringM);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringJ);

        // connectionStringJ (Juan Diego)
        // connectionStringM (Melany)
        // connectionString (Asoc. Acompañame)


        /// <summary>
        /// Retorna la lista de categorías existentes en la base de datos, ordenadas por su nombre
        /// </summary>
        /// <returns>Lista de categorías en base de datos</returns>
        public List<TOCategoria> consultarCategorias()
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            MySqlCommand cmd = new MySqlCommand("Select * from categoria order by nombre;", conex);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOCategoria> listaCategorias = new List<TOCategoria>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listaCategorias.Add(new TOCategoria(reader.GetInt32(0), reader.GetString(1)));
                }
            }

            return listaCategorias;
        }

        /// <summary>
        /// Verifica si la categoría especificada existe en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre de la categoría a verificar</param>
        /// <returns>True en caso de que la categoría no exista, false de lo contrario</returns>
        public bool verificarCategoria(string nombre)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            String qry = "select count(*) from categoria where nombre = @nom";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@nom", nombre);
            int result = Convert.ToInt32(cmd.ExecuteScalar());
            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result == 0 ? true : false);
        }
        
        /// <summary>
        /// Inserta una nueva categoría en la base de datos
        /// </summary>
        /// <param name="nombre">Nombre de la nueva categoría a ingresarparam>
        public void insertarCategoria(String nombre)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            String qry = "insert into categoria (nombre) values (@nomb)";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@nomb", nombre);
            cmd.ExecuteNonQuery();
            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
        }

        /// <summary>
        /// Retorna una tabla con el nombre de todas las categorías en la base de datos, ordenadas según su identificador
        /// </summary>
        /// <returns>Tabla que contiene todas las categorías en la base de datos</returns>
        public DataTable consultarCategoriasOrdenId()
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            MySqlCommand cmd = new MySqlCommand("Select nombre from categoria", conex);
            MySqlDataAdapter adapt = new MySqlDataAdapter();
            DataTable tableDa = new DataTable();
            adapt.SelectCommand = cmd;
            adapt.Fill(tableDa);
            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }

            return tableDa;
        }

        /// <summary>
        /// Retorna el nombre de la categoría, si existe, según el identificador de la categoría especificado
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría que se busca</param>
        /// <returns>Nombre de la categoría</returns>
        public string obtenerNombreCategoria(int idCategoria)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select nombre from categoria where idCategoria = @idCat";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idCat", idCategoria);
            string result = (String) cmd.ExecuteScalar();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return result;
        }

        /// <summary>
        /// Retorna la lista de categorías del sistema, a excepción de la categoría especificada
        /// </summary>
        /// <param name="categoriaArticulo">Nombre de la categoría a excluir de la lista</param>
        /// <returns>Lista de categorías del sistema</returns>
        public List<TOCategoria> consultarCategorias(string categoriaArticulo)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select nombre from categoria where nombre not in (@cat) order by nombre";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@cat", categoriaArticulo);

            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOCategoria> listaCategorias = new List<TOCategoria>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listaCategorias.Add(new TOCategoria(reader.GetString(0)));
                }
            }
            
            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return listaCategorias;
        }

        /// <summary>
        /// Elimina una categoría de la base de datos
        /// </summary>
        /// <param name="idCategoria">Identificador numérico de la categoría a eliminar</param>
        /// <returns>True si la categoría fue eliminada correctamente, false de la contrario</returns>
        public bool eliminarCategoria(int idCategoria)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "delete from categoria where idCategoria = @idC";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@idC", idCategoria);
            int result = cmd.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }


        /// <summary>
        /// Modifica el nombre de una categoría existente
        /// </summary>
        /// <param name="idCategoria">Identificador de la categoría a modificar</param>
        /// <param name="nombre">Nuevo nombre de la categoría</param>
        /// <returns>True en caso de que la categoría haya sido modificada correctamente, false de la contrario</returns>
        public bool actualizarCategoria(int idCategoria, string nombre)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "update categoria set nombre = @nom where idCategoria = @idCat";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@nom", nombre);
            cmd.Parameters.AddWithValue("@idCat", idCategoria);
            int result = cmd.ExecuteNonQuery();

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return (result > 0 ? true : false);
        }

        /// <summary>
        /// Retorna el identificador de la categoría especificada
        /// </summary>
        /// <param name="nombre">Nombre de la categoría</param>
        /// <returns>Identificador de la categoría</returns>
        public int obtenerIDCategoria(string nombre)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }

            String qry = "select idCategoria from categoria where nombre = @nom";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            cmd.Parameters.AddWithValue("@nom", nombre);
            int result = Convert.ToInt32(cmd.ExecuteScalar());

            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }
            return result;
        }
    }
}
