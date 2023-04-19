using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using UserAuth.Services;

namespace UserAuth.Controllers
{
    [Authorize]
    public class TestJwtController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
