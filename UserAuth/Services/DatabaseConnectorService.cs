using DataHelper;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;

namespace UserAuth.Services
{
    public static class DatabaseConnectorService
    {
        public static SqlConnection TryConnectToSql()
        {
            Func<string, SqlConnection> funcSql = (string connectionString) =>
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                try
                {
                    sqlConnection.Open();
                    return sqlConnection;
                }
                catch
                {
                    return null;
                }
            };

            return DataLoader.TryGetFromFile<SqlConnection>(@"connectionsettings.json", funcSql);
        }

        public static IDatabase TryConnectToRedis()
        {
            Func<string, IDatabase> funcRedis = (string connectionString) =>
            {
                ConfigurationOptions options = new ConfigurationOptions
                {
                    EndPoints = { connectionString },
                    ConnectTimeout = 5000
                };

                try
                {
                    ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
                    return connection.GetDatabase();
                }
                catch
                {
                    return null;
                }
            };

            return DataLoader.TryGetFromFile<IDatabase>("connectionsettings.json", funcRedis);
        }
    }
}
