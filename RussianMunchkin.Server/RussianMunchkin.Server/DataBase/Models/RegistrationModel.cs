using Repository;

namespace RussianMunchkin.Server.DataBase.Models
{
    public class RegistrationModel : IModel
    {
        [DbKey("login")] public string Login { get; set; }
        [DbKey("password")] public string Password { get; set; }
        [DbKey("user_id")] public int UserId { get; set; }
    }
}