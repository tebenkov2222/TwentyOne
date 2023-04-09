namespace RussianMunchkin.Server.Player.Auth.AuthFull
{
    public class PlayerAuthFullModel
    {
        public bool IsAuthorized { get; set; }
        public int UserId { get; set; }
        public string Login { get; set; }
    }
}