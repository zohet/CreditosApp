using CuentasAhorroApp.Data;
using CuentasAhorroApp.Models;
using CuentasAhorroApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CuentasAhorroApp.Services
{
    [TypeFilter(typeof(ExceptionFilter))]
    public class ControlDatosService : IControlDatosInterface
    {
        #region Propiedades
        protected readonly DBContextCuentasAhorro _context;
        #endregion

        public ControlDatosService(DBContextCuentasAhorro context)
        {
            _context = context;
        }
        
        public List<Clientes> GetClientes()
        {
            var clientes = _context.Clientes.Where(d => d.EstatusCliente == true).ToList();
            foreach(var cliente in clientes)
            {
                cliente.TotalCuentasAhorro =
                    _context.CuentasDeAhorro.Where(d => d.EstatusCuenta == true && d.IdCliente == cliente.Id).Count();
                cliente.NombreCompleto =
                    $"{cliente.Nombre} {cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
            }
            return clientes;
        }

        public Response CreateCliente(Clientes data)
        {
            Response response = new Response();

            var updateData = _context.Clientes.Where(d => d.Id == data.Id).FirstOrDefault();
            if (updateData != null)
            {
                updateData.Nombre = data.Nombre;
                updateData.ApellidoPaterno = data.ApellidoPaterno;
                updateData.ApellidoMaterno = data.ApellidoMaterno;
                updateData.Telefono = data.Telefono;
                updateData.Email = data.Email;
                updateData.FechaActualizacion = DateTime.Now;
                _context.Clientes.Update(updateData);
            }
            else
            {
                var cliente = _context.Clientes.Where(d => d.EstatusCliente == true && (d.Email == data.Email || d.Telefono == data.Telefono)).FirstOrDefault();
                if (cliente != null)
                {
                    response.IsSuccess = false;
                    if (cliente.Email == data.Email)
                        response.Message = $"Ya existe una cliente con el correo: {cliente.Email} registrado";
                    else
                        response.Message = $"Ya existe una cliente con el telefono: {cliente.Telefono} registrado";
                    return response;
                }

                data.EstatusCliente = true;
                data.FechaCreacion = DateTime.Now;
                data.FechaActualizacion = DateTime.Now;
                _context.Clientes.Add(data);
                _context.SaveChanges();
                data.Identificador = $"CI-{data.Id}-{DateTime.Now.ToString("ddMMyyHHmmss")}";
                _context.Clientes.Update(data);
            }
            
            _context.SaveChanges();
            response.IsSuccess = true;
            response.Message = "Se guardo la información con éxito";
            return response;
        }

        public Response DeleteCliente(int Id)
        {
            Response response = new Response();
            var cliente = _context.Clientes.Where(d => d.Id == Id).FirstOrDefault();
            if (cliente == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }

            var cuentas =
                _context.CuentasDeAhorro.Where(d => d.IdCliente == Id && d.EstatusCuenta == true).ToList();

            if (cuentas.Count > 0)
            {
                response.IsSuccess = false;
                response.Message = "No se puede eliminar el usuario, tiene cuentas activas de ahorro";
                return response;
            }

            cliente.EstatusCliente = false;
            cliente.FechaActualizacion = DateTime.Now;
            _context.Clientes.Update(cliente);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Usuario dado de baja exitosamente";
            
            return response;
        }

        public Response GetCliente(int Id)
        {
            Response response = new Response();
            var cliente = _context.Clientes.Where(d => d.Id == Id && d.EstatusCliente == true).FirstOrDefault();
            if (cliente == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }
            
            response.IsSuccess = true;
            response.Result = cliente;
            return response;
        }
        
        public List<CuentasDeAhorro> GetCuentas(int IdCliente)
        {
            var cuentas = _context.CuentasDeAhorro.Where(d => d.IdCliente == IdCliente && d.EstatusCuenta == true).ToList();
            foreach (var cta in cuentas)
            {
                cta.TotalTransacciones =
                    _context.Transacciones.Where(d => d.IdCuentaDeAhorro == cta.Id).Count();
            }
            return cuentas;
        }

        public Response CreateCuenta(CuentasDeAhorro data)
        {
            Response response = new Response();

            var updateData = _context.CuentasDeAhorro.Where(d => d.Id == data.Id).FirstOrDefault();
            if (updateData != null)
            {
                updateData.NombreCuenta = data.NombreCuenta;
                updateData.FechaActualizacion = DateTime.Now;
                _context.CuentasDeAhorro.Update(updateData);
            }
            else
            {
                data.EstatusCuenta = true;
                data.Saldo = 0;
                data.FechaCreacion = DateTime.Now;
                data.FechaActualizacion = DateTime.Now;
                _context.CuentasDeAhorro.Add(data);
                _context.SaveChanges();
                data.NumeroCuenta = $"CA-{data.Id}-{DateTime.Now.ToString("ddMMyyHHmmss")}";
                _context.CuentasDeAhorro.Update(data);
            }

            _context.SaveChanges();
            response.IsSuccess = true;
            response.Message = "Se guardo la información con éxito";
            return response;
        }
        
        public Response DeleteCuenta(int Id)
        {
            Response response = new Response();
            var cuenta = _context.CuentasDeAhorro.Where(d => d.Id == Id).FirstOrDefault();
            if (cuenta == null)
            {
                response.IsSuccess = false;
                response.Message = "Cuenta no encontrado";
                return response;
            }

            if (cuenta.Saldo > 0)
            {
                response.IsSuccess = false;
                response.Message = "No se puede eliminar la cuenta, favor de vaciar la cuenta";
                return response;
            }

            cuenta.EstatusCuenta = false;
            cuenta.FechaActualizacion = DateTime.Now;
            _context.CuentasDeAhorro.Update(cuenta);
            _context.SaveChanges();

            response.IsSuccess = true;
            response.Message = "Cuenta dada de baja exitosamente";

            return response;
        }
        
        public Response GetCuenta(int Id)
        {
            Response response = new Response();
            var cuenta = _context.CuentasDeAhorro.Where(d => d.Id == Id && d.EstatusCuenta == true).FirstOrDefault();
            if (cuenta == null)
            {
                response.IsSuccess = false;
                response.Message = "Cuenta no encontrado";
                return response;
            }

            response.IsSuccess = true;
            response.Result = cuenta;
            return response;
        }

        public List<Transacciones> GetTransacciones(int IdCuenta)
        {
            var result = _context.Transacciones.Where(d => d.IdCuentaDeAhorro == IdCuenta).ToList();
            foreach (var tra in result)
            {
                tra.TipoTransaccion = tra.Tipo == true ? "Deposito" : "Retiro";
            }
            return result;
        }

        public Response CreateTransaccion(Transacciones data)
        {
            Response response = new Response();

            var cuenta = _context.CuentasDeAhorro.Where(d => d.Id == data.IdCuentaDeAhorro).FirstOrDefault();
            if (cuenta == null)
            {
                response.IsSuccess = false;
                response.Message = "Cuenta no encontrada";
                return response;
            }

            if (data.Tipo)
            {
                cuenta.Saldo += data.Monto;
                cuenta.FechaActualizacion = DateTime.Now;
                _context.CuentasDeAhorro.Update(cuenta);
            }
            else
            {
                if (cuenta.Saldo < data.Monto)
                {
                    response.IsSuccess = false;
                    response.Message = "No se puede realizar la transacción, saldo insuficiente";
                    return response;
                }
                cuenta.Saldo -= data.Monto;
                cuenta.FechaActualizacion = DateTime.Now;
                _context.CuentasDeAhorro.Update(cuenta);
            }

            data.FechaCreacion = DateTime.Now;
            _context.Transacciones.Add(data);
            _context.SaveChanges();
            response.IsSuccess = true;
            response.Message = "Se guardo la información con éxito";
            return response;
        }

        public Response GetNombreCliente(int Id)
        {
            Response response = new Response();
            var cliente = _context.Clientes.Where(d => d.Id == Id && d.EstatusCliente == true).FirstOrDefault();
            if (cliente == null)
            {
                return response;
            }

            response.IsSuccess = true;
            response.Message = $"{cliente.Nombre} {cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
            return response;
        }

        public Response GetNombreCuenta(int IdCuenta, int IdCliente)
        {
            Response response = new Response();
            var cuenta = _context.CuentasDeAhorro.Where(d => d.Id == IdCuenta && d.EstatusCuenta == true).FirstOrDefault();
            if (cuenta == null)
            {
                return response;
            }

            var cliente = _context.Clientes.Where(d => d.Id == IdCliente).FirstOrDefault();

            if (cliente == null)
            {
                return response;
            }


            response.IsSuccess = true;
            response.Message = cuenta.NombreCuenta;
            response.Result = $"{cliente.Nombre} {cliente.ApellidoPaterno} {cliente.ApellidoMaterno}";
            return response;
        }
    }
}
