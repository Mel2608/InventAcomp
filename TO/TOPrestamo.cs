﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO
{
    public class TOPrestamo
    {
        public int idPrestamo { get; set; }
        public string numeroContrato { get; set; }
        public string paciente { get; set; }
        public string responsable { get; set; }
        public DateTime fechaPrestamo { get; set; }
        public DateTime fechaEntrega { get; set; }
        public int idArticulo { get; set; }

        public TOPrestamo() { }

        public TOPrestamo(int idPrestamo, string numeroContrato, string paciente, string responsable, DateTime fechaPrestamo, DateTime fechaEntrega, int idArticulo)
        {
            this.idPrestamo = idPrestamo;
            this.numeroContrato = numeroContrato;
            this.paciente = paciente;
            this.responsable = responsable;
            this.fechaPrestamo = fechaPrestamo;
            this.fechaEntrega = fechaEntrega;
            this.idArticulo = idArticulo;
        }

        public TOPrestamo(string numeroContrato, string paciente, string responsable, DateTime fechaPrestamo, DateTime fechaEntrega, int idArticulo)
        {
            this.numeroContrato = numeroContrato;
            this.paciente = paciente;
            this.responsable = responsable;
            this.fechaPrestamo = fechaPrestamo;
            this.fechaEntrega = fechaEntrega;
            this.idArticulo = idArticulo;
        }

    }
}
