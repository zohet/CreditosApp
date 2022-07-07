using System;
using System.Collections.Generic;


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
        public double Saldo { get; set; }
        public int IdCliente { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public bool EstatusCuenta { get; set; }

        public virtual Clientes IdClienteNavigation { get; set; }
        public virtual ICollection<Transacciones> Transacciones { get; set; }
    }
}
