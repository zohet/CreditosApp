using CuentasAhorroApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CuentasAhorroApp.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
    public class CuentasAhorroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
