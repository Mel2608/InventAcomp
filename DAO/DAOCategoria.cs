﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TO;
using MySql.Data.MySqlClient;

namespace DAO
{
    public class DAOCategoria
    {
        MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringM);
        //MySqlConnection conex = new MySqlConnection(Properties.Settings.Default.connectionStringJ);// connectionStringJ (Juan Diego)
                                                                                                   // connectionStringM (Melany)
        public List<TOCategoria> consultarCategorias()
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            MySqlCommand cmd = new MySqlCommand("Select nombre from categoria order by nombre;", conex);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<TOCategoria> listaCategorias = new List<TOCategoria>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    listaCategorias.Add(new TOCategoria(reader.GetString(0)));
                }
            }

            //MySqlDataAdapter adapt = new MySqlDataAdapter();
            //DataTable tableDa = new DataTable();
            //adapt.SelectCommand = cmd;
            //adapt.Fill(tableDa);
            //if (conex.State != ConnectionState.Closed)
            //{
            //    conex.Close();
            //}

            return listaCategorias;
        }

        public void insertarCategoria(String nombre)
        {
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            String qry = "insert into categoria(nombre) values (@nomb)";
            MySqlCommand cmd = new MySqlCommand(qry, conex);
            if (conex.State != ConnectionState.Open)
            {
                conex.Open();
            }
            cmd.Parameters.AddWithValue("@nomb", nombre);
            cmd.ExecuteNonQuery();
            if (conex.State != ConnectionState.Closed)
            {
                conex.Close();
            }

        }

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

    }
}
