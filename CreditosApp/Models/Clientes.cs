using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CuentasAhorroApp.Models
{
    public partial class Clientes
    {
        public Clientes()
        {
            CuentasDeAhorro = new HashSet<CuentasDeAhorro>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Identificador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool EstatusCliente { get; set; }
        [NotMapped]
        public int TotalCuentasAhorro { get; set; }
        [NotMapped]
        public string NombreCompleto { get; set; }

        public virtual ICollection<CuentasDeAhorro> CuentasDeAhorro { get; set; }
    }
}
