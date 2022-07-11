using CuentasAhorroApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace CuentasAhorroApp.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
    public class TransaccionesController : Controller
    {
        public IControlDatosInterface _controlDatosService;
        public TransaccionesController(IControlDatosInterface controlDatosService)
        {
            _controlDatosService = controlDatosService;
        }
        public IActionResult Index(int IdCuenta, int IdUsuario)
        {
            var userResult = _controlDatosService.GetNombreCuenta(IdCuenta, IdUsuario);
            string NombreUsuario = "Usuario error";
            string NombreCuenta = "Cuenta no existente";
            if (userResult.IsSuccess)
            {
                NombreCuenta = userResult.Message;
                NombreUsuario = userResult.Result.ToString();
            }
            ViewBag.NombreCuenta = NombreCuenta;
            ViewBag.UsuarioNombre = NombreUsuario;
            ViewBag.IdCuenta = IdCuenta;
            ViewBag.IdUsuario = IdUsuario;
            return View();
        }
        public IActionResult GetAllTransacciones(int? page, int IdCuenta, int IdUsuario, bool Filter, string SearchData)
        {
            IPagedList<Models.Transacciones> result = new List<Models.Transacciones>().ToPagedList();

            var pageNumber = page ?? 1;
            int pageSize = 10;
            var lstClientes = _controlDatosService.GetTransacciones(IdCuenta);
            if (Filter)
            {
                SearchData = SearchData.ToLower();
                result = lstClientes.Where(w => w.TipoTransaccion.ToLower().Contains(SearchData)
                || w.Monto.ToString().Contains(SearchData)
                || w.FechaCreacion.ToString("dd/MM/yyyy").Contains(SearchData))
                    .OrderByDescending(w => w.FechaCreacion).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                result = lstClientes.OrderByDescending(w => w.FechaCreacion).ToPagedList(pageNumber, pageSize);
            }

            ViewBag.PageCount = result.PageCount;
            ViewBag.PageNumber = result.PageNumber;
            ViewBag.TotalItemCount = result.TotalItemCount;

            return PartialView("~/Views/Transacciones/_ListaTransacciones.cshtml", result);
        }

        [HttpPost]
        public JsonResult SaveOrUpdateTransacciones(Models.Transacciones data)
        {
            var result = _controlDatosService.CreateTransaccion(data);
            return Json(result);
        }
    }
}
