using Microsoft.AspNetCore.Mvc;
using DataHelper;

using UserAuth.Models;
using UserAuth.Services;

namespace UserAuth.Controllers
{
    public class SigninController : Controller
    {
        private DataContainer _dataContainer;

        public SigninController()
        {
            DataContainer dataContainer = new DataContainer(
                DatabaseConnectorService.TryConnectToSql(),
                DatabaseConnectorService.TryConnectToRedis());

            _dataContainer = dataContainer;
        }

        public IActionResult Index()
        {
            return View("Views/Signin/Signin.cshtml");
        }

        [HttpPost]
        public IActionResult Signin(UserModel userModel)
        {
            UserService userService = new UserService(_dataContainer);
            userService.FindUser(userModel);

            return null;
        }
    }
}
