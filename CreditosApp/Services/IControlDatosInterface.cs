using CuentasAhorroApp.Models;
using CuentasAhorroApp.ViewModels;
using System.Collections.Generic;

namespace CuentasAhorroApp.Services
{
    public interface IControlDatosInterface
    {
        List<Clientes> GetClientes();
        Response CreateCliente(Clientes data);
        Response DeleteCliente(int Id);
        Response GetCliente(int Id);
        Response GetNombreCliente(int Id);
        Response GetNombreCuenta(int IdCuenta, int IdCliente);
        List<CuentasDeAhorro> GetCuentas(int IdCliente);
        Response CreateCuenta(CuentasDeAhorro data);
        Response DeleteCuenta(int Id);
        Response GetCuenta(int Id);
        List<Transacciones> GetTransacciones(int IdCuenta);
        Response CreateTransaccion(Transacciones data);
    }
}
