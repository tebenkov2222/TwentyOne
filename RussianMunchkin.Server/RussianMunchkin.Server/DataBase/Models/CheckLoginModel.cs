using Repository;

namespace RussianMunchkin.Server.DataBase.Models
{
    public class CheckLoginModel : IModel
    {
        [DbKey("login")] public string Login { get; set; }
        [DbKey("login_exists")] public bool LoginExists { get; set; }
    }
}