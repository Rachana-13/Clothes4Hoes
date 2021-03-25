using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clothes4Hoes.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{StatusCode}")]
        public IActionResult Index(int StatusCode)
        {
            string Message = string.Empty;

            switch (StatusCode)
            {
                case 404:
                    Message = "helaas pagina niet gevonden";
                    break;
            }
            return View("NotFound");
        }
    }
}
