using CuentasAhorroApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CuentasAhorroApp.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
