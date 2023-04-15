using Microsoft.AspNetCore.Mvc;
using DataHelper;

using UserAuth.Models;
using UserAuth.Services;

namespace UserAuth.Controllers
{
    public class SignupController : Controller
    {
        private DataContainer _dataContainer;

        public SignupController()
        {
            DataContainer dataContainer = new DataContainer(
                DatabaseConnectorService.TryConnectToSql(), 
                DatabaseConnectorService.TryConnectToRedis());

            _dataContainer = dataContainer;
        }

        public IActionResult Index()
        {
            return View("Views/Signup/signup.cshtml");
        }

        [HttpPost]
        public IActionResult Signup(UserModel userModel)
        {
            UserService userService = new UserService(_dataContainer);
            userService.RegisterUser(userModel);

            return RedirectToAction("Index", "Signin");
        }
    }
}
