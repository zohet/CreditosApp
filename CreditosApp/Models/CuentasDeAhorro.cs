using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuentasAhorroApp.Models
{
    public partial class CuentasDeAhorro
    {
        public CuentasDeAhorro()
        {
            Transacciones = new HashSet<Transacciones>();
        }

        public int Id { get; set; }
        public string NumeroCuenta { get; set; }
        public string NombreCuenta { get; set; }
        public double Saldo { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool EstatusCuenta { get; set; }
        [NotMapped]
        public int TotalTransacciones { get; set; }

        public virtual Clientes IdClienteNavigation { get; set; }
        public virtual ICollection<Transacciones> Transacciones { get; set; }
    }
}
