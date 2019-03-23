﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using BL;

namespace UI
{
    public partial class ReportePrestamos : Form
    {
        public List<BLPrestamo> listaReporte = new List<BLPrestamo>();

        public ReportePrestamos()
        {
            InitializeComponent();
        }

        private void ReportePrestamos_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSetPrestamo", listaReporte));
            ReportParameter parameter1 = new ReportParameter("nombreUsuario", Manager.getManager().nombre);
            reportViewer1.LocalReport.SetParameters(parameter1);
            this.reportViewer1.RefreshReport();
        }
    }
}
