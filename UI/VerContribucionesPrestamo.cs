﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BL;

namespace UI
{
    public partial class VerContribucionesPrestamo : Form
    {
        public static int idPrestamo;

        public VerContribucionesPrestamo()
        {
            InitializeComponent();
            gridContribuciones.AutoGenerateColumns = false;
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ManejadorPrestamo manejPrest = new ManejadorPrestamo();
            if (manejPrest.existePrestamo(txtContrato.Text))
            {
                BLPrestamo prestamo = manejPrest.obtenerPrestamoContrato(txtContrato.Text);
                txtPaciente.Text = prestamo.paciente;
                txtResponsable.Text = prestamo.responsable;
                txtFecha.Text = prestamo.fechaPrestamo.ToString("dd/MM/yyyy");
                txtFechaEntrega.Text = prestamo.fechaEntrega.ToString("dd/MM/yyyy");
                cbActivo.Checked = prestamo.estado;
                btnBuscarContribuciones.Enabled = true;
            } else
            {
                MessageBox.Show("No existe un préstamo bajo ese número de contrato");
                txtContrato.Clear();
                txtPaciente.Clear();
                txtResponsable.Clear();
                txtFecha.Clear();
                txtFechaEntrega.Clear();
                cbActivo.Checked = false;
            }
        }

        private void btnBuscarContribuciones_Click(object sender, EventArgs e)
        {
            ManejadorContribucion manejCont = new ManejadorContribucion();
            List<BLContribucion> listaContribuciones = manejCont.contribucionesPrestamo(txtContrato.Text);
            mostrarContribuciones(listaContribuciones);
        }

        private void txtContrato_TextChanged(object sender, EventArgs e)
        {
            btnBuscar.Enabled = !string.IsNullOrEmpty(txtContrato.Text);
        }

        private void mostrarContribuciones(List<BLContribucion> listaContribuciones)
        {
            ManejadorContribucion manejCont = new ManejadorContribucion();
            if (gridContribuciones.DataSource != null)
            {
                gridContribuciones.DataSource = null;
                gridContribuciones.Rows.Clear();
            }

            if (listaContribuciones.Count != 0)
            {
                lblAdvertencia.Visible = false;
                gridContribuciones.ColumnCount = 3;

                gridContribuciones.Columns[0].Name = "Número recibo";
                gridContribuciones.Columns[0].HeaderText = "Número recibo";
                gridContribuciones.Columns[0].DataPropertyName = "numeroRecibo";

                gridContribuciones.Columns[1].Name = "Cuota";
                gridContribuciones.Columns[1].HeaderText = "Cuota";
                gridContribuciones.Columns[1].DataPropertyName = "cuota";

                gridContribuciones.Columns[2].Name = "Fecha";
                gridContribuciones.Columns[2].HeaderText = "Fecha";
                gridContribuciones.Columns[2].DataPropertyName = "fecha";

                gridContribuciones.DataSource = listaContribuciones;
            } else
            {
                lblAdvertencia.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
