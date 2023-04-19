using DataHelper;
using DataHelper.DataWriter;
using DataHelper.DataFinder;

using UserAuth.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace UserAuth.Services
{
    public class UserService : BaseDatabaseService
    {
        public UserService(DataContainer dataContainer) : base(dataContainer)
        {
        }

        public void RegisterUser(UserModel userModel)
        {
            if (_dataContainer.SqlConn != null && _dataContainer.Redis != null)
            {
                DataWriter dataWriter = new DataWriter(_dataContainer);
                int id = dataWriter.WriteToSqlWithIdentity(userModel);
                userModel.Id = id;
                dataWriter.WriteToRedis(userModel);
            }
        }

        public UserModel? FindUser(UserModel userModel)
        {
            if (_dataContainer.SqlConn != null && _dataContainer?.Redis != null)
            {
                string login = userModel.Login;
                string password = userModel.Password;

                UserModel? findedUser = FindUserByCreds(login, password);

                if (findedUser != null)
                {
                    // Write user to Redis
                    WriteUserToRedis(findedUser);
                    return findedUser;
                }
            }
            
            return null;
        }

        public UserModel? FindUserByCreds(string login, string password)
        {
            DataFinder dataFinder = new DataFinder(_dataContainer);

            // Создание анонимного класса. Первая строка - название таблицы. 
            // Остальные строки будут использоваться для WHERE в SQL запросе
            var findData = new
            {
                TableName = "Users",
                Login = login,
                Password = password
            };
            UserModel? findedUser = dataFinder.FindInSql<UserModel>(findData);

            if (findedUser != null)
                return findedUser;

            return null;
        }

        public void WriteUserToRedis(UserModel userModel)
        {
            DataWriter dataWriter = new DataWriter(_dataContainer);
            dataWriter.WriteToRedis(userModel);
        }

        public async Task<string> GetJwtFromApi(int id)
        {
            using HttpClient client = new HttpClient();
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7068/api/Auth/login/" + id.ToString(), content);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve JWT token. Status code: {response.StatusCode}");
            }
        }
    }
}
