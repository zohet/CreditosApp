using CuentasAhorroApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace CuentasAhorroApp.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
    public class ClientesController : Controller
    {
        public IControlDatosInterface _controlDatosService;
        public ClientesController(IControlDatosInterface controlDatosService)
        {
            _controlDatosService = controlDatosService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAllClientes(int? page, bool Filter, string SearchData)
        {
            IPagedList<Models.Clientes> result = new List<Models.Clientes>().ToPagedList();
            
            var pageNumber = page ?? 1;
            int pageSize = 10;
            var lstClientes = _controlDatosService.GetClientes();
            if (Filter)
            {
                SearchData = SearchData.ToLower();
                result = lstClientes.Where(w => w.Identificador.ToLower().Contains(SearchData) 
                || w.NombreCompleto.ToLower().Contains(SearchData) 
                || w.Telefono.ToLower().Contains(SearchData)
                || w.Email.ToLower().Contains(SearchData))
                    .OrderByDescending(w => w.FechaActualizacion).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                result = lstClientes.OrderByDescending(w => w.FechaActualizacion).ToPagedList(pageNumber, pageSize);
            }

            ViewBag.PageCount = result.PageCount;
            ViewBag.PageNumber = result.PageNumber;
            ViewBag.TotalItemCount = result.TotalItemCount;

            return PartialView("~/Views/Clientes/_ListaClientes.cshtml", result);
        }

        [HttpPost]
        public JsonResult SaveOrUpdateCliente(Models.Clientes data)
        {
            var result = _controlDatosService.CreateCliente(data);
            return Json(result);
        }

        [HttpPost]
        public JsonResult BajaUsuario(int Id)
        {
            var result = _controlDatosService.DeleteCliente(Id);
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetCliente(int Id)
        {
            var result = _controlDatosService.GetCliente(Id);
            return Json(result);
        }
    }
}
