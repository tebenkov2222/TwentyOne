
namespace RussianMunchkin.Server.Player.Auth.AuthFull
{
    public interface IClientAuthFullController
    {
        public PlayerAuthFullModel AuthModel { get; }

        public void LogIn(PlayerAuthFullModel authFullModel);
        public void LogOut();
    }
}