using DataHelper;

namespace UserAuth.Models
{
    // DataAttribute must contain:
    // Name of a table in Sql
    // Name of an identity column if has one
    [Table("Users", "Id")]
    public class UserModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = "";
        public string Password { get; set; }
    }
}