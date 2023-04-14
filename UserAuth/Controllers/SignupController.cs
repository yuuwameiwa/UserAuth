using DataHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using StackExchange.Redis;

using UserAuth.Models;
using UserAuth.Services;

namespace UserAuth.Controllers
{
    public class SignupController : Controller
    {
        private readonly DataContainer _dataContainer;

        public SignupController()
        {
            _dataContainer.SqlConn = DatabaseConnectorService.TryConnectToSql();
            _dataContainer.Redis = DatabaseConnectorService.TryConnectToRedis();
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

            //IDatabase redis = DatabaseService.TryConnectToRedis();

            return RedirectToAction("Signin", "Signin");
        }
    }
}
