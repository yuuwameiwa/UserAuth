using DataHelper;

namespace UserAuth.Models
{
    // TableAttribute must contain:
    // Name of a table in Sql
    // Name of an identity columns
    [Table("Users", "Id")]
    public class UserModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; }
    }
}