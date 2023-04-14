using DataHelper;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;
using UserAuth.Models;

namespace UserAuth.Services
{
    public class UserService : BaseDatabaseService
    {
        public UserService(DataContainer dataContainer) : base(dataContainer)
        {
        }

        public void RegisterUser(UserModel userModel)
        {
            DataWriter dataWriter = new DataWriter(_dataContainer);
            dataWriter.WriteToSql(userModel, out int? id);

            if (id == null)
                throw new ArgumentNullException($"У {nameof(userModel)} не может быть null значения Id");

            userModel.Id = id;
            dataWriter.WriteToRedis(userModel);
        }
    }
}
