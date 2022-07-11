using CuentasAhorroApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace CuentasAhorroApp.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
    public class CuentasAhorroController : Controller
    {
        public IControlDatosInterface _controlDatosService;
        public CuentasAhorroController(IControlDatosInterface controlDatosService)
        {
            _controlDatosService = controlDatosService;
        }
        public IActionResult Index(int IdCliente)
        {
            var userResult = _controlDatosService.GetNombreCliente(IdCliente);
            string Nombre = "Usuario no valido";
            if(userResult.IsSuccess)
                Nombre = userResult.Message;
            ViewBag.UsuarioNombre = Nombre;
            ViewBag.IdCliente = IdCliente;
            return View();
        }

        public IActionResult GetAllCuentas(int? page,int IdCliente, bool Filter, string SearchData)
        {
            IPagedList<Models.CuentasDeAhorro> result = new List<Models.CuentasDeAhorro>().ToPagedList();

            var pageNumber = page ?? 1;
            int pageSize = 10;
            var lstClientes = _controlDatosService.GetCuentas(IdCliente);
            if (Filter)
            {
                SearchData = SearchData.ToLower();
                result = lstClientes.Where(w => w.NumeroCuenta.ToLower().Contains(SearchData)
                || w.Saldo.ToString().Contains(SearchData)
                || w.NombreCuenta.ToString().Contains(SearchData))
                    .OrderByDescending(w => w.FechaActualizacion).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                result = lstClientes.OrderByDescending(w => w.FechaActualizacion).ToPagedList(pageNumber, pageSize);
            }

            ViewBag.PageCount = result.PageCount;
            ViewBag.PageNumber = result.PageNumber;
            ViewBag.TotalItemCount = result.TotalItemCount;

            return PartialView("~/Views/CuentasAhorro/_ListaCuentasAhorro.cshtml", result);
        }

        [HttpPost]
        public JsonResult SaveOrUpdateCuentaAhorro(Models.CuentasDeAhorro data)
        {
            var result = _controlDatosService.CreateCuenta(data);
            return Json(result);
        }

        [HttpPost]
        public JsonResult BajaCuenta(int Id)
        {
            var result = _controlDatosService.DeleteCuenta(Id);
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetCuentaAhorro(int Id)
        {
            var result = _controlDatosService.GetCuenta(Id);
            return Json(result);
        }
    }
}
