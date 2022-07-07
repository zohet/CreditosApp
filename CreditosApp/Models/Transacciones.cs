using System;
using System.Collections.Generic;

#nullable disable

namespace CuentasAhorroApp.Models
{
    public partial class Transacciones
    {
        public int Id { get; set; }
        public bool Tipo { get; set; }
        public double Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdCuentaDeAhorro { get; set; }

        public virtual CuentasDeAhorro IdCuentaDeAhorroNavigation { get; set; }
    }
}
