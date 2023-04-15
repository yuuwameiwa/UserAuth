using DataHelper;
using System.Reflection;
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
            if (_dataContainer.SqlConn != null && _dataContainer.Redis != null)
            {
                DataWriter dataWriter = new DataWriter(_dataContainer);
                int id = dataWriter.WriteToSqlWithIdentity(userModel);
                userModel.Id = id;
                dataWriter.WriteToRedis(userModel);
            }
        }

        public void FindUser(UserModel userModel)
        {
            if (_dataContainer.SqlConn != null && _dataContainer?.Redis != null)
            {
                DataFinder dataFinder = new DataFinder(_dataContainer);

                TableAttribute? tableAttr = userModel.GetType().GetCustomAttribute<TableAttribute>();
                string tableName = tableAttr.TableName;

                var findData = new
                {
                    TableName = tableName,
                    Name = userModel.Name,
                    Password = userModel.Password,
                };

                UserModel findedUser = dataFinder.FindInSql<UserModel>(findData);
                Console.WriteLine('a');
            }
        }
    }
}
