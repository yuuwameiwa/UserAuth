using Microsoft.AspNetCore.Mvc;

using UserAuth.Models;

namespace UserAuth.Controllers
{
    public class SigninController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signin(UserModel userModel)
        {
            return null;
        }
    }
}
