using Microsoft.AspNetCore.Mvc;

using DataHelper;

using UserAuth.Models;
using UserAuth.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace UserAuth.Controllers
{
    public class SigninController : Controller
    {
        private readonly DataContainer _dataContainer;

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
        public async Task<IActionResult> SigninAsync(UserModel userModel)
        {
            UserService userService = new UserService(_dataContainer);
            UserModel? findedUser =  userService.FindUser(userModel);

            if (findedUser != null && findedUser.Id != null)
            {
                string token = userService.GetJwtFromApi((int)findedUser.Id).Result;

                HttpContext.Response.Cookies.Append("jwt", token,
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromMinutes(60)
                });
            }

            return RedirectToAction("Index", "TestJwt");
        }
    }
}
