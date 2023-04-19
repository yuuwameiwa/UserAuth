using DataHelper.Attributes;

namespace UserAuth.Models
{
    // SqlAttribute contains:
    // Name of a table in Sql
    // Name of an identity column if has one
    [SqlTable("Users", "Id")]
    // Redis Attribute contains:
    // key:identity
    // expire time in seconds
    [RedisTable("users", "Login", 900)]
    public class UserModel
    {
        public int? Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}